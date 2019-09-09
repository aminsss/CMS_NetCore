using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using  CMS_NetCore.ViewModels;

namespace CMS_NetCore.Interfaces
{
    public interface IMenuGroupService
    {
        DataGridViewModel<MenuGroup> GetBySearch(int? page, int? pageSize, string searchString);
        MenuGroup GetById(int? id);
        void Add(MenuGroup menuGroup);
        void Edit(MenuGroup menuGroup);
        void Delete(MenuGroup menuGroup);
        void Delete(int? id);
        IEnumerable<MenuGroup> MenuGroup();
    }
}
