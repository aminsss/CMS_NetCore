using System.Collections.Generic;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CMS_NetCore.Web.Areas.Admin.ViewComponents;

[ViewComponent(Name = "ModuleUsersShow")]
public class ModuleUsersShow : ViewComponent
{
    private readonly IUserService _userService;

    public ModuleUsersShow(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<IViewComponentResult> InvokeAsync(int? moduleId)
    {
        ViewBag.moduleId = moduleId;

        return View(await GetUsers());
    }

    // GET: Admin/Partial
    public async Task<IEnumerable<User>> GetUsers() =>
        await _userService.GetContactPerson();
}