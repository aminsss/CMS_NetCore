using System.Collections.Generic;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;

namespace CMS_NetCore.Interfaces;

public interface INewsGalleryService
{
    Task<NewsGallery> GetById(int? id);
    Task Remove(NewsGallery newsGallery);
    Task Add(IList<NewsGallery> newsGalleries);
}