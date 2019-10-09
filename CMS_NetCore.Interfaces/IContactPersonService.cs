using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;

namespace CMS_NetCore.Interfaces
{
    public interface IContactPersonService
    {
        Task<bool> ExistContactPerson(int? moduleId, int? userId);
        Task<ContactPerson> GetByModuleUser(int? moduleId, int? userId);
        Task Add(IList<ContactPerson> contactPeople);
        Task Remove(IList<ContactPerson> contactPeople);
    }
}
