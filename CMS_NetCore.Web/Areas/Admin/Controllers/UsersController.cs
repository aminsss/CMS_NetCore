using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using CMS_NetCore.ViewModels;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace CMS_NetCore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly IChartPost _chartPost;
        private readonly IUserService _userService;
        private readonly IHostingEnvironment _env;


        public UsersController(IUserService userService,IRoleService roleService,IChartPost chartPost, IHostingEnvironment env)
        {
            _userService = userService;
            _roleService = roleService;
            _chartPost = chartPost;
            _env = env;
        }

        // GET: Admin/Users
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetUsers(int page = 1, int pageSize = 5, string searchString = "")
        {
            searchString = searchString ?? string.Empty;

            var list = await _userService.GetBySearch(page, pageSize, searchString);

            int totalCount = list.TotalCount;
            int numPages = (int)Math.Ceiling((float)totalCount / pageSize);


            var getList = (from obj in list.Records
                           select new
                           {
                               profile = obj.Profile,
                               moblie = obj.moblie,
                               userid = obj.UserId,
                               rolename = obj.Role.RoleName,
                               name = obj.Name,
                               isactive = obj.ISActive,
                               email = obj.Email,
                               addedate = obj.AddedDate,
                           });

            return Json(new { getList, totalCount, numPages });
        }

        // GET: Admin/Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userService.GetById(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Admin/Users/Create
        public async Task<IActionResult> Create()
        {
            ViewData["RoleId"] = new SelectList(await _roleService.Roles(), "RoleId", "RoleName");
            ViewData["chartPostId"] = new SelectList(await _chartPost.chartPosts(), "chartPostId", "Postduty");
            return View();
        }

        // POST: Admin/Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user, IFormFile file)
        {
            var fileName = "no-photo.png";
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    var uploads = Path.Combine(_env.WebRootPath, "Upload\\Profile");
                    fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);

                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    //---------------------resize Images ----------------------
                    InsertShowImage.ImageResizer img = new InsertShowImage.ImageResizer(128);
                    img.Resize(Path.Combine(uploads, fileName), Path.Combine(uploads, "thumbnail", fileName));
                }
            }
            user.Profile = fileName;
            //---------------------------
            await _userService.Add(user);
            ViewData["RoleId"] = new SelectList(await _roleService.Roles(), "RoleId", "RoleName", user.RoleId);
            ViewData["chartPostId"] = new SelectList(await _chartPost.chartPosts(), "chartPostId", "Postduty", user.chartPostId);
            return RedirectToAction(nameof(Index));
        }

       
        // GET: Admin/Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userService.GetById(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(await _roleService.Roles(), "RoleId", "RoleName", user.RoleId);
            ViewData["chartPostId"] = new SelectList(await _chartPost.chartPosts(), "chartPostId", "Postduty", user.chartPostId);
            return View(user);
        }

        // POST: Admin/Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User user,IFormFile file)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var fileName = "no-photo.jpg";
                    if (file != null )
                    {
                        var uploads = Path.Combine(_env.WebRootPath, "Upload\\Profile");
                        fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);

                        using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        //---------------------resize Images ----------------------
                        InsertShowImage.ImageResizer img = new InsertShowImage.ImageResizer(128);
                        img.Resize(Path.Combine(uploads, fileName), Path.Combine(uploads, "thumbnail", fileName));
                    }
                    user.Profile = fileName;
                    await _userService.Edit(user);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _userService.UserExistence(user.UserId) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(await _roleService.Roles(), "RoleId", "RoleName", user.RoleId);
            ViewData["chartPostId"] = new SelectList(await _chartPost.chartPosts(), "chartPostId", "Postduty", user.chartPostId);
            return View(user);
        }

        // GET: Admin/Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userService.GetById(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _userService.GetById(id);
            await _userService.Remove(user);
            return RedirectToAction(nameof(Index));
        }

        public async Task<JsonResult> UniqueEmail(string Email, int UserId)
        {
            if (await _userService.UniqueEmail(Email, UserId) != null)
            {
                return Json(false);
            }
            else
            {
                return Json(true);
            }
        }

        public async Task<JsonResult> Uniquemoblie(string moblie, int UserId)
        {
            if (await _userService.UniqueMobile(moblie, UserId) != null)
            {
                return Json(false);
            }
            else
            {
                return Json(true);
            }
        }

        public async Task<ActionResult> RetUserID()
        {
            return PartialView(await _userService.GetUserByIdentity(User.Identity.Name));
        }

        public async Task<ActionResult> changePass(int? id)
        {
            User users = await _userService.GetById(id);
            changePassViewModel ch = new changePassViewModel()
            {
                UserId = users.UserId,
            };
            return View(ch);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> changePass(changePassViewModel chang)
        {
            var Qchange = await _userService.GetUserByPassword(chang.UserId, chang.Oldpass);
            if (ModelState.IsValid)
            {
                if (Qchange != null)
                {
                    await _userService.EditPassword(Qchange, chang.pass);
                    Qchange.Password = chang.pass;
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Oldpass", "رمز را به درستی واردنمایید");
                }
            }
            return View();
        }


    }
}
