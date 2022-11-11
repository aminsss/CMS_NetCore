using System.Collections.Generic;
using System.Threading.Tasks;
using CMS_NetCore.DataLayer;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using CMS_NetCore.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.ServiceLayer;

public class EfProductGroupService : RepositoryBase<ProductGroup>, IProductGroupService
{
    public EfProductGroupService(AppDbContext context) : base(context)
    {
    }

    public async Task<DataGridViewModel<ProductGroup>> GetBySearch(
        int? page,
        int? pageSize,
        string searchString
    )
    {
        return new DataGridViewModel<ProductGroup>
        {
            Records = await FindByCondition(
                    productGroup =>
                        productGroup.GroupTitle.Contains(searchString) ||
                        productGroup.AliasName.Contains(searchString)
                )
                .ToListAsync(),
        };
    }

    public async Task Add(ProductGroup productGroup)
    {
        if (productGroup.ParentId != 0)
        {
            var groupParent = await GetById(productGroup.ParentId);
            productGroup.Depth = groupParent.Depth + 1;
            //define hierarchy of Group  
            productGroup.Path = groupParent.Id + "/" + groupParent.Path;
        }
        else
        {
            productGroup.Depth = 0;
            productGroup.Path = "0";
        }

        Create(productGroup);
        await SaveAsync();
    }

    public async Task Remove(ProductGroup productGroup)
    {
        await ChildRemove(productGroup);
        Delete(productGroup);
        await SaveAsync();
    }

    private async Task ChildRemove(ProductGroup productGroup)
    {
        foreach (var productGroupEntity in await FindByCondition(
                     productGroupEntity =>
                         productGroupEntity.ParentId == productGroup.Id
                 ).ToListAsync())
        {
            Delete(productGroupEntity);
            await ChildRemove(productGroupEntity);
        }
    }

    public async Task Edit(ProductGroup productGroup)
    {
        Update(productGroup);
        await ChildEdit(productGroup);
        await SaveAsync();
    }

    private async Task ChildEdit(ProductGroup productGroup)
    {
        foreach (var productGroupEntity in await FindByCondition(
                     productGroupEntity =>
                         productGroupEntity.ParentId == productGroup.Id
                 ).ToListAsync())
        {
            productGroupEntity.Path = productGroup.Id + "/" + productGroup.Path;
            productGroupEntity.Depth = productGroup.Depth + 1;
            Update(productGroupEntity);

            await ChildEdit(productGroupEntity);
        }
    }

    public async Task<ProductGroup> GetById(int? id) =>
        await FindByCondition(productGroup => productGroup.Id.Equals(id))
            .FirstOrDefaultAsync();

    public async Task<IEnumerable<ProductGroup>> ProductGroups() =>
        await FindAll().ToListAsync();

    public async Task<IList<ProductGroup>> GetByType(string type) =>
        await FindByCondition(productGroup => productGroup.Type == type).ToListAsync();

    public async Task<bool> UniqueAlias(
        string aliasName,
        int? productGroupId
    ) =>
        await FindByCondition(
                productGroup =>
                    productGroup.AliasName == aliasName &&
                    productGroup.Id != productGroupId
            )
            .AnyAsync();

    public async Task<bool> IsExist(int id) =>
        await FindByCondition(productGroup => productGroup.Id == id)
            .AnyAsync();
}