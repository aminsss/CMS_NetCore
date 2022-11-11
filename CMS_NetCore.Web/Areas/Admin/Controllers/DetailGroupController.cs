using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class DetailGroupController : Controller
{
    private readonly IDetailGroupService _detailGroupService;

    public DetailGroupController(IDetailGroupService detailGroupService)
    {
        _detailGroupService = detailGroupService;
    }

    public async Task<IActionResult> Index(
        int page = 1,
        int pageSize = 100,
        string searchString = ""
    )
    {
        return View(
            (await _detailGroupService.GetBySearch(
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
    public async Task<IActionResult> Create([Bind("Id,Name,ProductGroupId")] DetailGroup detailGroup)
    {
        if (!ModelState.IsValid)
            return View(detailGroup);

        await _detailGroupService.Add(detailGroup);
        return RedirectToAction(nameof(Index));
    }

    // GET: Admin/DetailGroups/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var detailGroup = await _detailGroupService.GetById(id);

        if (detailGroup == null)
            return NotFound();

        return PartialView(detailGroup);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("Id,Name,ProductGroupId")]
        DetailGroup detailGroup
    )
    {
        if (id != detailGroup.Id)
            return NotFound();

        if (!ModelState.IsValid)
            return View(detailGroup);

        try
        {
            await _detailGroupService.Edit(detailGroup);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _detailGroupService.IsExist(detailGroup.Id))
                return NotFound();
            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<JsonResult> DeleteConfirmed(int id)
    {
        var detailGroup = await _detailGroupService.GetById(id);
        await _detailGroupService.Remove(detailGroup);
        return Json(true);
    }
}