using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class DetailItemController : Controller
{
    private readonly IDetailItemService _detailItemService;
    private readonly IDetailGroupService _detailGroupService;

    public DetailItemController(
        IDetailGroupService detailGroupService,
        IDetailItemService detailItemService
    )
    {
        _detailItemService = detailItemService;
        _detailGroupService = detailGroupService;
    }

    public async Task<IActionResult> Index(int? id)
    {
        ViewBag.DetailGroupSelected = id;
        var list = await _detailItemService.GetByDetailGroupId(id);
        return View(list.Records);
    }

    public async Task<IActionResult> Create(int? id)
    {
        ViewBag.DetailGroupSelected = id;

        ViewData["DetailGroupId"] = new SelectList(
            await _detailGroupService.GetAll(),
            "DetailGroupId",
            "Name",
            id
        );

        return PartialView();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("DetailItemId,DetailTitle,DetailType,DetailGroupId")] DetailItem detailItem
    )
    {
        if (ModelState.IsValid)
        {
            await _detailItemService.Add(detailItem);
            return RedirectToAction(
                nameof(Index),
                new { id = detailItem.DetailGroupId }
            );
        }

        ViewData["DetailGroupId"] = new SelectList(
            await _detailGroupService.GetAll(),
            "DetailGroupId",
            "Name",
            detailItem.DetailGroupId
        );
        return View(detailItem);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var detailItem = await _detailItemService.GetById(id);

        if (detailItem == null)
            return NotFound();

        ViewData["DetailGroupId"] = new SelectList(
            await _detailGroupService.GetAll(),
            "DetailGroupId",
            "Name",
            detailItem.DetailGroupId
        );

        return PartialView(detailItem);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("DetailItemId,DetailTitle,DetailType,DetailGroupId")]
        DetailItem detailItem
    )
    {
        if (id != detailItem.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                await _detailItemService.Edit(detailItem);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _detailItemService.IsExist(detailItem.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["DetailGroupId"] = new SelectList(
            await _detailGroupService.GetAll(),
            "DetailGroupId",
            "Name",
            detailItem.DetailGroupId
        );

        return View(detailItem);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<JsonResult> DeleteConfirmed(int id)
    {
        var detailItem = await _detailItemService.GetById(id);
        await _detailItemService.Remove(detailItem);
        return Json(true);
    }
}