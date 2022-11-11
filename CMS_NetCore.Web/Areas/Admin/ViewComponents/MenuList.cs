using System.Collections.Generic;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CMS_NetCore.Web.Areas.Admin.ViewComponents;

[ViewComponent(Name = "MenuList")]
public class MenuList : ViewComponent
{
    private readonly IMenuService _menuService;

    public MenuList(IMenuService menuService)
    {
        _menuService = menuService;
    }

    public async Task<IViewComponentResult> InvokeAsync(
        int depth,
        int parentId,
        int menuGroupId
    )
    {
        ViewBag.Depth = depth;
        ViewBag.ParentId = parentId;
        var list = await GetMenuList(menuGroupId);
        return null;
    }

    // GET: Admin/Partial
    public async Task<IEnumerable<Menu>> GetMenuList(int id)
    {
        var list = await _menuService.GetByMenuGroup(id);
        return list.Records;
    }
}