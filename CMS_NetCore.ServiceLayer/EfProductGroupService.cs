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
            Create(productGroup);
            await SaveAsync();
        }

        public async Task Remove(ProductGroup productGroup)
        {
            foreach (var item in await FindAll().OrderByDescending(o => o.Depth).ToListAsync())
            {
                foreach (var parent in (item.Path.Split('/')))
                {
                    if (parent == (productGroup.ProductGroupId.ToString()))
                    {
                        Delete(item);
                    }
                }
            }
            Delete(productGroup);
            await SaveAsync();
        }


        public async Task Edit(ProductGroup productGroup)
        {
            foreach (var item in await GetByDepth(0))
            {
                foreach (var parent in (item.Path.Split('/')))
                {
                    if (parent == (productGroup.ProductGroupId.ToString()))
                    {
                        var parent_item = await GetById(item.ParentId);
                        item.Depth = parent_item.Depth + 1;
                        item.Path = parent_item.ProductGroupId + "/" + parent_item.Path;
                        Update(item);
                    }
                }
            }

            Update(productGroup);
            await SaveAsync();
        }

        public async Task<ProductGroup> GetById(int? id) =>
            await FindByCondition(x => x.ProductGroupId.Equals(id)).DefaultIfEmpty(new ProductGroup()).SingleAsync();

        public async Task<IList<ProductGroup>> GetByDepth(int? depth) =>
            await FindByCondition(x => x.Depth > depth).OrderBy(s => s.Depth).ToListAsync();

        public async Task<IEnumerable<ProductGroup>> ProductGroups() =>
            await FindAll().ToListAsync();

        public async Task<IList<ProductGroup>> GetByType(string type) =>
           await FindByCondition(x => x.type == type).ToListAsync();

        public async Task<bool> UniqueAlias(string aliasName, int? productGroupId) =>
            await FindByCondition(s => s.AliasName == aliasName && s.ProductGroupId != productGroupId).AnyAsync();

    }
}
