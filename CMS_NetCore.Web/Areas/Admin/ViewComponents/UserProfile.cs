using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CMS_NetCore.Web.Areas.Admin.ViewComponents;

[ViewComponent(Name = "UserProfile")]
public class UserProfile : ViewComponent
{
    private readonly IUserService _userService;

    public UserProfile(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View(await ReturnUserId());
    }

    // GET: Admin/Partial
    public async Task<User> ReturnUserId()
    {
        return await _userService.GetUserByIdentity(User.Identity!.Name);
    }
}