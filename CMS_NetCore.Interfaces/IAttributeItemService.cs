using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using  CMS_NetCore.ViewModels;

namespace CMS_NetCore.Interfaces
{
    public interface IAttributeItemService
    {
        DataGridViewModel<AttributItem> GetByAttrGrpId(int? id);
        AttributItem GetById(int? id);
        void Add(AttributItem attributItem);
        void Edit(AttributItem attributItem);
        void Delete(AttributItem attributItem);
        void Delete(int? id);
    }
}
