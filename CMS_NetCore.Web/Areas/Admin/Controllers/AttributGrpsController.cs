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

    
    // GET: Admin/AttributGrps
    
    [Area("Admin")]
    public class AttributGrpsController : Controller
    {
        //private readonly AppDbContext _context;
        private IAttributeGrpService _attributeGrpService;
        private IProductGroupService _productGroupService;

        public AttributGrpsController(IAttributeGrpService attributeGrpService, IProductGroupService productGroupService)
        {
            _attributeGrpService = attributeGrpService;
            _productGroupService = productGroupService;
        }
            
         
        // GET: Admin/AttributGrps
        public async Task<IActionResult> Index(int page = 1, int pageSize = 100, string searchString = "")
        {
            var attributGrp = await _attributeGrpService.GetBySearch(page, pageSize, searchString);
            return View(attributGrp.Records);
        }

        // GET: Admin/AttributGrps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attributGrp = await _attributeGrpService.GetById(id); 
            if (attributGrp == null)
            {
                return NotFound();
            }

            return View(attributGrp);
        }

        // GET: Admin/AttributGrps/Create
        public async Task<IActionResult> Create()
        {
            ViewData["ProductGroupId"] = new SelectList( await _productGroupService.ProductGroups(), "ProductGroupId", "AliasName");
            return View();
        }

        // POST: Admin/AttributGrps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AttributGrpId,Name,Attr_type,ProductGroupId")] AttributGrp attributGrp)
        {
            if (ModelState.IsValid)
            {
                await _attributeGrpService.Add(attributGrp);
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductGroupId"] = new SelectList(await _productGroupService.ProductGroups(), "ProductGroupId", "AliasName", attributGrp.ProductGroupId);
            return View(attributGrp);
        }

        // GET: Admin/AttributGrps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attributGrp = await _attributeGrpService.GetById(id);
            if (attributGrp == null)
            {
                return NotFound();
            }
            ViewData["ProductGroupId"] = new SelectList(await _productGroupService.ProductGroups(), "ProductGroupId", "AliasName", attributGrp.ProductGroupId);
            return PartialView(attributGrp);
        }

        // POST: Admin/AttributGrps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AttributGrpId,Name,Attr_type,ProductGroupId")] AttributGrp attributGrp)
        {
            if (id != attributGrp.AttributGrpId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _attributeGrpService.Edit(attributGrp);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _attributeGrpService.AttributeGrpExistence(attributGrp.AttributGrpId))
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
            ViewData["ProductGroupId"] = new SelectList(await _productGroupService.ProductGroups(), "ProductGroupId", "AliasName", attributGrp.ProductGroupId);
            return View(attributGrp);
        }

        // GET: Admin/AttributGrps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attributGrp = await _attributeGrpService.GetById(id);
            if (attributGrp == null)
            {
                return NotFound();
            }

            return View(attributGrp);
        }

        // POST: Admin/AttributGrps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var attributGrp = await _attributeGrpService.GetById(id);
            await _attributeGrpService.Remove(attributGrp);
            return RedirectToAction(nameof(Index));
        }

    }
}
