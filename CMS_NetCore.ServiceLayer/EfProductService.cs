using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CMS_NetCore.DataLayer;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using CMS_NetCore.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.ServiceLayer;

public class EfProductService : RepositoryBase<Product>, IProductService
{
    private readonly IProductGalleryService _productGalleryService;
    private readonly IProductTagService _productTagService;
    private readonly IAttributeGroupService _attributeGroupService;
    private readonly IProductAttributeService _productAttributeService;

    public EfProductService(
        AppDbContext context,
        IProductGalleryService productGalleryService,
        IProductTagService productTagService,
        IProductAttributeService productAttributeService,
        IAttributeGroupService attributeGroupService
    ) : base(context)
    {
        _productGalleryService = productGalleryService;
        _productTagService = productTagService;
        _productAttributeService = productAttributeService;
        _attributeGroupService = attributeGroupService;
    }

    public async Task<DataGridViewModel<Product>> GetBySearch(
        int page,
        int pageSize,
        string searchString
    )
    {
        return new DataGridViewModel<Product>
        {
            Records = await FindByCondition(
                    product => product.Name
                                   .Contains(searchString) ||
                               product.Title.Contains(searchString)
                )
                .OrderBy(product => product.ModifiedDate)
                .Include(product => product.ProductGroup)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(),

            TotalCount = await FindByCondition(
                    product =>
                        product.Name.Contains(searchString) ||
                        product.Title.Contains(searchString)
                ).OrderBy(product => product.ModifiedDate)
                .Include(product => product.ProductGroup)
                .CountAsync()
        };
    }

    public async Task Add(
        Product product,
        string galleyFiles,
        string tags
    )
    {
        product.CreatedDate = DateTime.Now;
        product.ModifiedDate = DateTime.Now;

        //-------------------Tags---------------------
        if (!string.IsNullOrEmpty(tags))
        {
            foreach (var tag in tags.Split('-'))
            {
                product.ProductTags.Add(
                    new ProductTag
                    {
                        ProductId = product.Id,
                        TagTitle = tag.ToLowerInvariant().Trim()
                    }
                );
            }
        }

        //------------Create Gallery Product --------------
        if (!string.IsNullOrEmpty(galleyFiles))
        {
            var gallery = galleyFiles.Split(',');
            for (var i = 0; i < gallery.Length - 1; i++)
            {
                product.ProductGalleries.Add(
                    new ProductGallery
                    {
                        ProductId = product.Id,
                        ImageName = gallery[i],
                    }
                );
            }
        }

        Create(product);
        await SaveAsync();
    }

    public async Task Edit(
        Product product,
        string galleryFiles,
        string tags
    )
    {
        //-------------------Tags---------------------
        await _productTagService.DeleteByProductId(product.Id);

        if (!string.IsNullOrEmpty(tags))
        {
            var productTags = tags.Split('-')
                .Select(
                    tag => new ProductTag
                    {
                        ProductId = product.Id,
                        TagTitle = tag.ToLowerInvariant().Trim()
                    }
                )
                .ToList();

            _productTagService.Add(productTags);
        }

        //------------Create Gallery Product --------------
        if (!string.IsNullOrEmpty(galleryFiles))
        {
            var productGalleries = galleryFiles.Split(',')
                .TakeWhile(gallery => !string.IsNullOrEmpty(gallery))
                .Select(
                    gallery => new ProductGallery
                    {
                        ProductId = product.Id,
                        ImageName = gallery,
                    }
                )
                .ToList();

            await _productGalleryService.Add(productGalleries);
        }

        product.ModifiedDate = DateTime.Now;
        Update(product);
        await SaveAsync();
    }

    public async Task Remove(
        Product product,
        string webRootPath
    )
    {
        //-------------------------delete tags----------------------------------------
        await _productTagService.DeleteByProductId(product.Id);

        //-------------------------delete gallery----------------------------------------
        foreach (var gallery in product.ProductGalleries.ToList())
        {
            if (File.Exists(
                    Path.Combine(
                        webRootPath,
                        "Upload\\ProductImages",
                        gallery.ImageName
                    )
                ))
                File.Delete(
                    Path.Combine(
                        webRootPath,
                        "Upload\\ProductImages",
                        gallery.ImageName
                    )
                );
            if (File.Exists(
                    Path.Combine(
                        webRootPath,
                        "Upload\\ProductImages\\thumbnail",
                        gallery.ImageName
                    )
                ))
                File.Delete(
                    Path.Combine(
                        webRootPath,
                        "Upload\\ProductImages\\thumbnail",
                        gallery.ImageName
                    )
                );

            await _productGalleryService.Remove(gallery);
        }

        //-------------------------delete Images----------------------------------------
        if (product.Image != "no-photo.jpg")
        {
            if (File.Exists(
                    Path.Combine(
                        webRootPath,
                        "Upload\\ProductImages",
                        product.Image
                    )
                ))
                File.Delete(
                    Path.Combine(
                        webRootPath,
                        "Upload\\ProductImages",
                        product.Image
                    )
                );
            if (File.Exists(
                    Path.Combine(
                        webRootPath,
                        "Upload\\ProductImages\\thumbnail",
                        product.Image
                    )
                ))
                File.Delete(
                    Path.Combine(
                        webRootPath,
                        "Upload\\ProductImages\\thumbnail",
                        product.Image
                    )
                );
        }

        //-----------------delete attribute ---------------------------------------------
        foreach (var attributeGroup in await _attributeGroupService.GetByProductGroupId(product.ProductGroupId))
        {
            var productAttribute = await _productAttributeService.GetProductAttribute(
                product.Id,
                attributeGroup.Id
            );

            if (productAttribute != null)
                _productAttributeService.Remove(productAttribute);
        }

        //delete selected product
        Delete(product);
        await SaveAsync();
    }

    public async Task<Product> GetById(int? id) =>
        await FindByCondition(product => product.Id.Equals(id))
            .FirstOrDefaultAsync();

    public async Task<bool> UniqueAlias(
        string aliasName,
        int? productId
    ) =>
        await FindByCondition(
            product =>
                product.AliasName == aliasName &&
                product.Id != productId
        ).AnyAsync();

    public async Task<IEnumerable<Product>> GetAll() =>
        await FindAll().ToListAsync();

    public async Task<bool> IsExist(int? id) =>
        await FindByCondition(product => product.Id == id).AnyAsync();

    public async Task<Product> GetIncludeById(int? id) =>
        await FindByCondition(product => product.Id == id)
            .Include(product => product.ProductTags)
            .Include(product => product.ProductGalleries)
            .FirstOrDefaultAsync();

    public async Task DeleteImage(
        int id,
        string webRootPath
    )
    {
        var gallery = await _productGalleryService.GetById(id);

        await _productGalleryService.Remove(gallery);

        if (File.Exists(
                Path.Combine(
                    webRootPath,
                    "Upload\\ProductImages",
                    gallery.ImageName
                )
            ))
            File.Delete(
                Path.Combine(
                    webRootPath,
                    "Upload\\ProductImages",
                    gallery.ImageName
                )
            );
        if (File.Exists(
                Path.Combine(
                    webRootPath,
                    "Upload\\ProductImages\\thumbnail",
                    gallery.ImageName
                )
            ))
            File.Delete(
                Path.Combine(
                    webRootPath,
                    "Upload\\ProductImages\\thumbnail",
                    gallery.ImageName
                )
            );
    }
}