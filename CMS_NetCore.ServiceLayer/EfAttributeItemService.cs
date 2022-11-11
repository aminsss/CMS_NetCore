using System.Linq;
using System.Threading.Tasks;
using CMS_NetCore.DataLayer;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using CMS_NetCore.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.ServiceLayer;

public class EfAttributeItemService : RepositoryBase<AttributeItem>, IAttributeItemService
{
    public EfAttributeItemService(AppDbContext context) : base(context)
    {
    }

    public async Task<DataGridViewModel<AttributeItem>> GetByAttrGroupId(int? attributeGroupId)
    {
        return new DataGridViewModel<AttributeItem>
        {
            Records = await FindByCondition(x => x.AttributeGroupId == attributeGroupId)
                .OrderBy(attributeItem => attributeItem.Id)
                .Include(attributeItem => attributeItem.AttributeGroup)
                .ThenInclude(attributeGroup => attributeGroup.ProductGroup)
                .ToListAsync(),
        };
    }

    public async Task<AttributeItem> GetById(int? id) =>
        await FindByCondition(attributeItem => attributeItem.Id.Equals(id))
            .Include(attributeItem => attributeItem.AttributeGroup)
            .FirstOrDefaultAsync();

    public async Task Add(AttributeItem attributeItem)
    {
        Create(attributeItem);
        await SaveAsync();
    }

    public async Task Remove(AttributeItem attributeItem)
    {
        Delete(attributeItem);
        await SaveAsync();
    }

    public async Task Edit(AttributeItem attributeItem)
    {
        Update(attributeItem);
        await SaveAsync();
    }

    public async Task<bool> IsExist(int? id) =>
        await FindByCondition(attributeItem => attributeItem.Id.Equals(id)).AnyAsync();
}