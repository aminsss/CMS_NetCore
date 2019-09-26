using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS_NetCore.Interfaces;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.ViewModels;
using CMS_NetCore.DataLayer;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.ServiceLayer
{
    public class EfAttributeItemService : RepositoryBase<AttributItem>, IAttributeItemService
    {

        public EfAttributeItemService(AppDbContext context) : base(context)
        {
        }

        public async Task<DataGridViewModel<AttributItem>> GetByAttrGrpId(int? attributGrpId)
        {
            var DataGridView = new DataGridViewModel<AttributItem>
            {
                Records = await FindByCondition(x => x.AttributGrpId == attributGrpId).OrderBy(x => x.AttributItemId)
                .Include(x=>x.AttributGrp).Include(x=>x.AttributGrp.ProductGroup).ToListAsync(),
            };

            return DataGridView;
        }

        public async Task<AttributItem> GetById(int? id) =>
             await FindByCondition(x=>x.AttributItemId.Equals(id)).DefaultIfEmpty(new AttributItem())
            .Include(x=>x.AttributGrp).SingleAsync();

        public async Task Add(AttributItem attributItem)
        {
            Create(attributItem);
            await SaveAsync();
        }

        public async Task Remove(AttributItem attributItem)
        {
            Delete(attributItem);
            await SaveAsync();
        }

        public async Task Edit(AttributItem attributItem)
        {
            Update(attributItem);
            await SaveAsync();
        }

        public async Task<bool> AttributeItemsExistence(int? id) =>
             await FindByCondition(x => x.AttributItemId.Equals(id)).AnyAsync();
        
    }
}
