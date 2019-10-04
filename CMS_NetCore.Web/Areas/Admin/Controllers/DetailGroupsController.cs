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
    public class DetailGroupsController : Controller
    {
        private IProductGroupService _productGroupService;
        private IDetailGroupService _detailGroupService;

        public DetailGroupsController(IProductGroupService productGroupService, IDetailGroupService detailGroupService)
        {
            _detailGroupService = detailGroupService;
            _productGroupService = productGroupService;
        }

        // GET: Admin/DetailGroups
        public async Task<IActionResult> Index(int page = 1, int pageSize = 100, string searchString = "")
        {
            var list = await _detailGroupService.GetBySearch(page, pageSize, searchString);
            return View(list.Records);
        }


        // GET: Admin/DetailGroups/Create
        public IActionResult Create()
        {
            return PartialView();
        }

        // POST: Admin/DetailGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DetailGroupId,Name,ProductGroupId")] DetailGroup detailGroup)
        {
            if (ModelState.IsValid)
            {
                await _detailGroupService.Add(detailGroup);
                return RedirectToAction(nameof(Index));
            }
            return View(detailGroup);
        }

        // GET: Admin/DetailGroups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detailGroup = await _detailGroupService.GetById(id);
            if (detailGroup == null)
            {
                return NotFound();
            }
            return PartialView(detailGroup);
        }

        // POST: Admin/DetailGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DetailGroupId,Name,ProductGroupId")] DetailGroup detailGroup)
        {
            if (id != detailGroup.DetailGroupId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _detailGroupService.Edit(detailGroup);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await _detailGroupService.DetailGroupExistence(detailGroup.DetailGroupId))
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
            return View(detailGroup);
        }

        // POST: Admin/DetailGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> DeleteConfirmed(int id)
        {
            var detailGroup = await _detailGroupService.GetById(id);
            await _detailGroupService.Remove(detailGroup);
            return Json(true);
        }
    }
}
