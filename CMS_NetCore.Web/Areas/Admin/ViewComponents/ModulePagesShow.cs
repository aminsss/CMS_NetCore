using System.Collections.Generic;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CMS_NetCore.Web.Areas.Admin.ViewComponents;

[ViewComponent(Name = "ModulePagesShow")]
public class ModulePagesShow : ViewComponent
{
    private readonly IMenuGroupService _menuGroupService;

    public ModulePagesShow(IMenuGroupService menuGroupService)
    {
        _menuGroupService = menuGroupService;
    }

    public async Task<IViewComponentResult> InvokeAsync(int moduleId)
    {
        ViewBag.moduleId = moduleId;
        var list = await GetModulePage();
        return View(list);
    }

    // GET: Admin/Partial
    public async Task<IEnumerable<MenuGroup>> GetModulePage() =>
        await _menuGroupService.MenuGroup();
}