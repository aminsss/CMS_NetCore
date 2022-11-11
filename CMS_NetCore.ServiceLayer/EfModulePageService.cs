using System.Collections.Generic;
using System.Threading.Tasks;
using CMS_NetCore.DataLayer;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.ServiceLayer;

public class EfModulePageService : RepositoryBase<ModulePage>, IModulePageService
{
    public EfModulePageService(AppDbContext context) : base(context)
    {
    }

    public async Task Add(IList<ModulePage> modulePages)
    {
        if (modulePages.Count > 0)
        {
            foreach (var modulePage in modulePages)
                Create(modulePage);

            await SaveAsync();
        }
    }

    public async Task Remove(IList<ModulePage> modulePages)
    {
        if (modulePages.Count > 0)
        {
            foreach (var modulePage in modulePages)
                Delete(modulePage);

            await SaveAsync();
        }
    }

    public async Task<bool> IsExist(
        int? moduleId,
        int? menuId
    ) =>
        await FindByCondition(
            modulePage =>
                modulePage.ModuleId == moduleId &&
                modulePage.MenuId == menuId
        ).AnyAsync();

    public async Task<ModulePage> GetByMenuModule(
        int? moduleId,
        int? menuId
    ) =>
        await FindByCondition(
            modulePage =>
                modulePage.ModuleId == moduleId &&
                modulePage.MenuId == menuId
        ).FirstOrDefaultAsync();

    public async Task<IList<ModulePage>> GetByMenuId(int menuId) =>
        await FindByCondition(modulePage => modulePage.MenuId == menuId)
            .Include(modulePage => modulePage.Module)
            .ThenInclude(module => module.Component)
            .Include(modulePage => modulePage.Module)
            .ThenInclude(module => module.Position)
            .ToListAsync();
}