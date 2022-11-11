using System.Collections.Generic;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CMS_NetCore.Web.Areas.Admin.ViewComponents;

[ViewComponent(Name = "MenuOrder")]
public class MenuOrder : ViewComponent
{
    private IMenuService _menuService;

    public MenuOrder(IMenuService menuService)
    {
        _menuService = menuService;
    }

    public async Task<IViewComponentResult> InvokeAsync(
        int parentId,
        int menuGroupId
    )
    {
        var list = await GetMenuOrder(
            parentId,
            menuGroupId
        );
        return null;
    }

    // GET: Admin/Partial
    public async Task<IEnumerable<Menu>> GetMenuOrder(
        int parentId,
        int menuGroupId
    ) =>
        await _menuService.GetByParentId(
            parentId,
            menuGroupId
        );
}