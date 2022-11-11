using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS_NetCore.Web.Areas.Admin.Controllers;

[Authorize(Roles = "Admin")]
[Area("Admin")]
public class DefaultController : Controller
{
    [AllowAnonymous]
    public IActionResult Index()
    {
        return View();
    }
}