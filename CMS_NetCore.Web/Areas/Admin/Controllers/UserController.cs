using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using CMS_NetCore.ServiceLayer;
using CMS_NetCore.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class UserController : Controller
{
    private readonly IRoleService _roleService;
    private readonly IChartPost _chartPost;
    private readonly IUserService _userService;
    private readonly IWebHostEnvironment _env;

    public UserController(
        IUserService userService,
        IRoleService roleService,
        IChartPost chartPost,
        IWebHostEnvironment env
    )
    {
        _userService = userService;
        _roleService = roleService;
        _chartPost = chartPost;
        _env = env;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<ActionResult> GetUsers(
        int page = 1,
        int pageSize = 5,
        string searchString = ""
    )
    {
        searchString ??= string.Empty;

        var users = await _userService.GetBySearch(
            page,
            pageSize,
            searchString
        );

        var totalCount = users.TotalCount;
        var numPages = (int)Math.Ceiling((float)totalCount / pageSize);

        var getList = from user in users.Records
            select new
            {
                profile = user.Profile,
                moblie = user.Mobile,
                userid = user.Id,
                rolename = user.Role.Name,
                name = user.Name,
                isactive = user.IsActive,
                email = user.Email,
                addedate = user.CreatedDate,
            };

        return Json(new { getList, totalCount, numPages });
    }

    public async Task<IActionResult> Create()
    {
        ViewData["RoleId"] = new SelectList(
            await _roleService.Roles(),
            "Id",
            "Name"
        );
        ViewData["chartPostId"] = new SelectList(
            await _chartPost.ChartPosts(),
            "Id",
            "PostDuty"
        );

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        User user,
        IFormFile file
    )
    {
        var fileName = "no-photo.png";
        if (ModelState.IsValid)
        {
            if (file != null)
            {
                var uploads = Path.Combine(
                    _env.WebRootPath,
                    "Upload\\Profile"
                );
                fileName = Guid.NewGuid().ToString().Replace(
                    "-",
                    ""
                ) + Path.GetExtension(file.FileName);

                await using (var fileStream = new FileStream(
                                 Path.Combine(
                                     uploads,
                                     fileName
                                 ), FileMode.Create
                             ))
                {
                    await file.CopyToAsync(fileStream);
                }

                var img = new ImageResizer(128);

                img.Resize(
                    Path.Combine(
                        uploads,
                        fileName
                    ), Path.Combine(
                        uploads,
                        "thumbnail",
                        fileName
                    )
                );
            }
        }

        user.Profile = fileName;
        await _userService.Add(user);
        ViewData["RoleId"] = new SelectList(
            await _roleService.Roles(),
            "Id",
            "Name",
            user.RoleId
        );

        ViewData["chartPostId"] = new SelectList(
            await _chartPost.ChartPosts(),
            "Id",
            "PostDuty",
            user.ChartPostId
        );

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var user = await _userService.GetById(id);
        if (user == null)
            return NotFound();

        ViewData["RoleId"] = new SelectList(
            await _roleService.Roles(),
            "Id",
            "Name",
            user.RoleId
        );

        ViewData["ChartPostId"] = new SelectList(
            await _chartPost.ChartPosts(),
            "Id",
            "PostDuty",
            user.ChartPostId
        );

        return View(user);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        User user,
        IFormFile file
    )
    {
        if (id != user.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                var fileName = "no-photo.png";
                if (file != null)
                {
                    var uploads = Path.Combine(
                        _env.WebRootPath,
                        "Upload\\Profile"
                    );
                    fileName = Guid.NewGuid().ToString().Replace(
                        "-",
                        ""
                    ) + Path.GetExtension(file.FileName);

                    await using (var fileStream = new FileStream(
                                     Path.Combine(
                                         uploads,
                                         fileName
                                     ), FileMode.Create
                                 ))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    var img = new ImageResizer(128);

                    img.Resize(
                        Path.Combine(
                            uploads,
                            fileName
                        ), Path.Combine(
                            uploads,
                            "thumbnail",
                            fileName
                        )
                    );
                }

                user.Profile = fileName;
                await _userService.Edit(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _userService.IsExist(user.Id) == null)
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["RoleId"] = new SelectList(
            await _roleService.Roles(),
            "Id",
            "Name",
            user.RoleId
        );

        ViewData["chartPostId"] = new SelectList(
            await _chartPost.ChartPosts(),
            "Id",
            "PostDuty",
            user.ChartPostId
        );
        return View(user);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var user = await _userService.GetById(id);
        await _userService.Remove(user);
        return RedirectToAction(nameof(Index));
    }

    public async Task<JsonResult> UniqueEmail(
        string email,
        int userId
    )
    {
        return Json(
            await _userService.UniqueEmail(
                email,
                userId
            ) == null
        );
    }

    public async Task<JsonResult> UniqueMobile(
        string mobile,
        int userId
    )
    {
        return Json(
            await _userService.UniqueMobile(
                mobile,
                userId
            ) == null
        );
    }

    public async Task<ActionResult> ChangePass(int? id)
    {
        var users = await _userService.GetById(id);
        var ch = new ChangePasswordViewModel
        {
            UserId = users.Id,
        };

        return Ok(ch);
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<ActionResult> ChangePassword(ChangePasswordViewModel chang)
    {
        var user = await _userService.GetUserByPassword(
            chang.UserId,
            chang.OldPass
        );

        if (!ModelState.IsValid)
            return NoContent();

        if (user != null)
        {
            await _userService.EditPassword(
                user,
                chang.Pass
            );

            user.Password = chang.Pass;
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError(
            "OldPass",
            "رمز را به درستی واردنمایید"
        );

        return Ok();
    }
}