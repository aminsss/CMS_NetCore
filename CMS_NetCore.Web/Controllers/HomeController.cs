using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CMS_NetCore.Web.Models;
using Microsoft.AspNetCore.Authorization;
using CMS_NetCore.Interfaces;

namespace CMS_NetCore.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        private IMenuService _menuService;
        private IModulePageService _modulePageService;
        private IUserService _userService;
        private IMessageService _messageService;
        private IProductGroupService _productGroupService;
        private IProductService _productService;
        private INewsGroupService _newsGroupService;
        private INewsService _newsService;

        public HomeController(IMenuService menuService, IUserService userService, IMessageService messageService, IProductGroupService productGroupService, IProductService productService,
                               INewsGroupService newsGroupService, INewsService newsService,IModulePageService modulePageService)
        {
            _menuService = menuService;
            _modulePageService = modulePageService;
            _userService = userService;
            _messageService = messageService;
            _productGroupService = productGroupService;
            _productService = productService;
            _newsGroupService = newsGroupService;
            _newsService = newsService;
        }

        [AllowAnonymous]
        [Route("{id?}")]
        public async Task<IActionResult> Index(string id)
        {
            id = id ?? "Home";
            //if(id.ToLower() == "admin")
            //    return RedirectToAction("Index", "Default", new { area = "Admin" });
            var menu = await _menuService.GetByPageName(id);
            if (menu == null)
                return View("Error");

            return View(menu);
        }

        //public ActionResult _navbar()
        //{
        //    return PartialView(/*_menuService.menus()*/);
        //}

        //public ActionResult NotFound()
        //{
        //    return View("_NotFound");
        //}

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
