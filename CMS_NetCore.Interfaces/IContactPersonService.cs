using System.Collections.Generic;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;

namespace CMS_NetCore.Interfaces;

public interface IContactPersonService
{
    Task<ContactPerson> GetByModuleUser(
        int? moduleId,
        int? userId
    );

    Task<bool> IsExist(
        int? moduleId,
        int? userId
    );

    Task Add(IList<ContactPerson> contactPeople);
    Task Remove(IList<ContactPerson> contactPeople);
}