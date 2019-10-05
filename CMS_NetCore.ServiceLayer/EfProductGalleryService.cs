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

namespace CMS_NetCore.ServiceLayer
{
    public class EfProductGalleryService : RepositoryBase<ProductGallery>,IProductGalleryService
    {

        public EfProductGalleryService(AppDbContext context) : base(context)
        {
        }

        public void Remove(ProductGallery productGallery)
        {
            Delete(productGallery);
        }

        public async Task<ProductGallery> GetById(int id) =>
            await FindByCondition(x=>x.ProductGalleryId.Equals(id)).DefaultIfEmpty(new ProductGallery()).SingleAsync();

        public void Add(IList<ProductGallery> productGalleries)
        {
            foreach (var productGallery in productGalleries)
            {
                Create(productGallery);
            }
        }
    }
}
