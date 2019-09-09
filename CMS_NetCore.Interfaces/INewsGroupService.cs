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
        DataGridViewModel<NewsGroup> GetBySearch(int page, int pageSize, string searchString);
        NewsGroup GetById(int? id);
        void Add(NewsGroup newsGroup);
        void Edit(NewsGroup newsGroup);
        void Delete(NewsGroup newsGroup);
        void Delete(int? id);
        bool UniqueAlias(string aliasName, int? newsGroupId);

        IEnumerable<NewsGroup> NewsGroups();

        
    }
}
