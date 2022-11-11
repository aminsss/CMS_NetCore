using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class AttributeItemController : Controller
{
    private readonly IAttributeGroupService _attributeGroupService;
    private readonly IAttributeItemService _attributeItemService;

    public AttributeItemController(
        IAttributeGroupService attributeGroupService,
        IAttributeItemService attributeItemService
    )
    {
        _attributeGroupService = attributeGroupService;
        _attributeItemService = attributeItemService;
    }

    public async Task<IActionResult> Index(int? id)
    {
        var list = await _attributeItemService.GetByAttrGroupId(id);
        return View(list.Records);
    }

    public async Task<IActionResult> Create()
    {
        ViewData["AttributeGroupId"] = new SelectList(
            await _attributeGroupService.GetAll(),
            "Id",
            "Name"
        );
        return PartialView();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Id,Name,value,IdFilter,AttributeGroupId")] AttributeItem attributeItem
    )
    {
        if (ModelState.IsValid)
        {
            await _attributeItemService.Add(attributeItem);
            return RedirectToAction(
                nameof(Index),
                new { id = attributeItem.AttributeGroupId }
            );
        }

        ViewData["AttributeGroupId"] = new SelectList(
            await _attributeGroupService.GetAll(),
            "Id",
            "Name",
            attributeItem.AttributeGroupId
        );

        return View(attributeItem);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var attributeItem = await _attributeItemService.GetById(id);
        if (attributeItem == null)
            return NotFound();

        ViewData["AttributeGroupId"] = new SelectList(
            await _attributeGroupService.GetAll(),
            "Id",
            "Name",
            attributeItem.AttributeGroupId
        );

        return PartialView(attributeItem);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("Id,Name,value,IdFilter,AttributeGroupId")]
        AttributeItem attributeItem
    )
    {
        if (id != attributeItem.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                await _attributeItemService.Edit(attributeItem);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _attributeItemService.IsExist(attributeItem.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(
                nameof(Index),
                new { id = attributeItem.AttributeGroupId }
            );
        }

        ViewData["AttributeGroupId"] = new SelectList(
            await _attributeGroupService.GetAll(),
            "Id",
            "Name",
            attributeItem.AttributeGroupId
        );
        return View(attributeItem);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<JsonResult> DeleteConfirmed(int id)
    {
        var attributeItem = await _attributeItemService.GetById(id);
        await _attributeItemService.Remove(attributeItem);
        return Json(true);
    }
}