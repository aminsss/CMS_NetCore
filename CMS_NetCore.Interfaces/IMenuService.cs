using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using  CMS_NetCore.ViewModels;

namespace CMS_NetCore.Interfaces
{
    public interface IMenuService
    {
        DataGridViewModel<Menu> GetByMenuGroup(int? menuGroupId);
        Menu GetById(int? id);
        IList<Menu> GetByParentId(int? parentId);
        Menu GetLastOrder(int? parentId,int? menuGroupId);
        IList<Menu> GetByParentGroupOrder(int? parentId, int? menuGroupId, int? pastDisOrder);
        void Add(Menu menu);
        void Edit(Menu menu, int? pastDisOrder, int? pastParentId, int? pastGroupId);
        void Delete(Menu menu);
        void Delete(int id);
        bool UniquePageName(string pageName, int? menuId);
        IEnumerable<Menu> menus();
    }
}
