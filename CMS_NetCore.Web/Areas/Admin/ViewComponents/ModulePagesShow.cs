using CMS_NetCore.Interfaces;
using CMS_NetCore.DomainClasses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS_NetCore.Web.ViewComponents
{
    [ViewComponent(Name = "ModulePagesShow")]
    public class ModulePagesShow : ViewComponent
    {
        private IMenuGroupService _menuGroupService;

        public ModulePagesShow(IMenuGroupService menuGroupService)
        {
            _menuGroupService = menuGroupService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var list = await GetModulePage();
            return View(list);
        }
        // GET: Admin/Partial
        public async Task<IEnumerable<MenuGroup>> GetModulePage() =>
             await _menuGroupService.MenuGroup();
    }
}
