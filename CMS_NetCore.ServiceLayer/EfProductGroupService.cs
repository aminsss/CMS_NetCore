using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS_NetCore.DataLayer;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using CMS_NetCore.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.ServiceLayer
{
    public class EfProductGroupService : RepositoryBase<ProductGroup>, IProductGroupService
    {

        public EfProductGroupService(AppDbContext context) : base(context)
        {
        }

        public async Task<DataGridViewModel<ProductGroup>> GetBySearch(int? page, int? pageSize, string searchString)
        {
            var dataGridView = new DataGridViewModel<ProductGroup>
            {
                Records = await FindByCondition(s => s.GroupTitle.Contains(searchString) || s.AliasName.Contains(searchString))
                .ToListAsync(),
            };

            return dataGridView;
        }

        public async Task Add(ProductGroup productGroup)
        {
            if (productGroup.ParentId == 0)
            {
                productGroup.Depth = 0;
                productGroup.Path = "0";
            }
            else
            {
                var GroupParent = await GetById(productGroup.ParentId);
                productGroup.Depth = GroupParent.Depth + 1;
                //define hierarchy of Group  
                productGroup.Path = GroupParent.ProductGroupId + "/" + GroupParent.Path;
            }

            Create(productGroup);
            await SaveAsync();
        }

        public async Task Remove(ProductGroup productGroup)
        {
            //remove all children belong to this Group
            await ChildRemove(productGroup);
            //remove current Group
            Delete(productGroup);
            await SaveAsync();
        }

        public async Task ChildRemove(ProductGroup productGroup)
        {
            foreach (ProductGroup child in await FindByCondition(x => x.ParentId == productGroup.ProductGroupId).ToListAsync())
            {
                Delete(child);
                await ChildRemove(child);
            }
        }

        public async Task Edit(ProductGroup productGroup)
        {
            //update curent Group
            Update(productGroup);
            //updating children's depth and path of changed group 
            await ChildEdit(productGroup);
            await SaveAsync();
        }

        public async Task ChildEdit(ProductGroup productGroup)
        {
            foreach (ProductGroup child in await FindByCondition(x => x.ParentId == productGroup.ProductGroupId).ToListAsync())
            {
                child.Path = productGroup.ProductGroupId + "/" + productGroup.Path;
                child.Depth = productGroup.Depth + 1;
                Update(child);

                await ChildEdit(child);
            }
        }

        public async Task<ProductGroup> GetById(int? id) =>
            await FindByCondition(x => x.ProductGroupId.Equals(id)).FirstOrDefaultAsync();

        public async Task<IEnumerable<ProductGroup>> ProductGroups() =>
            await FindAll().ToListAsync();

        public async Task<IList<ProductGroup>> GetByType(string type) =>
           await FindByCondition(x => x.type == type).ToListAsync();

        public async Task<bool> UniqueAlias(string aliasName, int? productGroupId) =>
            await FindByCondition(s => s.AliasName == aliasName && s.ProductGroupId != productGroupId).AnyAsync();

        public async Task<bool> ProductGroupExistense(int id) =>
            await FindByCondition(x => x.ProductGroupId == id).AnyAsync();

    }
}
