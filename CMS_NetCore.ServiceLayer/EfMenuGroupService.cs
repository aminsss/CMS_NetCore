using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS_NetCore.DataLayer;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.ViewModels;
using CMS_NetCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.ServiceLayer
{
    public class EfMenuGroupService : RepositoryBase<MenuGroup>,IMenuGroupService
    {
        public EfMenuGroupService(AppDbContext context ) : base(context)
        {
        }

        public async Task<DataGridViewModel<MenuGroup>> GetBySearch(int? page, int? pageSize, string searchString)
        {
            var dataGridView = new DataGridViewModel<MenuGroup>
            {
                Records = await  FindByCondition(x => x.MenuTitile.Contains(searchString))
                .OrderBy(x => x.MenuGroupId).ToListAsync(),
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
            await FindByCondition(x=>x.MenuGroupId.Equals(id)).DefaultIfEmpty(new MenuGroup()).SingleAsync();

        public async Task<IEnumerable<MenuGroup>> MenuGroup() =>
            await FindAll().ToListAsync();

        public async Task<bool> MenuGroupExistence(int? id) =>
            await FindByCondition(x => x.MenuGroupId.Equals(id)).AnyAsync();
    }
}
