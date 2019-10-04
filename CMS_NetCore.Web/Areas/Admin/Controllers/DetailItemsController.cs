using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CMS_NetCore.Interfaces;
using CMS_NetCore.DomainClasses;

namespace CMS_NetCore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DetailItemsController : Controller
    {
        private IDetailItemService _detailItemService;
        private IDetailGroupService _detailGroupService;

        public DetailItemsController(IDetailGroupService detailGroupService, IDetailItemService detailItemService)
        {
            _detailItemService = detailItemService;
            _detailGroupService = detailGroupService;
        }

        // GET: Admin/DetailItems
        public async Task<IActionResult> Index(int? id)
        {
            ViewBag.DetailGroupSelected = id;
            var list = await _detailItemService.GetByDetailGroupId(id);
            return View(list.Records);
        }

        // GET: Admin/DetailItems/Create
        public async Task<IActionResult> Create(int? id)
        {
            ViewBag.DetailGroupSelected = id;
            ViewData["DetailGroupId"] = new SelectList(await _detailGroupService.GetAll(), "DetailGroupId", "Name", id);
            return PartialView();
        }

        // POST: Admin/DetailItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DetailItemId,DetailTitle,DetailType,DetailGroupId")] DetailItem detailItem)
        {
            if (ModelState.IsValid)
            {
                await _detailItemService.Add(detailItem);
                return RedirectToAction(nameof(Index) , new { id = detailItem.DetailGroupId });
            }
            ViewData["DetailGroupId"] = new SelectList(await _detailGroupService.GetAll(), "DetailGroupId", "Name", detailItem.DetailGroupId);
            return View(detailItem);
        }

        // GET: Admin/DetailItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detailItem = await _detailItemService.GetById(id);
            if (detailItem == null)
            {
                return NotFound();
            }
            ViewData["DetailGroupId"] = new SelectList(await _detailGroupService.GetAll(), "DetailGroupId", "Name", detailItem.DetailGroupId);
            return PartialView(detailItem);
        }

        // POST: Admin/DetailItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DetailItemId,DetailTitle,DetailType,DetailGroupId")] DetailItem detailItem)
        {
            if (id != detailItem.DetailItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _detailItemService.Edit(detailItem);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _detailItemService.DetailItemExistence(detailItem.DetailItemId))
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
            ViewData["DetailGroupId"] = new SelectList(await _detailGroupService.GetAll(), "DetailGroupId", "Name", detailItem.DetailGroupId);
            return View(detailItem);
        }

        // POST: Admin/DetailItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> DeleteConfirmed(int id)
        {
            var detailItem = await _detailItemService.GetById(id);
            await _detailItemService.Remove(detailItem);
            return Json(true);
        }

    }
}
