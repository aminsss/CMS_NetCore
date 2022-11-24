using System.Collections.Generic;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CMS_NetCore.Web.Areas.Admin.ViewComponents;

[ViewComponent(Name = "ModuleOrder")]
public class ModuleOrder : ViewComponent
{
    private IModuleService _moduleService;

    public ModuleOrder(IModuleService moduleService)
    {
        _moduleService = moduleService;
    }

    public async Task<IViewComponentResult> InvokeAsync(int positionId)
    {
        return View(await GetModuleOrder(positionId));
    }

    // GET: Admin/Partial
    public async Task<IEnumerable<Module>> GetModuleOrder(int id) =>
        await _moduleService.GetByPositionId(id);
}