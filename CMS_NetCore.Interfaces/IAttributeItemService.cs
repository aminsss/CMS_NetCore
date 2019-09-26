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
        Task<DataGridViewModel<AttributItem>> GetByAttrGrpId(int? id);
        Task<AttributItem> GetById(int? id);
        Task Add(AttributItem attributItem);
        Task Edit(AttributItem attributItem);
        Task Remove(AttributItem attributItem);
        Task<bool> AttributeItemsExistence(int? id);
    }
}
