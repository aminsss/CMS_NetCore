using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;

namespace CMS_NetCore.Interfaces
{
    public interface IModulePageService
    {
        Task<ModulePage> GetByMenuModule(int? moduleId, int? menuId);
        Task Add(IList<ModulePage> modulePage);
        Task Remove(IList<ModulePage> modulePage);
        Task<bool> ExistModulePage(int? moduleId, int? menuId);
    }
}
