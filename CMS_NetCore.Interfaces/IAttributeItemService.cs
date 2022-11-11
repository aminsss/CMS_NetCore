using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.ViewModels;

namespace CMS_NetCore.Interfaces;

public interface IAttributeItemService
{
    Task<DataGridViewModel<AttributeItem>> GetByAttrGroupId(int? attributeGroupId);
    Task<AttributeItem> GetById(int? id);
    Task Add(AttributeItem attributeItem);
    Task Edit(AttributeItem attributeItem);
    Task Remove(AttributeItem attributeItem);
    Task<bool> IsExist(int? attributeItemId);
}