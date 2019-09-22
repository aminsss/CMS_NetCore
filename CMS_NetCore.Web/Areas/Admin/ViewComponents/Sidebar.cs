using CMS_NetCore.Interfaces;
using CMS_NetCore.DomainClasses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS_NetCore.Web.ViewComponents
{
    [ViewComponent(Name = "Sidebar")]
    public class Sidebar : ViewComponent
    {
        private IUserService _userService;

        public Sidebar(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var item = await SidebarList();
            return View(item);
        }
        // GET: Admin/Partial
        public async Task<User> SidebarList()
        {
           return await _userService.GetUserByIdentity(User.Identity.Name);
        }
    }
}
