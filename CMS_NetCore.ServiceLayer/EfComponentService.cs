using System.Collections.Generic;
using System.Threading.Tasks;
using CMS_NetCore.DataLayer;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.ServiceLayer;

public class EfComponentService : RepositoryBase<Component>, IComponentService
{
    public EfComponentService(AppDbContext context) : base(context)
    {
    }

    public async Task<IList<Component>> GetAll() =>
        await FindAll().ToListAsync();
}