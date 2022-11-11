using System.Threading.Tasks;
using CMS_NetCore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CMS_NetCore.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class ModuleController : Controller
{
    private readonly IModuleService _moduleService;
    private readonly IComponentService _componentService;

    public ModuleController(
        IModuleService moduleService,
        IComponentService componentService
    )
    {
        _moduleService = moduleService;
        _componentService = componentService;
    }

    public async Task<IActionResult> Index(string searchString = "")
    {
        var modules = await _moduleService.GetBySearch(searchString);
        return View(modules.Records);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<JsonResult> DeleteConfirmed(int id)
    {
        var module = await _moduleService.GetById(id);
        await _moduleService.Remove(module);
        return Json(true);
    }

    public async Task<IActionResult> ComponentList()
    {
        return PartialView(await _componentService.GetAll());
    }
}