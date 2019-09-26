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
    public class MenuGroupsController : Controller
    {
        private IMenuGroupService _menuGroupService;

        public MenuGroupsController(IMenuGroupService menuGroupService)
        {
            _menuGroupService = menuGroupService;
        }

        // GET: Admin/MenuGroups
        public async Task<IActionResult> Index(int page = 1, int pageSize = 1000, string searchstring = "")
        {
            var List = await _menuGroupService.GetBySearch(page, pageSize, searchstring);
            return View(List.Records);
        }

        // GET: Admin/MenuGroups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuGroup = await _menuGroupService.GetById(id);
            if (menuGroup == null)
            {
                return NotFound();
            }

            return View(menuGroup);
        }

        // GET: Admin/MenuGroups/Create
        public IActionResult Create()
        {
            return PartialView();
        }

        // POST: Admin/MenuGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MenuGroupId,MenuTitile,MenuType,IsActive")] MenuGroup menuGroup)
        {
            if (ModelState.IsValid)
            {
                await _menuGroupService.Add(menuGroup);
                return RedirectToAction(nameof(Index));
            }
            return View(menuGroup);
        }

        // GET: Admin/MenuGroups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuGroup = await _menuGroupService.GetById(id);
            if (menuGroup == null)
            {
                return NotFound();
            }
            return PartialView(menuGroup);
        }

        // POST: Admin/MenuGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MenuGroupId,MenuTitile,MenuType,IsActive")] MenuGroup menuGroup)
        {
            if (id != menuGroup.MenuGroupId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                   await _menuGroupService.Edit(menuGroup);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _menuGroupService.MenuGroupExistence(menuGroup.MenuGroupId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(menuGroup);
        }

        // POST: Admin/MenuGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> DeleteConfirmed(int id)
        {
            var menuGroup = await _menuGroupService.GetById(id);
            await _menuGroupService.Remove(menuGroup);
            return Json(true);
        }
    }
}
