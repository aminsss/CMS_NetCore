using System.Collections.Generic;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.ViewModels;

namespace CMS_NetCore.Interfaces;

public interface IMenuGroupService
{
    Task<DataGridViewModel<MenuGroup>> GetBySearch(
        int? page,
        int? pageSize,
        string searchString
    );

    Task<MenuGroup> GetById(int? id);
    Task Add(MenuGroup menuGroup);
    Task Edit(MenuGroup menuGroup);
    Task Remove(MenuGroup menuGroup);
    Task<IEnumerable<MenuGroup>> MenuGroup();
    Task<bool> IsExist(int? menuGroupId);
}