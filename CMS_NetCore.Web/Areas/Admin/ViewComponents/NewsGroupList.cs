using CMS_NetCore.Interfaces;
using CMS_NetCore.DomainClasses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS_NetCore.Web.ViewComponents
{
    [ViewComponent(Name = "NewsGroupList")]
    public class NewsGroupList : ViewComponent
    {
        private INewsGroupService _newsGroupService ;

        public NewsGroupList(INewsGroupService newsGroupService)
        {
            _newsGroupService = newsGroupService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int depth, int parentId)
        {
            ViewBag.Depth = depth;
            ViewBag.ParentId = parentId;
            var list = await GetNewsGroup();
            return View(list);
        }
        // GET: Admin/Partial
        public async Task<IEnumerable<NewsGroup>> GetNewsGroup()
        {
            return await _newsGroupService.GetAll();
        }
    }
}
