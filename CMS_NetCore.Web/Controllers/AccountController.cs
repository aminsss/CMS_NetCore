using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CMS_NetCore.DataLayer;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using CMS_NetCore.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace CMS_NetCore.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IUserService _userService;

        public AccountController(AppDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        // GET: Account
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            var userToken = await _userService.Authenticate(login.UserName.Trim(), login.Password.Trim());

            if (userToken != null)
            {
                ////Save token in session object
                //HttpContext.Session.SetString("JWToken", userToken);

                HttpContext.Response.Cookies.Append("JWToken", userToken, new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.UtcNow.AddDays(7),
                    SameSite = SameSiteMode.Strict
                });

                return Redirect("~/Account/Login");
            }

            return Redirect("~/Home/Index");

        }

        public async Task<IActionResult> Logoff()
        {
            await HttpContext.SignOutAsync(
              CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("~/Account/LogIn");
        }
       
    }
}
