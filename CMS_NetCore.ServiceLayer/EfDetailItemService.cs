using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using CMS_NetCore.ViewModels;
using CMS_NetCore.DataLayer;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.ServiceLayer
{
    public class EfDetailItemService : RepositoryBase<DetailItem>,IDetailItemService
    {
        public EfDetailItemService(AppDbContext context) : base(context)
        {
        }

        public async Task<DataGridViewModel<DetailItem>> GetByDetailGroupId(int? detailGroupId)
        {
            var dataGridView = new DataGridViewModel<DetailItem>
            {
                Records = await FindByCondition(s => s.DetailGroupId == detailGroupId)
                .OrderBy(x => x.DetailItemId).Include(x=>x.DetailGroup).ToListAsync(),
            };

            return dataGridView;

        }

        public async Task Add(DetailItem detailItem)
        {
            Create(detailItem);
            await SaveAsync();
        }

        public async Task Remove(DetailItem detailItem)
        {
            Delete(detailItem);
            await SaveAsync();
        }

        public async Task Edit(DetailItem detailItem)
        {
            Update(detailItem);
            await SaveAsync();
        }

        public async Task<DetailItem> GetById(int? id) =>
            await FindByCondition(x => x.DetailItemId.Equals(id)).DefaultIfEmpty(new DetailItem()).SingleAsync();

        public async Task<IList<DetailItem>> GetByProduct(Product product) =>
            await FindByCondition(x => x.DetailGroup.ProductGroupId == product.ProductGroupId).ToListAsync();

        public async Task<bool> DetailItemExistence(int? id) =>
            await FindByCondition(x => x.DetailItemId == id).AnyAsync();
    }
}
