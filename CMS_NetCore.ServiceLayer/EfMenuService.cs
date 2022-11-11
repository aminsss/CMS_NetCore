using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS_NetCore.DataLayer;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using CMS_NetCore.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.ServiceLayer;

public class EfMenuService : RepositoryBase<Menu>, IMenuService
{
    public EfMenuService(AppDbContext context) : base(context)
    {
    }

    public async Task<DataGridViewModel<Menu>> GetByMenuGroup(int? menuGroupId)
    {
        return new DataGridViewModel<Menu>
        {
            Records = await FindByCondition(menu => menu.MenuGroupId == menuGroupId)
                .OrderBy(menu => menu.Id)
                .Include(menu => menu.MenuGroup)
                .ToListAsync()
        };
    }

    public async Task<Menu> GetById(int? id) =>
        await FindByCondition(menu => menu.Id.Equals(id)).FirstOrDefaultAsync();

    public async Task Add(Menu menu)
    {
        if (menu.ParentId != 0)
        {
            var menus = await GetById(menu.ParentId);
            menu.Depth = menus.Depth + 1;
            menu.Path = menus.Id + "/" + menus.Path;
        }
        else
        {
            menu.Depth = 0;
            menu.Path = "0";
        }

        var last = await GetLastOrder(
            menu.ParentId,
            menu.MenuGroupId
        );

        if (last == null)
            menu.DisplayOrder = 1;
        else
            menu.DisplayOrder = last.DisplayOrder + 1;

        Create(menu);
        await SaveAsync();
    }


    public async Task Edit(
        Menu menu,
        int? pastDisplayOrder,
        int? pastParentId,
        int? pastGroupId
    )
    {
        //////if new parent is the same as past parent and past group
        if (pastParentId == menu.ParentId && pastGroupId == menu.MenuGroupId)
        {
            //if new Menu order is lower than this menu order
            if (menu.DisplayOrder < pastDisplayOrder)
            {
                //Get list of menu between lower of past display order and upper of new display order
                foreach (var menuEntity in await FindByCondition(
                             menuCondition => menuCondition.ParentId == menu.ParentId &&
                                              menuCondition.MenuGroupId == menu.MenuGroupId &&
                                              menuCondition.DisplayOrder >= menu.DisplayOrder &&
                                              menuCondition.DisplayOrder < pastDisplayOrder
                         ).OrderBy(o => o.DisplayOrder).ToListAsync())
                {
                    menuEntity.DisplayOrder += 1;
                    Update(menuEntity);
                }
            }
            //if new parent order is upper than this menu order
            else if (menu.DisplayOrder > pastDisplayOrder)
            {
                //Get list of menu between upper of past display order and lower of new display order
                foreach (var menuEntity in await FindByCondition(
                             menuCondition => menuCondition.ParentId == menu.ParentId &&
                                              menuCondition.MenuGroupId == menu.MenuGroupId &&
                                              menuCondition.DisplayOrder <= menu.DisplayOrder &&
                                              menuCondition.DisplayOrder > pastDisplayOrder
                         ).OrderBy(o => o.DisplayOrder).ToListAsync())
                {
                    menuEntity.DisplayOrder -= 1;
                    Update(menuEntity);
                }
            }
        }
        //if menu choose another group menu
        else
        {
            //ordering display order of past parent 
            foreach (var item in await GetByParentGroupOrder(
                         pastParentId,
                         pastGroupId,
                         pastDisplayOrder
                     ))
            {
                item.DisplayOrder -= 1;
                Update(item);
            }

            //ordering the last child of new parent 
            var last = await GetLastOrder(
                menu.ParentId,
                menu.MenuGroupId
            );
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

    private async Task ChildEdit(Menu menu)
    {
        foreach (var menuEntity in await FindByCondition(
                     menuCondition =>
                         menuCondition.ParentId == menu.Id
                 ).ToListAsync())
        {
            menuEntity.Path = menu.Id + "/" + menu.Path;
            menuEntity.Depth = menu.Depth + 1;
            menuEntity.MenuGroupId = menu.MenuGroupId;

            Update(menuEntity);

            await ChildEdit(menuEntity);
        }
    }

    public async Task Remove(Menu menu)
    {
        //editing Order Of other menu after deleting this menu in the same parent 
        foreach (var menuEntity in await FindByCondition(
                     menuCondition => menuCondition.ParentId == menu.ParentId &&
                                      menuCondition.MenuGroupId == menu.MenuGroupId &&
                                      menuCondition.DisplayOrder > menu.DisplayOrder
                 ).OrderBy(menuCondition => menuCondition.DisplayOrder).ToListAsync())
        {
            menuEntity.DisplayOrder -= 1;
            Update(menuEntity);
        }

        //Delete the children of Deleted Menu of this Menu 
        await ChildRemove(menu);

        Delete(menu);
        await SaveAsync();
    }

    private async Task ChildRemove(Menu menu)
    {
        foreach (var menuEntity in await FindByCondition(
                     menuCondition =>
                         menuCondition.ParentId == menu.Id
                 ).ToListAsync())
        {
            Delete(menuEntity);
            await ChildRemove(menuEntity);
        }
    }

    public async Task<Menu> GetLastOrder(
        int? parentId,
        int? menuGroupId
    )
    {
        return await FindByCondition(
                menu =>
                    menu.ParentId == parentId &&
                    menu.MenuGroupId == menuGroupId
            )
            .OrderByDescending(o => o.DisplayOrder)
            .FirstOrDefaultAsync();
    }

    public async Task<IList<Menu>> GetByParentGroupOrder(
        int? parentId,
        int? menuGroupId,
        int? pastDisOrder
    ) =>
        await FindByCondition(
            menu => menu.ParentId == parentId &&
                    menu.MenuGroupId == menuGroupId &&
                    menu.DisplayOrder > pastDisOrder
        ).OrderBy(o => o.DisplayOrder).ToListAsync();

    public async Task<bool> UniquePageName(
        string pageName,
        int? menuId
    ) =>
        await FindByCondition(menu => menu.PageName == pageName && menu.Id != menuId).AnyAsync();

    public async Task<IList<Menu>> GetByParentId(
        int? parentId,
        int? menuGroupId
    ) =>
        await FindByCondition(
            menu => menu.ParentId == parentId &&
                    menu.MenuGroupId == menuGroupId
        ).ToListAsync();

    public async Task<IEnumerable<Menu>> Menus() =>
        await FindAll().ToListAsync();

    public async Task<bool> IsExist(int? id) =>
        await FindByCondition(menu => menu.Id.Equals(id)).AnyAsync();

    public async Task<IList<Menu>> GetIncludeModulePage(int? menuGroupId) =>
        await FindByCondition(menu => menu.MenuGroupId.Equals(menuGroupId))
            .Include(menu => menu.ModulePages)
            .ToListAsync();

    public async Task<Menu> GetByPageName(string pageName) =>
        await FindByCondition(menu => menu.PageName == pageName)
            .Include(menu => menu.ModulePages)
            .ThenInclude(modulePage => modulePage.Module)
            .ThenInclude(module => module.Component)
            .Include(menu => menu.ModulePages)
            .ThenInclude(modulePage => modulePage.Module)
            .ThenInclude(module => module.Position)
            .FirstOrDefaultAsync();
}