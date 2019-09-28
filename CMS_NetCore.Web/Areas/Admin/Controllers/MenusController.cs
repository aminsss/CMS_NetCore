using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;

namespace CMS_NetCore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MenusController : Controller
    {
        private IMenuService _menuService;
        private IMenuGroupService _menuGroupService;

        public MenusController(IMenuGroupService menuGroupService, IMenuService menuService)
        {
            _menuGroupService = menuGroupService;
            _menuService = menuService;
        }

        // GET: Admin/Menus
        public IActionResult Index(int? id)
        {
            ViewBag.MnuGroupSelected = id;
            return View();
        }

        // GET: Admin/Menus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await _menuService.GetById(id);
            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        // GET: Admin/Menus/Create
        public async Task<IActionResult> Create(int? id)
        {
            ViewBag.MenuGroupSelected = id;
            ViewData["MenuGroupId"] = new SelectList(await _menuGroupService.MenuGroup(), "MenuGroupId", "MenuTitile");
            return View();
        }

        // POST: Admin/Menus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MenuId,MenuTitle,PageName,Depth,Path,IsActive,DisplayOrder,ParentId,Description,PageType,PageContetnt,MenuGroupId")] Menu menu)
        {
            if (ModelState.IsValid)
            {

                await _menuService.Add(menu);
                return RedirectToAction(nameof(Index), new { id = menu.MenuGroupId });
            }
            ViewData["MenuGroupId"] = new SelectList(await _menuGroupService.MenuGroup(), "MenuGroupId", "MenuTitile", menu.MenuGroupId);
            return View(menu);
        }

        // GET: Admin/Menus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await _menuService.GetById(id);
            if (menu == null)
            {
                return NotFound();
            }
            ViewData["MenuGroupId"] = new SelectList(await _menuGroupService.MenuGroup(), "MenuGroupId", "MenuTitile", menu.MenuGroupId);
            return View(menu);
        }

        // POST: Admin/Menus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Menu menu, int? pastDisOrder, int? pastParentId, int? pastGroupId)
        {
            if (id != menu.MenuId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (menu.MenuId == menu.ParentId)
                    {
                        ModelState.AddModelError("ParentId", "نمی توانید گروه فعلی را برای گروه والد انتخاب کنید");
                        return View(menu);
                    }
                    if (menu.ParentId == 0)
                    {
                        menu.Depth = 0;
                        menu.Path = "0";
                    }
                    else
                    {
                        var newParent_Menu = await _menuService.GetById(menu.ParentId);
                        foreach (var item in newParent_Menu.Path.Split('/'))
                        {
                            if (item == ((menu.MenuId).ToString()))
                            {
                                ModelState.AddModelError("ParentId", "نمی توانید از زیر گروه های این گروه انتخاب کنید");
                                return View(menu);
                            }
                        }
                        menu.Depth = newParent_Menu.Depth + 1;
                        menu.Path = newParent_Menu.MenuId + "/" + newParent_Menu.Path;
                    }
                    await _menuService.Edit(menu, pastDisOrder, pastParentId, pastGroupId);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _menuService.MenuExistence(menu.MenuId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = menu.MenuGroupId });
            }
            ViewData["MenuGroupId"] = new SelectList(await _menuGroupService.MenuGroup(), "MenuGroupId", "MenuTitile", menu.MenuGroupId);
            return View(menu);
        }

        // POST: Admin/Menus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> DeleteConfirmed(int id)
        {
            var menu = await _menuService.GetById(id);
            await _menuService.Remove(menu);
            return Json(true);
        }

        public async Task<JsonResult> UniquePageName(string pageName, int? menuId)
        {
            if (await _menuService.UniquePageName(pageName, menuId))
            {
                return Json(false);
            }
            else
            {
                return Json(true);
            }
        }

        public ActionResult MenuList(int? id)
        {
            return ViewComponent("MenuList", new { depth = 0, parentId = 0, menuGroupId = id });
        }

        public ActionResult GroupsOfProduct()
        {
            return ViewComponent("GroupsOfProduct");
        }

        public ActionResult GroupsOfNews()
        {
            return ViewComponent("GroupsOfNews");
        }

       

    }
}
