using CMS_NetCore.Interfaces;
using CMS_NetCore.DomainClasses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS_NetCore.Web.ViewComponents
{
    [ViewComponent(Name = "UserProfile")]
    public class UserProfile : ViewComponent
    {
        private IUserService _userService;

        public UserProfile(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var item = await RetUserID();
            return View(item);
        }
        // GET: Admin/Partial
        public async Task<User> RetUserID()
        {
            return await _userService.GetUserByIdentity(User.Identity.Name);
        }

        //public ActionResult UserCount()
        //{
        //    return PartialView(_userService.Users());
        //}
        //public ActionResult ProductCount()
        //{
        //    return PartialView(_productService.Products());
        //}
        //public ActionResult OrderCount()
        //{
        //    return PartialView(_orderService.Orders());
        //}
        //public ActionResult IncomeSum()
        //{
        //    return PartialView(_orderService.Orders().Where(s => s.IsFinally == true));
        //}

        //public async Task<IViewComponentResult> InvokeAsync()
        //{
        //    List<string> categories = new List<string>() {
        //        "Category 1", "Category 2", "Category 3", "Category 4", "Category 5"
        //    };
        //    return View("Index", categories);
        //}
    }
}
