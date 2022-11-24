using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CMS_NetCore.Web.Areas.Admin.ViewComponents;

[ViewComponent(Name = "Sidebar")]
public class Sidebar : ViewComponent
{
    private readonly IUserService _userService;

    public Sidebar(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View(await SidebarList());
    }

    // GET: Admin/Partial
    public async Task<User> SidebarList()
    {
        return await _userService.GetUserByIdentity(User.Identity!.Name);
    }
}