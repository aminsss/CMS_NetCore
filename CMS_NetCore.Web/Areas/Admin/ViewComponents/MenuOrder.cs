using CMS_NetCore.Interfaces;
using CMS_NetCore.DomainClasses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS_NetCore.Web.ViewComponents
{
    [ViewComponent(Name = "MenuOrder")]
    public class MenuOrder : ViewComponent
    {
        private IMenuService _menuService ;

        public MenuOrder(IMenuService menuService)
        {
            _menuService = menuService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int parentId , int menuGroupId)
        {
            var list = await GetMenuOrder(parentId, menuGroupId);
            return View(list);
        }
        // GET: Admin/Partial
        public async Task<IEnumerable<Menu>> GetMenuOrder(int parentId, int menuGroupId) =>
             await _menuService.GetByParentId(parentId, menuGroupId);
        
    }
}
