using CMS_NetCore.Interfaces;
using CMS_NetCore.DomainClasses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS_NetCore.Web.ViewComponents
{
    [ViewComponent(Name = "MenuList")]
    public class MenuList : ViewComponent
    {
        private IMenuService _menuService ;

        public MenuList(IMenuService menuService)
        {
            _menuService = menuService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int depth, int parentId,int menuGroupId)
        {
            ViewBag.Depth = depth;
            ViewBag.ParentId = parentId;
            var list = await GetMenuList(menuGroupId);
            return View(list);
        }
        // GET: Admin/Partial
        public async Task<IEnumerable<Menu>> GetMenuList(int id)
        {
            var list = await _menuService.GetByMenuGroup(id);
            return list.Records;
        }
    }
}
