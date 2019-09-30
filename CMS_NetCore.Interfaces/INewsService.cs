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
        Task<DataGridViewModel<News>> GetBySearch(int page, int pageSize, string searchString);
        Task<News> GetById(int? id);
        Task Add(News news);
        Task Edit(News news);
        Task Remove(News news);
        Task<bool> UniqueAlias(string aliasName, int? newsId);
        Task<bool> NewsExistence(int? id);

    }
}
