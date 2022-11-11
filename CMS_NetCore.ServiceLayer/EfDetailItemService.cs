using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS_NetCore.DataLayer;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using CMS_NetCore.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.ServiceLayer;

public class EfDetailItemService : RepositoryBase<DetailItem>, IDetailItemService
{
    public EfDetailItemService(AppDbContext context) : base(context)
    {
    }

    public async Task<DataGridViewModel<DetailItem>> GetByDetailGroupId(int? detailGroupId)
    {
        var dataGridView = new DataGridViewModel<DetailItem>
        {
            Records = await FindByCondition(detailItem => detailItem.DetailGroupId == detailGroupId)
                .OrderBy(detailItem => detailItem.Id)
                .Include(detailItem => detailItem.DetailGroup)
                .ToListAsync(),
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
        await FindByCondition(detailItem => detailItem.Id.Equals(id)).FirstOrDefaultAsync();

    public async Task<IList<DetailItem>> GetByProductGroupId(Product product) =>
        await FindByCondition(
                detailItem =>
                    detailItem.DetailGroup.ProductGroupId == product.ProductGroupId
            )
            .ToListAsync();

    public async Task<bool> IsExist(int? id) =>
        await FindByCondition(detailItem => detailItem.Id == id).AnyAsync();
}