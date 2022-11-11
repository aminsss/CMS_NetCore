using System.Threading.Tasks;
using CMS_NetCore.DataLayer;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.ServiceLayer;

public class EfMenuModuleService : RepositoryBase<MenuModule>, IMenuModuleService
{
    public EfMenuModuleService(AppDbContext context) : base(context)
    {
    }

    public async Task Edit(MenuModule menuModule)
    {
        Update(menuModule);
        await SaveAsync();
    }

    public async Task<MenuModule> GetByModuleId(int? id) =>
        await FindByCondition(menuModule => menuModule.Id == id).FirstOrDefaultAsync();
}