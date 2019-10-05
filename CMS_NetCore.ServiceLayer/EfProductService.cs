using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.DataLayer;
using CMS_NetCore.Interfaces;
using CMS_NetCore.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.AspNetCore.Hosting;


namespace CMS_NetCore.ServiceLayer
{
    public class EfProductService : RepositoryBase<Product>,IProductService
    {
        private readonly IProductGalleryService _productGalleryService;
        private readonly IProductTagService _productTagService;
        private readonly IAttributeGrpService _attributeGrpService;
        private readonly IProductAttributeService _productAttributeService;
        private readonly IHostingEnvironment _env;

        public EfProductService(AppDbContext context, IProductGalleryService productGalleryService,
            IProductTagService productTagService ,IProductAttributeService productAttributeService,
            IAttributeGrpService attributeGrpService,IHostingEnvironment env) : base(context)
        {
            _productGalleryService = productGalleryService;
            _productTagService = productTagService;
            _productAttributeService = productAttributeService;
            _attributeGrpService = attributeGrpService;
            _env = env;
        }

        public async Task<DataGridViewModel<Product>> GetBySearch(int page, int pageSize, string searchString)
        {
            var DataGridView = new DataGridViewModel<Product>
            {
                Records = await FindByCondition(s=>s.ProductName.Contains(searchString) ||
                s.ProductTitle.Contains(searchString)).OrderBy(o=>o.ModifiedDate).Include(x=>x.ProductGroup)
                .Skip((page -1) * pageSize).Take(pageSize).ToListAsync(),

                TotalCount = await FindByCondition(s => s.ProductName.Contains(searchString) ||
                s.ProductTitle.Contains(searchString)).OrderBy(o => o.ModifiedDate)
                .Include(x => x.ProductGroup).CountAsync()
            };

            return DataGridView;
        }
        public async Task Add(Product product,string galleyFiles, string tags)
        {
            product.AddedDate = DateTime.Now;
            product.ModifiedDate = DateTime.Now;

            //------------Create Gallery Product --------------
            var Gallery = galleyFiles.Split(',');
            for (int i = 0; i < Gallery.Length - 1; i++)
            {
                product.ProductGallery.Add(new ProductGallery
                {
                    ProductId = product.ProductId,
                    ImageName = Gallery[i],
                });
            }

            //-------------------Tags---------------------
            if (!string.IsNullOrEmpty(tags))
            {
                foreach (string tag in tags.Split('-'))
                {
                    product.ProductTag.Add(new ProductTag()
                    {
                        ProductId = product.ProductId,
                        TagTitle = tag.ToLowerInvariant().Trim()
                    });
                }
            }
            
            Create(product);
            await SaveAsync();
        }

        public async Task Edit(Product product, string galleyFiles, string tags)
        {
            //-------------------Tags---------------------
            await _productTagService.DeleteByProductId(product.ProductId);

            if (!string.IsNullOrEmpty(tags))
            {
                List<ProductTag> productTags = new List<ProductTag>();
                foreach (string tag in tags.Split('-'))
                {
                    productTags.Add(new ProductTag
                    {
                        ProductId = product.ProductId,
                        TagTitle = tag.ToLowerInvariant().Trim()
                    });
                }
                _productTagService.Add(productTags);
            };


            //------------Create Gallery Product --------------
            List<ProductGallery> productGalleries = new List<ProductGallery>();
            var Gallery = galleyFiles.Split(',');
            for (int i = 0; i < Gallery.Length - 1; i++)
            {
                productGalleries.Add(new ProductGallery
                {
                    ProductId = product.ProductId,
                    ImageName = Gallery[i],
                });
            }
            _productGalleryService.Add(productGalleries);

            product.ModifiedDate = DateTime.Now;
            Update(product);
            await SaveAsync();
        }

        public async Task Remove(Product product)
        {
            //-------------------------delete tags----------------------------------------
            await _productTagService.DeleteByProductId(product.ProductId);

            //-------------------------delete gallery----------------------------------------
            foreach (var gallery in product.ProductGallery.ToList())
            {
                if (File.Exists(Path.Combine(_env.WebRootPath, "Upload\\ProductImages", gallery.ImageName)))
                    File.Delete(Path.Combine(_env.WebRootPath, "Upload\\ProductImages", gallery.ImageName));
                if (File.Exists(Path.Combine(_env.WebRootPath, "Upload\\ProductImages\\thumbnail", gallery.ImageName)))
                    File.Delete(Path.Combine(_env.WebRootPath, "Upload\\ProductImages\\thumbnail", gallery.ImageName));

                _productGalleryService.Remove(gallery);
            }
           
            //-------------------------delete Images----------------------------------------
            if (product.ProductImage != "no-photo.jpg")
            {
                if (File.Exists(Path.Combine(_env.WebRootPath, "Upload\\ProductImages", product.ProductImage)))
                    File.Delete(Path.Combine(_env.WebRootPath, "Upload\\ProductImages", product.ProductImage));
                if (File.Exists(Path.Combine(_env.WebRootPath, "Upload\\ProductImages\\thumbnail", product.ProductImage)))
                    File.Delete(Path.Combine(_env.WebRootPath, "Upload\\ProductImages\\thumbnail", product.ProductImage));
            }

            //-----------------delete attribute ---------------------------------------------
            foreach (var atrgrp in await _attributeGrpService.GetByProductGroupId(product.ProductGroupId))
            {
                var find = await  _productAttributeService.GetProductAttribute(product.ProductId, atrgrp.AttributGrpId);
                if (find != null)
                {
                    _productAttributeService.Remove(find);
                }
            }

            //-----------------delete details---------------------
            

            //delete selected product
            Delete(product);
            await SaveAsync();
        }

        public async Task<Product> GetById(int? id) =>
            await FindByCondition(x=>x.ProductId.Equals(id)).DefaultIfEmpty(new Product()).SingleAsync();

        public async Task<bool> UniqueAlias(string aliasName, int? productId) =>
            await FindByCondition(s => s.AliasName == aliasName && s.ProductId != productId).AnyAsync();

        public async Task<IEnumerable<Product>> GetAll() =>
            await FindAll().ToListAsync();

        public async Task<bool> ProductExsitence(int? id) =>
            await FindByCondition(x => x.ProductId == id).AnyAsync();
        
    }
}
