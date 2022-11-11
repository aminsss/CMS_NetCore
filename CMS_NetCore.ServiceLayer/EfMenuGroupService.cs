using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS_NetCore.DataLayer;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using CMS_NetCore.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.ServiceLayer;

public class EfMenuGroupService : RepositoryBase<MenuGroup>, IMenuGroupService
{
    public EfMenuGroupService(AppDbContext context) : base(context)
    {
    }

    public async Task<DataGridViewModel<MenuGroup>> GetBySearch(
        int? page,
        int? pageSize,
        string searchString
    )
    {
        var dataGridView = new DataGridViewModel<MenuGroup>
        {
            Records = await FindByCondition(menuGroup => menuGroup.Title.Contains(searchString))
                .OrderBy(menuGroup => menuGroup.Id)
                .ToListAsync(),
        };

        return dataGridView;
    }

    public async Task Add(MenuGroup menuGroup)
    {
        Create(menuGroup);
        await SaveAsync();
    }

    public async Task Remove(MenuGroup menuGroup)
    {
        Delete(menuGroup);
        await SaveAsync();
    }


    public async Task Edit(MenuGroup menuGroup)
    {
        Update(menuGroup);
        await SaveAsync();
    }

    public async Task<MenuGroup> GetById(int? id) =>
        await FindByCondition(menuGroup => menuGroup.Id.Equals(id)).FirstOrDefaultAsync();

    public async Task<IEnumerable<MenuGroup>> MenuGroup() =>
        await FindAll().ToListAsync();

    public async Task<bool> IsExist(int? id) =>
        await FindByCondition(menuGroup => menuGroup.Id.Equals(id)).AnyAsync();
}