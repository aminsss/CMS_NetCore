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
    [Area("Admin")]
    public class ProductGroupsController : Controller
    {
        private IProductGroupService _productGroupService;

        public ProductGroupsController(IProductGroupService productGroupService)
        {
            _productGroupService = productGroupService;
        }

        // GET: Admin/ProductGroups
        public async Task<IActionResult> Index(int page = 1, int pageSize = 1000, string searchString = "")
        {
            searchString = searchString ?? "";
            var list = await _productGroupService.GetBySearch(page, pageSize, searchString);
            return View(list.Records);
        }

        // GET: Admin/ProductGroups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productGroup = await _productGroupService.GetById(id);
            if (productGroup == null)
            {
                return NotFound();
            }

            return View(productGroup);
        }

        // GET: Admin/ProductGroups/Create
        public IActionResult Create()
        {
            return PartialView();
        }

        // POST: Admin/ProductGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductGroupId,GroupTitle,Depth,Path,IsActive,DisplayOrder,ParentId,AliasName,type,AddedDate,ModifiedDate,IP")] ProductGroup productGroup)
        {
            if (ModelState.IsValid)
            {
               
                await _productGroupService.Add(productGroup);
                return RedirectToAction(nameof(Index));
            }
            return View(productGroup);
        }

        // GET: Admin/ProductGroups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productGroup = await _productGroupService.GetById(id);
            if (productGroup == null)
            {
                return NotFound();
            }
            return PartialView(productGroup);
        }

        // POST: Admin/ProductGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductGroupId,GroupTitle,Depth,Path,IsActive,DisplayOrder,ParentId,AliasName,type,AddedDate,ModifiedDate,IP")] ProductGroup productGroup)
        {
            if (id != productGroup.ProductGroupId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (productGroup.ProductGroupId == productGroup.ParentId)
                    {
                        ModelState.AddModelError("ParentId", "نمی توانید گروه فعلی را برای گروه والد انتخاب کنید");
                        return View(productGroup);
                    }
                    if (productGroup.ParentId == 0)
                    {
                        productGroup.Depth = 0;
                        productGroup.Path = "0";
                    }
                    else
                    {
                        var NewParent_Group = await _productGroupService.GetById(productGroup.ParentId);
                        foreach (var item in NewParent_Group.Path.Split('/'))
                        {
                            if (item == ((productGroup.ProductGroupId).ToString()))
                            {
                                ModelState.AddModelError("ParentId", "نمی توانید از زیر گروه های این گروه انتخاب کنید");
                                return View(productGroup);
                            }
                        }
                        productGroup.Depth = NewParent_Group.Depth + 1;
                        productGroup.Path = NewParent_Group.ProductGroupId + "/" + NewParent_Group.Path;
                    }
                    await _productGroupService.Edit(productGroup);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _productGroupService.ProductGroupExistense(productGroup.ProductGroupId))
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
            return View(productGroup);
        }

        // GET: Admin/ProductGroups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productGroup = await _productGroupService.GetById(id);
            if (productGroup == null)
            {
                return NotFound();
            }

            return PartialView(productGroup);
        }

        // POST: Admin/ProductGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> DeleteConfirmed(int id)
        {
            var productGroup = await _productGroupService.GetById(id);
            await _productGroupService.Remove(productGroup);
            return Json(true);
        }

       

        public async Task<JsonResult> ErrorGroup(int? productGroupId, int? parentId)
        {
            if (parentId == 0)
            {
                return Json(true);
            }
            if (productGroupId == parentId)
            {
                return Json(false);
            }
            var product_Group = await _productGroupService.GetById(parentId);
            foreach (var item in product_Group.Path.Split('/'))
            {
                if (item == ((productGroupId).ToString()))
                {
                    return Json(false);
                }
            }
            return Json(true);
        }

        public async Task<JsonResult> UniqueAlias(string aliasName, int? productGroupId)
        {
            if (await _productGroupService.UniqueAlias(aliasName, productGroupId))
            {
                return Json(false);
            }
            else
            {
                return Json(true);
            }
        }

        public async Task<ActionResult> GroupItems()
        {
            return PartialView(await _productGroupService.ProductGroups());
        }
    }
}
