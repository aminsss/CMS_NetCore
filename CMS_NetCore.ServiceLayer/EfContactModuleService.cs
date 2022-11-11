using System.Threading.Tasks;
using CMS_NetCore.DataLayer;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.ServiceLayer;

public class EfContactModuleService : RepositoryBase<ContactModule>, IContactModuleService
{
    public EfContactModuleService(AppDbContext context) : base(context)
    {
    }

    public async Task Edit(ContactModule contactModule)
    {
        Update(contactModule);
        await SaveAsync();
    }

    public async Task<ContactModule> GetByModuleId(int? moduleId) =>
        await FindByCondition(x => x.Id == moduleId).FirstOrDefaultAsync();
}