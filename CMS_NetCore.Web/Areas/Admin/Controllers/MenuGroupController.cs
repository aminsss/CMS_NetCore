using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class MenuGroupController : Controller
{
    private readonly IMenuGroupService _menuGroupService;

    public MenuGroupController(IMenuGroupService menuGroupService)
    {
        _menuGroupService = menuGroupService;
    }

    public async Task<IActionResult> Index(
        int page = 1,
        int pageSize = 1000,
        string searchString = ""
    )
    {
        return View(
            (await _menuGroupService.GetBySearch(
                page,
                pageSize,
                searchString
            )).Records
        );
    }

    public IActionResult Create()
    {
        return PartialView();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,Type,IsActive")] MenuGroup menuGroup)
    {
        if (!ModelState.IsValid)
            return View(menuGroup);

        await _menuGroupService.Add(menuGroup);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var menuGroup = await _menuGroupService.GetById(id);
        if (menuGroup == null)
            return NotFound();

        return PartialView(menuGroup);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("Id,Title,Type,IsActive")] MenuGroup menuGroup
    )
    {
        if (id != menuGroup.Id)
            return NotFound();

        if (!ModelState.IsValid)
            return View(menuGroup);

        try
        {
            await _menuGroupService.Edit(menuGroup);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _menuGroupService.IsExist(menuGroup.Id))
                return NotFound();
            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<JsonResult> DeleteConfirmed(int id)
    {
        var menuGroup = await _menuGroupService.GetById(id);
        await _menuGroupService.Remove(menuGroup);
        return Json(true);
    }
}