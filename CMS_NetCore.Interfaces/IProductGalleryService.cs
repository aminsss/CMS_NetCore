using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;

namespace CMS_NetCore.Interfaces
{
    public interface IProductGalleryService
    {
        void Remove(ProductGallery productGallery);
        Task<ProductGallery> GetById(int id);
        void Add(IList<ProductGallery> productGalleries);
    }
}
