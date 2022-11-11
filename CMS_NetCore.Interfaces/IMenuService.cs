using System.Collections.Generic;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.ViewModels;

namespace CMS_NetCore.Interfaces;

public interface IMenuService
{
    Task<DataGridViewModel<Menu>> GetByMenuGroup(int? menuGroupId);
    Task<Menu> GetById(int? id);

    Task<IList<Menu>> GetByParentId(
        int? parentId,
        int? menuGroupId
    );

    Task<Menu> GetLastOrder(
        int? parentId,
        int? menuGroupId
    );

    Task<IList<Menu>> GetByParentGroupOrder(
        int? parentId,
        int? menuGroupId,
        int? pastDisOrder
    );

    Task Add(Menu menu);

    Task Edit(
        Menu menu,
        int? pastDisplayOrder,
        int? pastParentId,
        int? pastGroupId
    );

    Task<Menu> GetByPageName(string pageName);
    Task Remove(Menu menu);

    Task<bool> UniquePageName(
        string pageName,
        int? menuId
    );

    Task<IEnumerable<Menu>> Menus();
    Task<bool> IsExist(int? menuId);
    Task<IList<Menu>> GetIncludeModulePage(int? menuGroupId);
}