using System.Collections.Generic;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;

namespace CMS_NetCore.Interfaces;

public interface IModulePageService
{
    Task<ModulePage> GetByMenuModule(
        int? moduleId,
        int? menuId
    );

    Task Add(IList<ModulePage> modulePage);
    Task Remove(IList<ModulePage> modulePage);

    Task<bool> IsExist(
        int? moduleId,
        int? menuId
    );

    Task<IList<ModulePage>> GetByMenuId(int menuId);
}