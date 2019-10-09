using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS_NetCore.DataLayer;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.ServiceLayer
{
    public class EfContactPersonService : RepositoryBase<ContactPerson>,IContactPersonService
    {

        public EfContactPersonService(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> ExistContactPerson(int? moduleId, int? userId) =>
            await FindByCondition(x => x.ContactModuleId == moduleId && x.UserId == userId).AnyAsync();

        public async Task<ContactPerson> GetByModuleUser(int? moduleId, int? userId) =>
            await FindByCondition(x => x.ContactModuleId == moduleId && x.UserId == userId).FirstOrDefaultAsync();

        public async Task Add(IList<ContactPerson> contactPeople)
        {
            if (contactPeople.Count > 0)
            {
                foreach (var item in contactPeople)
                {
                    Create(item);
                }
                await SaveAsync();
            }

        }

        public async Task Remove(IList<ContactPerson> contactPeople)
        {
            if (contactPeople.Count > 0)
            {
                foreach (var item in contactPeople)
                {
                    Delete(item);
                }
                await SaveAsync();
            }
        }

       
    }
}
