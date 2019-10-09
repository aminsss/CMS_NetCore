using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using CMS_NetCore.DataLayer;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.ServiceLayer
{
    public class EfModulePageService : RepositoryBase<ModulePage>,IModulePageService
    {

        public EfModulePageService(AppDbContext context) : base(context)
        {
        }


        public async Task Add(IList<ModulePage> modulePages)
        {
            if (modulePages.Count > 0)
            {
                foreach (var item in modulePages)
                {
                    Create(item);
                }
                await SaveAsync();
            }
        }

        public async Task Remove(IList<ModulePage> modulePages)
        {
            if (modulePages.Count > 0)
            {
                foreach (var item in modulePages)
                {
                    Delete(item);
                }
               await SaveAsync();
            }
        }

        public async Task<bool> ExistModulePage(int? moduleId, int? menuId) =>
           await FindByCondition(s => s.ModuleId == moduleId && s.MenuId == menuId).AnyAsync();

        public async Task<ModulePage> GetByMenuModule(int? moduleId, int? menuId) =>
            await FindByCondition(s => s.ModuleId == moduleId && s.MenuId == menuId).FirstOrDefaultAsync();
    }
}
