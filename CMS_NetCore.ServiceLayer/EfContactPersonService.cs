using System.Collections.Generic;
using System.Threading.Tasks;
using CMS_NetCore.DataLayer;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.ServiceLayer;

public class EfContactPersonService : RepositoryBase<ContactPerson>, IContactPersonService
{
    public EfContactPersonService(AppDbContext context) : base(context)
    {
    }

    public async Task<bool> IsExist(
        int? moduleId,
        int? userId
    )
    {
        return await FindByCondition(
            contactPerson =>
                contactPerson.ContactModuleId == moduleId &&
                contactPerson.UserId == userId
        ).AnyAsync();
    }

    public async Task<ContactPerson> GetByModuleUser(
        int? moduleId,
        int? userId
    )
    {
        return await FindByCondition(
            contactPerson =>
                contactPerson.ContactModuleId == moduleId &&
                contactPerson.UserId == userId
        ).FirstOrDefaultAsync();
    }

    public async Task Add(IList<ContactPerson> contactPeople)
    {
        if (contactPeople.Count > 0)
        {
            foreach (var contactPerson in contactPeople)
                Create(contactPerson);

            await SaveAsync();
        }
    }

    public async Task Remove(IList<ContactPerson> contactPeople)
    {
        if (contactPeople.Count > 0)
        {
            foreach (var item in contactPeople)
                Delete(item);

            await SaveAsync();
        }
    }
}