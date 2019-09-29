using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using  CMS_NetCore.ViewModels;

namespace CMS_NetCore.Interfaces
{
    public interface INewsGroupService
    {
        Task<DataGridViewModel<NewsGroup>> GetBySearch(int page, int pageSize, string searchString);
        Task<NewsGroup> GetById(int? id);
        Task Add(NewsGroup newsGroup);
        Task Edit(NewsGroup newsGroup);
        Task Remove(NewsGroup newsGroup);
        Task<bool> UniqueAlias(string aliasName, int? newsGroupId);
        Task<IEnumerable<NewsGroup>> GetAll();

        
    }
}
