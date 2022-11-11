using System.Linq;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class ProductGroupController : Controller
{
    private readonly IProductGroupService _productGroupService;

    public ProductGroupController(IProductGroupService productGroupService)
    {
        _productGroupService = productGroupService;
    }

    public async Task<IActionResult> Index(
        int page = 1,
        int pageSize = 1000,
        string searchString = ""
    )
    {
        searchString ??= "";
        var products = await _productGroupService.GetBySearch(
            page,
            pageSize,
            searchString
        );

        return View(products.Records);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var productGroup = await _productGroupService.GetById(id);
        if (productGroup == null)
            return NotFound();

        return View(productGroup);
    }

    public IActionResult Create()
    {
        return PartialView();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Id,GroupTitle,Depth,Path,IsActive,DisplayOrder,ParentId,AliasName,type")] ProductGroup productGroup
    )
    {
        if (!ModelState.IsValid)
            return View(productGroup);

        await _productGroupService.Add(productGroup);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var productGroup = await _productGroupService.GetById(id);
        if (productGroup == null)
            return NotFound();

        return PartialView(productGroup);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("Id,GroupTitle,Depth,Path,IsActive,DisplayOrder,ParentId,AliasName,type,")]
        ProductGroup productGroup
    )
    {
        if (id != productGroup.Id)
            return NotFound();

        if (!ModelState.IsValid)
            return View(productGroup);

        try
        {
            if (productGroup.Id == productGroup.ParentId)
            {
                ModelState.AddModelError(
                    "ParentId",
                    "نمی توانید گروه فعلی را برای گروه والد انتخاب کنید"
                );
                return View(productGroup);
            }

            if (productGroup.ParentId == 0)
            {
                productGroup.Depth = 0;
                productGroup.Path = "0";
            }
            else
            {
                var newParentGroup = await _productGroupService.GetById(productGroup.ParentId);
                if (newParentGroup.Path.Split('/').Any(item => item == productGroup.Id.ToString()))
                {
                    ModelState.AddModelError(
                        "ParentId",
                        "نمی توانید از زیر گروه های این گروه انتخاب کنید"
                    );
                    return View(productGroup);
                }

                productGroup.Depth = newParentGroup.Depth + 1;
                productGroup.Path = newParentGroup.Id + "/" + newParentGroup.Path;
            }

            await _productGroupService.Edit(productGroup);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _productGroupService.IsExist(productGroup.Id))
                return NotFound();
            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<JsonResult> DeleteConfirmed(int id)
    {
        var productGroup = await _productGroupService.GetById(id);
        await _productGroupService.Remove(productGroup);
        return Json(true);
    }

    public async Task<JsonResult> ErrorGroup(
        int? productGroupId,
        int? parentId
    )
    {
        if (parentId == 0)
            return Json(true);

        if (productGroupId == parentId)
            return Json(false);

        var productGroup = await _productGroupService.GetById(parentId);
        return Json(productGroup.Path.Split('/').All(path => path != productGroupId.ToString()));
    }

    public async Task<JsonResult> UniqueAlias(
        string aliasName,
        int? productGroupId
    )
    {
        return Json(
            !await _productGroupService.UniqueAlias(
                aliasName,
                productGroupId
            )
        );
    }
}