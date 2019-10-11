using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.ViewModels;
using CMS_NetCore.Interfaces;
using CMS_NetCore.DataLayer;
using Microsoft.EntityFrameworkCore;


namespace CMS_NetCore.ServiceLayer
{
    public class EfModuleService : RepositoryBase<Module>,IModuleService
    {
        private IMenuService _menuService;

        public EfModuleService(AppDbContext context,IMenuService menuService) : base(context)
        {
            _menuService = menuService;
        }

        public async Task<DataGridViewModel<Module>> GetBySearch(string searchString)
        {
            var dataGridView = new DataGridViewModel<Module>
            {
                Records = await FindByCondition(x => x.ModuleTitle.Contains(searchString))
                .OrderBy(o => o.DisplayOrder).OrderBy(O => O.PositionId).Include(x => x.Component).Include(x => x.Position)
                .ToListAsync()
            };

            return dataGridView;
        }

        public async Task Add(Module module)
        {
            module.AddedDate = DateTime.Now;
            module.ModifiedDate = DateTime.Now;

            //method for asign order in create
            var last = await GetLastByPosition(module.PositionId);
            if (last == null)
                module.DisplayOrder = 1;
            else
                module.DisplayOrder = (int)last.DisplayOrder + 1;

            Create(module);
            await SaveAsync();
        }

        
        public async Task Edit(Module module, int? pastPosition, int? pastDisOrder)
        {
            module.ModifiedDate = DateTime.Now;

            // ordering => if new order is lower than this module
            if (pastPosition == module.PositionId && module.DisplayOrder < pastDisOrder)
            {
                foreach (var item in await FindByCondition(x => x.PositionId == module.PositionId && x.DisplayOrder >= module.DisplayOrder && x.DisplayOrder < pastDisOrder).OrderBy(o => o.DisplayOrder).ToListAsync())
                {
                    item.DisplayOrder += 1;
                    Update(item);
                }
            }
            //if new order is higher than this module
            else if (pastPosition == module.PositionId && module.DisplayOrder > pastDisOrder)
            {
                foreach (var item in await FindByCondition(x => x.PositionId == module.PositionId && x.DisplayOrder <= module.DisplayOrder && x.DisplayOrder > pastDisOrder).OrderBy(o => o.DisplayOrder).ToListAsync())
                {
                    item.DisplayOrder -= 1;
                    Update(item);
                }
            }
            //if menu choose another position
            else if (pastPosition != module.PositionId)
            {
                //ordering displalay order of past position 
                foreach (var item in await FindByCondition(x => x.PositionId == pastPosition && x.DisplayOrder > pastDisOrder).OrderBy(o => o.DisplayOrder).ToListAsync())
                {
                    item.DisplayOrder -= 1;
                    Update(item);
                }

                //making the last child of new position 
                var last = await GetLastByPosition(module.PositionId);
                if (last == null)
                    module.DisplayOrder = 1;
                else
                    module.DisplayOrder = (int)last.DisplayOrder + 1;
            }

            Update(module);
            await SaveAsync();
        }

        public async Task Remove(Module module)
        {
            //editing order of modules with bigger displayOrder in current Position
            foreach (var item in await FindByCondition(x => x.PositionId == module.PositionId && x.DisplayOrder > module.DisplayOrder).OrderBy(o => o.DisplayOrder).ToListAsync())
            {
                item.DisplayOrder -= 1;
                Update(item);
            }
            Delete(module);
             await SaveAsync();
        }

        public async Task<Module> GetLastByPosition(int? positionId) =>
            await FindByCondition(x => x.PositionId == positionId)
                .OrderByDescending(o=>o.DisplayOrder).FirstOrDefaultAsync();

        public async Task<IList<Module>> GetByPositionId(int? id) =>
            await FindByCondition(x => x.PositionId == id).ToListAsync();

        public async Task<Module> GetById(int? id) =>
           await FindByCondition(x => x.ModuleId.Equals(id)).DefaultIfEmpty(new Module()).SingleAsync();

        public async Task<bool> ExistModule(int? id) =>
            await FindByCondition(x => x.ModuleId.Equals(id)).AnyAsync();

        public async Task<Module> GetMenuModuleById(int? id) =>
            await FindByCondition(x => x.ModuleId.Equals(id)).Include(x => x.MenuModule).DefaultIfEmpty(new Module()).FirstOrDefaultAsync();
    }
}
