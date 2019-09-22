using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using  CMS_NetCore.ViewModels;

namespace CMS_NetCore.Interfaces
{
    public interface IAttributeGrpService
    {
        Task<DataGridViewModel<AttributGrp>> GetBySearch(int page, int pageSize, string searchString);
        Task<AttributGrp> GetById(int? id);
        Task Add(AttributGrp attributGrp);
        Task Edit(AttributGrp attributGrp);
        Task Remove(AttributGrp attributGrp);
        Task<IEnumerable<AttributGrp>> GetAll();
        Task<bool> AttributeGrpExistence(int? id);
    }
}
