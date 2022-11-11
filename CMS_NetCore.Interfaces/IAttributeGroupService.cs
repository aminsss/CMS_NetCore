using System.Collections.Generic;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.ViewModels;

namespace CMS_NetCore.Interfaces;

public interface IAttributeGroupService
{
    Task<DataGridViewModel<AttributeGroup>> GetBySearch(
        int page,
        int pageSize,
        string searchString
    );

    Task<IEnumerable<AttributeGroup>> GetAll();
    Task<IList<AttributeGroup>> GetByProductGroupId(int? productGroupId);
    Task<AttributeGroup> GetById(int? id);
    Task Add(AttributeGroup attributeGroup);
    Task Edit(AttributeGroup attributeGroup);
    Task Remove(AttributeGroup attributeGroup);
    Task<bool> IsExist(int? attributeGroupId);
}