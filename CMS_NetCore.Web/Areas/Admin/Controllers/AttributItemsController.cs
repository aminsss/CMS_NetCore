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
    public class AttributItemsController : Controller
    {
        private IAttributeGrpService _attributeGrpService;
        private IAttributeItemService _attributeItemService;

        public AttributItemsController(IAttributeGrpService attributeGrpService, IAttributeItemService attributeItemService)
        {
            _attributeGrpService = attributeGrpService;
            _attributeItemService = attributeItemService;
        }

        // GET: Admin/AttributItems
        public async Task<IActionResult> Index(int? id)
        {
            var list = await _attributeItemService.GetByAttrGrpId(id);
            return View(list.Records);
        }

        // GET: Admin/AttributItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attributItem = await _attributeItemService.GetById(id);
            if (attributItem == null)
            {
                return NotFound();
            }

            return View(attributItem);
        }

        // GET: Admin/AttributItems/Create
        public async Task<IActionResult> Create()
        {
            ViewData["AttributGrpId"] = new SelectList(await _attributeGrpService.GetAll(), "AttributGrpId", "Name");
            return PartialView();
        }

        // POST: Admin/AttributItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AttributItemId,Name,value,idfilter,AttributGrpId")] AttributItem attributItem)
        {
            if (ModelState.IsValid)
            {
                await _attributeItemService.Add(attributItem);
                return RedirectToAction(nameof(Index), new { id = attributItem.AttributGrpId } );
            }
            ViewData["AttributGrpId"] = new SelectList(await _attributeGrpService.GetAll(), "AttributGrpId", "Name", attributItem.AttributGrpId);
            return View(attributItem);
        }

        // GET: Admin/AttributItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attributItem = await _attributeItemService.GetById(id);
            if (attributItem == null)
            {
                return NotFound();
            }
            ViewData["AttributGrpId"] = new SelectList(await _attributeGrpService.GetAll(), "AttributGrpId", "Name", attributItem.AttributGrpId);
            return PartialView(attributItem);
        }

        // POST: Admin/AttributItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AttributItemId,Name,value,idfilter,AttributGrpId")] AttributItem attributItem)
        {
            if (id != attributItem.AttributItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _attributeItemService.Edit(attributItem);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await _attributeItemService.AttributeItemsExistence(attributItem.AttributItemId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = attributItem.AttributGrpId });
            }
            ViewData["AttributGrpId"] = new SelectList(await _attributeGrpService.GetAll(), "AttributGrpId", "Name", attributItem.AttributGrpId);
            return View(attributItem);
        }

        // POST: Admin/AttributItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> DeleteConfirmed(int id)
        {
            var attributItem = await _attributeItemService.GetById(id);
            await _attributeItemService.Remove(attributItem);
            return Json(true);
        }
    }
}
