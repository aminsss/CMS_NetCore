using CMS_NetCore.Interfaces;
using CMS_NetCore.DomainClasses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS_NetCore.Web.ViewComponents
{
    [ViewComponent(Name = "ModuleUsersShow")]
    public class ModuleUsersShow : ViewComponent
    {
        private IUserService _userService;

        public ModuleUsersShow(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int? moduleId)
        {
            ViewBag.moduleId = moduleId;
            var list = await GetUsers();
            return View(list);
        }
        // GET: Admin/Partial
        public async Task<IEnumerable<User>> GetUsers() =>
             await _userService.GetContactctPerson();
    }
}
