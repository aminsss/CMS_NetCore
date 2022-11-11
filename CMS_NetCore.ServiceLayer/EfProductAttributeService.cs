using System.Collections.Generic;
using System.Threading.Tasks;
using CMS_NetCore.DataLayer;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.ServiceLayer;

public class EfProductAttributeService : RepositoryBase<ProductAttribute>, IProductAttributeService
{
    public EfProductAttributeService(AppDbContext context) : base(context)
    {
    }

    public async Task<ProductAttribute> GetProductAttribute(
        int? productId,
        int? attributeGroupId
    ) =>
        await FindByCondition(
                productAttribute =>
                    productAttribute.ProductId == productId &&
                    productAttribute.AttributeItem.AttributeGroupId == attributeGroupId
            )
            .FirstOrDefaultAsync();

    public void Remove(ProductAttribute productAttribute) =>
        Delete(productAttribute);

    public void Edit(ProductAttribute productAttribute) =>
        Update(productAttribute);

    public void Add(IList<ProductAttribute> productAttributes)
    {
        foreach (var productAttribute in productAttributes)
            Create(productAttribute);
    }
}