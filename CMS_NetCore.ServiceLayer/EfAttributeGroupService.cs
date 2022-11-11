using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS_NetCore.DataLayer;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using CMS_NetCore.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.ServiceLayer;

public class EfAttributeGroupService : RepositoryBase<AttributeGroup>, IAttributeGroupService
{
    public EfAttributeGroupService(AppDbContext context)
        : base(context)
    {
    }

    public async Task Add(AttributeGroup attributeGroup)
    {
        Create(attributeGroup);
        await SaveAsync();
    }

    public async Task Remove(AttributeGroup attributeGroup)
    {
        Delete(attributeGroup);
        await SaveAsync();
    }

    public async Task Edit(AttributeGroup attributeGroup)
    {
        Update(attributeGroup);
        await SaveAsync();
    }

    public async Task<IEnumerable<AttributeGroup>> GetAll() =>
        await FindAll().ToListAsync();

    public async Task<DataGridViewModel<AttributeGroup>> GetBySearch(
        int page,
        int pageSize,
        string searchString
    )
    {
        var dataGridView = new DataGridViewModel<AttributeGroup>
        {
            Records = await FindByCondition(attributeGroup => attributeGroup.Name.Contains(searchString))
                .Include(x => x.ProductGroup)
                .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(),

            TotalCount = await FindByCondition(attributeGroup => attributeGroup.Name.Contains(searchString))
                .Include(x => x.ProductGroup)
                .Skip((page - 1) * pageSize).Take(pageSize).CountAsync(),
        };

        return dataGridView;
    }

    public async Task<AttributeGroup> GetById(int? id) =>
        await FindByCondition(attributeGroup => attributeGroup.Id.Equals(id)).Include(x => x.ProductGroup)
            .DefaultIfEmpty(new AttributeGroup()).SingleAsync();

    public async Task<bool> IsExist(int? id) =>
        await FindByCondition(attributeGroup => attributeGroup.Id.Equals(id)).AnyAsync();

    public async Task<IList<AttributeGroup>> GetByProductGroupId(int? productGroupId) =>
        await FindByCondition(attributeGroup => attributeGroup.ProductGroupId == productGroupId)
            .Include(attributeGroup => attributeGroup.AttributeItems)
            .ThenInclude(attributeItem => attributeItem.ProductAttributes).ToListAsync();
}