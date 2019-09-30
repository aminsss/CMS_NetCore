using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using  CMS_NetCore.ViewModels;

namespace CMS_NetCore.Interfaces
{
    public interface INewsGalleryService
    {
        Task<NewsGallery> GetById(int? id);
        Task Remove(NewsGallery newsGallery);
        Task Add(IList<NewsGallery> newsGalleries);
    }
}
