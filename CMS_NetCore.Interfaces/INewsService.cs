using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using  CMS_NetCore.ViewModels;

namespace CMS_NetCore.Interfaces
{
    public interface INewsService
    {
        DataGridViewModel<News> GetBySearch(int page, int pageSize, string searchString);
        News GetById(int? id);
        void Add(News news);
        void Edit(News news);
        void Delete(News news);
        void Delete(int? id);
        bool UniqueAlias(string aliasName, int? newsId);

        //NewsTags
        void DeleteTagsByNews(int? newsId);
        void AddTags(IList<NewsTag> newsTags);

        //NewsGaalery
        NewsGallery GetNewsGalleryById(int? id);
        void DeleteNewGalery(NewsGallery newsGallery);
        void AddGallery(IList<NewsGallery> newsGalleries);
    }
}
