using CMS_NetCore.Interfaces;
using CMS_NetCore.DomainClasses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS_NetCore.Web.ViewComponents
{
    [ViewComponent(Name = "ModuleMenuList")]
    public class ModuleMenuList : ViewComponent
    {
        private IMenuService _menuService;

        public ModuleMenuList(IMenuService menuService)
        {
            _menuService = menuService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int depth, int parentId, int menuGroupId, int moduleId)
        {
            ViewBag.depth = depth;
            ViewBag.parentId = parentId;
            ViewBag.moduleId = moduleId;
            var list = await GetModulePage(menuGroupId);
            return View(list);
        }
        // GET: Admin/Partial
        public async Task<IEnumerable<Menu>> GetModulePage(int id) =>
             await _menuService.GetIncludeModulePage(id);
    }
}
