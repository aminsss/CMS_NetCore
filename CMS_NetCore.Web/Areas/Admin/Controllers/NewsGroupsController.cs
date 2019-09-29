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
    public class NewsGroupsController : Controller
    {
        private INewsGroupService _newsGroupService;

        public NewsGroupsController(INewsGroupService newsGroupService)
        {
            _newsGroupService = newsGroupService;
        }

        // GET: Admin/NewsGroups
        public async Task<IActionResult> Index(int page = 1, int pageSize = 1000, string searchString = "")
        {
            var newsGroup = await _newsGroupService.GetBySearch(page, pageSize, searchString);
            return View(newsGroup.Records);
        }

        // GET: Admin/NewsGroups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsGroup = await _newsGroupService.GetById(id);
            if (newsGroup == null)
            {
                return NotFound();
            }

            return View(newsGroup);
        }

        // GET: Admin/NewsGroups/Create
        public IActionResult Create()
        {
            return PartialView();
        }

        // POST: Admin/NewsGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NewsGroupId,GroupTitle,Depth,Path,IsActive,DisplayOrder,ParentId,AliasName,AddedDate,ModifiedDate,IP")] NewsGroup newsGroup)
        {
            if (ModelState.IsValid)
            {
                if (newsGroup.ParentId == 0)
                {
                    newsGroup.Depth = 0;
                    newsGroup.Path = "0";
                }
                else
                {
                    var newsGroupParent = await _newsGroupService.GetById(newsGroup.ParentId);
                    newsGroup.Depth = newsGroupParent.Depth + 1;
                    newsGroup.Path = newsGroupParent.NewsGroupId + "/" + newsGroupParent.Path;
                }
                await _newsGroupService.Add(newsGroup);
                return RedirectToAction(nameof(Index));
            }
            return View(newsGroup);
        }

        // GET: Admin/NewsGroups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsGroup = await _newsGroupService.GetById(id);
            if(newsGroup == null)
            {
                return NotFound();
            }
            return PartialView(newsGroup);
        }

        // POST: Admin/NewsGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NewsGroupId,GroupTitle,Depth,Path,IsActive,DisplayOrder,ParentId,AliasName,AddedDate,ModifiedDate,IP")] NewsGroup newsGroup)
        {
            if (id != newsGroup.NewsGroupId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (newsGroup.NewsGroupId == newsGroup.ParentId)
                    {
                        ModelState.AddModelError("ParentId", "نمی توانید گروه فعلی را برای گروه والد انتخاب کنید");
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
                            if (item == ((newsGroup.NewsGroupId).ToString()))
                            {
                                ModelState.AddModelError("ParentId", "نمی توانید از زیر گروه های این گروه انتخاب کنید");
                                return View(newsGroup);
                            }
                        }
                        newsGroup.Depth = newGroupParent.Depth + 1;
                        newsGroup.Path = newGroupParent.NewsGroupId + "/" + newGroupParent.Path;
                    }
                    await _newsGroupService.Edit(newsGroup);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _newsGroupService.NewsGroupExistence(newsGroup.NewsGroupId))
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
            return View(newsGroup);
        }

        // POST: Admin/NewsGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> DeleteConfirmed(int id)
        {
            var newsGroup = await _newsGroupService.GetById(id);
            await _newsGroupService.Remove(newsGroup);
            return Json(true);
        }

        public async Task<JsonResult> ErrorGroup(int? NewsGroupId, int? ParentId)
        {
            if (ParentId == 0)
            {
                return Json(true);
            }
            if (NewsGroupId == ParentId)
            {
                return Json(false);
            }
            var newsGroupParent = await _newsGroupService.GetById(ParentId);
            foreach (var item in newsGroupParent.Path.Split('/'))
            {
                if (item == ((NewsGroupId).ToString()))
                {
                    return Json(false);
                }
            }
            return Json(true);
        }
    }
}
