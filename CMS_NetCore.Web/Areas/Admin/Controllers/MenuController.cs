using System.Linq;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class MenuController : Controller
{
    private readonly IMenuService _menuService;
    private readonly IMenuGroupService _menuGroupService;

    public MenuController(
        IMenuGroupService menuGroupService,
        IMenuService menuService
    )
    {
        _menuGroupService = menuGroupService;
        _menuService = menuService;
    }

    public IActionResult Index(int? id)
    {
        ViewBag.MnuGroupSelected = id;
        return View();
    }

    public async Task<IActionResult> Create(int? id)
    {
        ViewBag.MenuGroupSelected = id;
        ViewData["MenuGroupId"] = new SelectList(
            await _menuGroupService.MenuGroup(),
            "Id",
            "Title"
        );

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind(
            "Id,Title,PageName,Depth,Path,IsActive,DisplayOrder,ParentId,Description,PageType,PageContent,MenuGroupId"
        )]
        Menu menu
    )
    {
        if (ModelState.IsValid)
        {
            await _menuService.Add(menu);
            return RedirectToAction(
                nameof(Index),
                new { id = menu.MenuGroupId }
            );
        }

        ViewData["MenuGroupId"] = new SelectList(
            await _menuGroupService.MenuGroup(),
            "Id",
            "Title",
            menu.MenuGroupId
        );

        return View(menu);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var menu = await _menuService.GetById(id);
        if (menu == null)
            return NotFound();

        ViewData["MenuGroupId"] = new SelectList(
            await _menuGroupService.MenuGroup(),
            "Id",
            "Title",
            menu.MenuGroupId
        );

        return View(menu);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        Menu menu,
        int? pastDisOrder,
        int? pastParentId,
        int? pastGroupId
    )
    {
        if (id != menu.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                if (menu.Id == menu.ParentId)
                {
                    ModelState.AddModelError(
                        "ParentId",
                        "نمی توانید گروه فعلی را برای گروه والد انتخاب کنید"
                    );

                    return View(menu);
                }

                if (menu.ParentId == 0)
                {
                    menu.Depth = 0;
                    menu.Path = "0";
                }
                else
                {
                    var newParentMenu = await _menuService.GetById(menu.ParentId);

                    if (newParentMenu.Path.Split('/').Any(item => item == menu.Id.ToString()))
                    {
                        ModelState.AddModelError(
                            "ParentId",
                            "نمی توانید از زیر گروه های این گروه انتخاب کنید"
                        );
                        return View(menu);
                    }

                    menu.Depth = newParentMenu.Depth + 1;
                    menu.Path = newParentMenu.Id + "/" + newParentMenu.Path;
                }

                await _menuService.Edit(
                    menu,
                    pastDisOrder,
                    pastParentId,
                    pastGroupId
                );
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _menuService.IsExist(menu.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(
                nameof(Index),
                new { id = menu.MenuGroupId }
            );
        }

        ViewData["MenuGroupId"] = new SelectList(
            await _menuGroupService.MenuGroup(),
            "Id",
            "Title",
            menu.MenuGroupId
        );

        return View(menu);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<JsonResult> DeleteConfirmed(int id)
    {
        var menu = await _menuService.GetById(id);
        await _menuService.Remove(menu);
        return Json(true);
    }

    public async Task<JsonResult> UniquePageName(
        string pageName,
        int? menuId
    )
    {
        return Json(
            !await _menuService.UniquePageName(
                pageName,
                menuId
            )
        );
    }

    public ActionResult MenuList(int? id)
    {
        return ViewComponent(
            "MenuList",
            new { depth = 0, parentId = 0, menuGroupId = id }
        );
    }

    public ActionResult GroupsOfProduct()
    {
        return ViewComponent("ProductGroupList");
    }

    public ActionResult GroupsOfNews()
    {
        return ViewComponent("NewsGroupList");
    }
}