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
        DataGridViewModel<AttributGrp> GetBySearch(int page, int pageSize, string searchString);
        AttributGrp GetById(int? id);
        void Add(AttributGrp attributGrp);
        void Edit(AttributGrp attributGrp);
        void Delete(AttributGrp attributGrp);
        void Delete(int id);

        IEnumerable<AttributGrp> GetAllAttributeGrp();
        IList<AttributGrp> GetAttrGrpProductBase(int? productGroupId, int? groupModel);
    }
}
