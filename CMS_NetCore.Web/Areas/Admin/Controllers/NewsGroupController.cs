using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class NewsGroupController : Controller
{
    private readonly INewsGroupService _newsGroupService;

    public NewsGroupController(INewsGroupService newsGroupService)
    {
        _newsGroupService = newsGroupService;
    }

    public async Task<IActionResult> Index(
        int page = 1,
        int pageSize = 1000,
        string searchString = ""
    )
    {
        var newsGroup = await _newsGroupService.GetBySearch(
            page,
            pageSize,
            searchString
        );
        return View(newsGroup.Records);
    }

    public IActionResult Create()
    {
        return PartialView();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Id,GroupTitle,Depth,Path,IsActive,DisplayOrder,ParentId,AliasName,CreatedDate,ModifiedDate,IP")]
        NewsGroup newsGroup
    )
    {
        if (!ModelState.IsValid)
            return View(newsGroup);

        if (newsGroup.ParentId == 0)
        {
            newsGroup.Depth = 0;
            newsGroup.Path = "0";
        }
        else
        {
            var newsGroupParent = await _newsGroupService.GetById(newsGroup.ParentId);
            newsGroup.Depth = newsGroupParent.Depth + 1;
            newsGroup.Path = newsGroupParent.Id + "/" + newsGroupParent.Path;
        }

        await _newsGroupService.Add(newsGroup);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var newsGroup = await _newsGroupService.GetById(id);
        if (newsGroup == null)
            return NotFound();

        return PartialView(newsGroup);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("Id,GroupTitle,Depth,Path,IsActive,DisplayOrder,ParentId,AliasName")]
        NewsGroup newsGroup
    )
    {
        if (id != newsGroup.Id)
            return NotFound();

        if (!ModelState.IsValid)
            return View(newsGroup);

        try
        {
            if (newsGroup.Id == newsGroup.ParentId)
            {
                ModelState.AddModelError(
                    "ParentId",
                    "نمی توانید گروه فعلی را برای گروه والد انتخاب کنید"
                );
                return View(newsGroup);
            }

            if (newsGroup.ParentId == 0)
            {
                newsGroup.Depth = 0;
                newsGroup.Path = "0";
            }
            else
            {
                var newGroupParent = await _newsGroupService.GetById(newsGroup.ParentId);
                foreach (var item in newGroupParent.Path.Split('/'))
                {
                    if (item != newsGroup.Id.ToString())
                        continue;

                    ModelState.AddModelError(
                        "ParentId",
                        "نمی توانید از زیر گروه های این گروه انتخاب کنید"
                    );
                    return View(newsGroup);
                }

                newsGroup.Depth = newGroupParent.Depth + 1;
                newsGroup.Path = newGroupParent.Id + "/" + newGroupParent.Path;
            }

            await _newsGroupService.Edit(newsGroup);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _newsGroupService.NewsGroupExistence(newsGroup.Id))
                return NotFound();
            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<JsonResult> DeleteConfirmed(int id)
    {
        var newsGroup = await _newsGroupService.GetById(id);
        await _newsGroupService.Remove(newsGroup);
        return Json(true);
    }

    public async Task<JsonResult> ErrorGroup(
        int? newsGroupId,
        int? parentId
    )
    {
        if (parentId == 0)
            return Json(true);

        if (newsGroupId == parentId)
            return Json(false);

        var newsGroupParent = await _newsGroupService.GetById(parentId);
        foreach (var item in newsGroupParent.Path.Split('/'))
        {
            if (item == ((newsGroupId).ToString()))
                return Json(false);
        }

        return Json(true);
    }
}