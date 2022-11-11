using System.Collections.Generic;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;

namespace CMS_NetCore.Interfaces;

public interface IProductGalleryService
{
    Task Remove(ProductGallery productGallery);
    Task<ProductGallery> GetById(int id);
    Task Add(IList<ProductGallery> productGalleries);
}