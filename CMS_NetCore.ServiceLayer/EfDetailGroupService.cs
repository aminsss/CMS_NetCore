using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS_NetCore.DataLayer;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using CMS_NetCore.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.ServiceLayer;

public class EfDetailGroupService : RepositoryBase<DetailGroup>, IDetailGroupService
{
    public EfDetailGroupService(AppDbContext context) : base(context)
    {
    }

    public async Task<DataGridViewModel<DetailGroup>> GetBySearch(
        int page,
        int pageSize,
        string searchString
    )
    {
        return new DataGridViewModel<DetailGroup>
        {
            Records = await FindByCondition(detailGroup => detailGroup.Name.Contains(searchString)).OrderBy(x => x.Id)
                .Include(detailGroup => detailGroup.ProductGroup)
                .Take(pageSize)
                .Skip((page - 1) * pageSize)
                .ToListAsync(),
        };
    }


    public async Task Add(DetailGroup detailGroup)
    {
        Create(detailGroup);
        await SaveAsync();
    }

    public async Task Remove(DetailGroup detailGroup)
    {
        Delete(detailGroup);
        await SaveAsync();
    }

    public async Task Edit(DetailGroup detailGroup)
    {
        Update(detailGroup);
        await SaveAsync();
    }

    public async Task<DetailGroup> GetById(int? id) =>
        await FindByCondition(detailGroup => detailGroup.Id.Equals(id)).FirstOrDefaultAsync();

    public async Task<IList<DetailGroup>> GetByProductGroupId(int? productGroupId) =>
        await FindByCondition(detailGroup => detailGroup.ProductGroupId.Equals(productGroupId))
            .Include(detailGroup => detailGroup.DetailItems)
            .ThenInclude(detailItem => detailItem.ProductDetails).ToListAsync();

    public async Task<IEnumerable<DetailGroup>> GetAll() =>
        await FindAll().ToListAsync();

    public async Task<bool> IsExist(int? id) =>
        await FindByCondition(detailGroup => detailGroup.Id == id).AnyAsync();
}