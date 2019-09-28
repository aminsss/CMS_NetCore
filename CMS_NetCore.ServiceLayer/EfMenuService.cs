using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS_NetCore.ViewModels;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.DataLayer;
using CMS_NetCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.ServiceLayer
{
    public class EfMenuService : RepositoryBase<Menu>, IMenuService
    {
        public EfMenuService(AppDbContext context) : base(context)
        {
        }

        public async Task<DataGridViewModel<Menu>> GetByMenuGroup(int? menuGroupId)
        {
            var dataGridView = new DataGridViewModel<Menu>
            {
                Records = await FindByCondition(x => x.MenuGroupId == menuGroupId)
                .OrderBy(x => x.MenuId).Include(x => x.MenuGroup).ToListAsync()
            };

            return dataGridView;
        }

        public async Task<Menu> GetById(int? id) =>
            await FindByCondition(x => x.MenuId.Equals(id)).DefaultIfEmpty(new Menu()).SingleAsync();

        public async Task Add(Menu menu)
        {
            if (menu.ParentId == 0)
            {
                menu.Depth = 0;
                menu.Path = "0";
            }
            else
            {
                var Menus = await GetById(menu.ParentId);
                menu.Depth = Menus.Depth + 1;
                menu.Path = Menus.MenuId + "/" + Menus.Path;
            }
            var last = await GetLastOrder(menu.ParentId, menu.MenuGroupId);
            if (last == null)
            {
                menu.DisplayOrder = 1;
            }
            else
            {
                menu.DisplayOrder = Convert.ToInt32(last.DisplayOrder) + 1;
            }
            Create(menu);
            await SaveAsync();
        }


        public async Task Edit(Menu menu, int? pastDisOrder, int? pastParentId, int? pastGroupId)
        {
            //////if new parent is the same as past parent and past group
            if (pastParentId == menu.ParentId && pastGroupId == menu.MenuGroupId)
            {
                //if new Menu order is lower than this menu order
                if (menu.DisplayOrder < pastDisOrder)
                {
                    //Get list of menu between lower of past display odrer and upper of new display order
                    foreach (var item in await FindByCondition(x => x.ParentId == menu.ParentId && x.MenuGroupId == menu.MenuGroupId &&
                                          x.DisplayOrder >= menu.DisplayOrder && x.DisplayOrder < pastDisOrder).OrderBy(o => o.DisplayOrder).ToListAsync())
                    {
                        item.DisplayOrder += 1;
                        Update(item);
                    }
                }
                //if new parent order is upper than this menu order
                else if (menu.DisplayOrder > pastDisOrder)
                {
                    //Get list of menu between upper of past display odrer and lower of new display order
                    foreach (var item in await FindByCondition(x => x.ParentId == menu.ParentId && x.MenuGroupId == menu.MenuGroupId &&
                                         x.DisplayOrder <= menu.DisplayOrder && x.DisplayOrder > pastDisOrder).OrderBy(o => o.DisplayOrder).ToListAsync())
                    {
                        item.DisplayOrder -= 1;
                        Update(item);
                    }
                }
            }
            //if menu choose another group menu
            else
            {
                //ordering displalay order of past parent 
                foreach (var item in await GetByParentGroupOrder(pastParentId, pastGroupId, pastDisOrder))
                {
                    item.DisplayOrder -= 1;
                    Update(item);
                }

                //ordering the last child of new parent 
                var last = await GetLastOrder(menu.ParentId, menu.MenuGroupId);
                if (last == null)
                {
                    menu.DisplayOrder = 1;
                }
                else
                {
                    menu.DisplayOrder = Convert.ToInt32(last.DisplayOrder) + 1;
                }
            }
            Update(menu);
            //Update the children of changed Menu of this Menu 
            await ChildEdit(menu);
            await SaveAsync();
        }

        public async Task ChildEdit(Menu menu)
        {
            foreach (Menu child in await FindByCondition(x => x.ParentId == menu.MenuId).ToListAsync())
            {
                child.Path = menu.MenuId + "/" + menu.Path;
                child.Depth = menu.Depth + 1;
                child.MenuGroupId = menu.MenuGroupId;

                Update(child);

                await ChildEdit(child);
            }
        }

        public async Task Remove(Menu menu)
        {
            //editing Order Of other menu after deleting this menu in the same parent 
            foreach (var item in await FindByCondition(x => x.ParentId == menu.ParentId && x.MenuGroupId == menu.MenuGroupId && x.DisplayOrder > menu.DisplayOrder).OrderBy(x => x.DisplayOrder).ToListAsync())
            {
                item.DisplayOrder -= 1;
                Update(item);
            }

            //Delete the children of Deleted Menu of this Menu 
            await ChildRemove(menu);

            Delete(menu);
            await SaveAsync();
        }
        public async Task ChildRemove(Menu menu)
        {
            foreach (Menu child in await FindByCondition(x => x.ParentId == menu.MenuId).ToListAsync())
            {
                Delete(child);
                await ChildRemove(child);
            }
        }
        public async Task<Menu> GetLastOrder(int? parentId, int? menuGroupId) =>
            await FindByCondition(x => x.ParentId == parentId && x.MenuGroupId == menuGroupId)
            .OrderByDescending(o => o.DisplayOrder).FirstOrDefaultAsync();

        public async Task<IList<Menu>> GetByParentGroupOrder(int? parentId, int? menuGroupId, int? pastDisOrder) =>
            await FindByCondition(x => x.ParentId == parentId && x.MenuGroupId == menuGroupId
            && x.DisplayOrder > pastDisOrder).OrderBy(o => o.DisplayOrder).ToListAsync();

        public async Task<bool> UniquePageName(string pageName, int? menuId) =>
            await FindByCondition(s => s.PageName == pageName && s.MenuId != menuId).AnyAsync();

        public async Task<IList<Menu>> GetByParentId(int? parentId,int? menuGroupId) =>
            await FindByCondition(x => x.ParentId == parentId && x.MenuGroupId == menuGroupId).ToListAsync();

        public async Task<IEnumerable<Menu>> Menus() =>
            await FindAll().ToListAsync();

        public async Task<bool> MenuExistence(int? id) =>
            await FindByCondition(x => x.MenuId.Equals(id)).AnyAsync();
    }
}
