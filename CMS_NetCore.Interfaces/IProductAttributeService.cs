using System.Collections.Generic;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;

namespace CMS_NetCore.Interfaces;

public interface IProductAttributeService
{
    Task<ProductAttribute> GetProductAttribute(
        int? productId,
        int? attributeGroupId
    );

    void Remove(ProductAttribute productAttribute);
    void Edit(ProductAttribute productAttribute);
    void Add(IList<ProductAttribute> productAttributes);
}