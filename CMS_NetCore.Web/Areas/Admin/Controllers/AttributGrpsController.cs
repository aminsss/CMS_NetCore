using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CMS_NetCore.DataLayer;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;

namespace CMS_NetCore.Web.Areas.Admin.Controllers
{

    
    // GET: Admin/AttributGrps
    
    [Area("Admin")]
    public class AttributGrpsController : Controller
    {
        private readonly AppDbContext _context;
        private IAttributeGrpService _attributeGrpService;
        private IProductGroupService _productGroupService;

        public AttributGrpsController(IAttributeGrpService attributeGrpService, IProductGroupService productGroupService,AppDbContext context)
        {
            _attributeGrpService = attributeGrpService;
            _productGroupService = productGroupService;
            _context = context;
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

            var attributGrp = await _context.AttributGrps
                .Include(a => a.ProductGroup)
                .FirstOrDefaultAsync(m => m.AttributGrpId == id);
            if (attributGrp == null)
            {
                return NotFound();
            }

            return View(attributGrp);
        }

        // GET: Admin/AttributGrps/Create
        public IActionResult Create()
        {
            ViewData["ProductGroupId"] = new SelectList(_context.ProductGroups, "ProductGroupId", "AliasName");
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
                _context.Add(attributGrp);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductGroupId"] = new SelectList(_context.ProductGroups, "ProductGroupId", "AliasName", attributGrp.ProductGroupId);
            return View(attributGrp);
        }

        // GET: Admin/AttributGrps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attributGrp = await _context.AttributGrps.FindAsync(id);
            if (attributGrp == null)
            {
                return NotFound();
            }
            ViewData["ProductGroupId"] = new SelectList(_context.ProductGroups, "ProductGroupId", "AliasName", attributGrp.ProductGroupId);
            return View(attributGrp);
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
                    _context.Update(attributGrp);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttributGrpExists(attributGrp.AttributGrpId))
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
            ViewData["ProductGroupId"] = new SelectList(_context.ProductGroups, "ProductGroupId", "AliasName", attributGrp.ProductGroupId);
            return View(attributGrp);
        }

        // GET: Admin/AttributGrps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attributGrp = await _context.AttributGrps
                .Include(a => a.ProductGroup)
                .FirstOrDefaultAsync(m => m.AttributGrpId == id);
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
            var attributGrp = await _context.AttributGrps.FindAsync(id);
            _context.AttributGrps.Remove(attributGrp);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AttributGrpExists(int id)
        {
            return _context.AttributGrps.Any(e => e.AttributGrpId == id);
        }
    }
}
