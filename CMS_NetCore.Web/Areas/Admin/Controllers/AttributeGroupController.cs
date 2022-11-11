using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class AttributeGroupController : Controller
{
    private readonly IAttributeGroupService _attributeGroupService;
    private readonly IProductGroupService _productGroupService;

    public AttributeGroupController(
        IAttributeGroupService attributeGroupService,
        IProductGroupService productGroupService
    )
    {
        _attributeGroupService = attributeGroupService;
        _productGroupService = productGroupService;
    }

    public async Task<IActionResult> Index(
        int page = 1,
        int pageSize = 100,
        string searchString = ""
    )
    {
        return View(
            (await _attributeGroupService.GetBySearch(
                page,
                pageSize,
                searchString
            )).Records
        );
    }

    public async Task<IActionResult> Create()
    {
        ViewData["ProductGroupId"] = new SelectList(
            await _productGroupService.ProductGroups(),
            "Id",
            "AliasName"
        );

        return PartialView();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Id,Name,AttributeType,ProductGroupId")] AttributeGroup attributeGroup
    )
    {
        if (ModelState.IsValid)
        {
            await _attributeGroupService.Add(attributeGroup);
            return RedirectToAction(nameof(Index));
        }

        ViewData["ProductGroupId"] = new SelectList(
            await _productGroupService.ProductGroups(),
            "Id",
            "AliasName",
            attributeGroup.ProductGroupId
        );

        return View(attributeGroup);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var attributeGroup = await _attributeGroupService.GetById(id);
        if (attributeGroup == null)
            return NotFound();

        ViewData["ProductGroupId"] = new SelectList(
            await _productGroupService.ProductGroups(),
            "Id",
            "AliasName",
            attributeGroup.ProductGroupId
        );

        return PartialView(attributeGroup);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("Id,Name,AttributeType,ProductGroupId")]
        AttributeGroup attributeGroup
    )
    {
        if (id != attributeGroup.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                await _attributeGroupService.Edit(attributeGroup);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _attributeGroupService.IsExist(attributeGroup.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["ProductGroupId"] = new SelectList(
            await _productGroupService.ProductGroups(),
            "Id",
            "AliasName",
            attributeGroup.ProductGroupId
        );
        return View(attributeGroup);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<JsonResult> DeleteConfirmed(int id)
    {
        var attributeGroup = await _attributeGroupService.GetById(id);
        await _attributeGroupService.Remove(attributeGroup);
        return Json(true);
    }
}