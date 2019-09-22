using CMS_NetCore.DataLayer;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.ServiceLayer
{
    public class EfRoleService : RepositoryBase<Role> ,IRoleService
    {
        public EfRoleService(AppDbContext contex)
       : base(contex)
        {
        }

        public async Task<IEnumerable<Role>> Roles() =>
              await FindAll().ToListAsync();
    }
}
