using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using CMS_NetCore.ViewModels;
using Microsoft.AspNetCore.Http;
using System.IO;
using CMS_NetCore.ServiceLayer;
using Microsoft.AspNetCore.Hosting;

namespace CMS_NetCore.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class MultiPictureModuleController : Controller
{
    private readonly IModuleService _moduleService;
    private readonly IMenuService _menuService;
    private readonly IModulePageService _modulePageService;
    private readonly IMultiPictureModuleService _multiPictureModuleService;
    private readonly IMultiPictureItemService _multiPictureItemService;
    private readonly IPositionService _positionService;
    private readonly IWebHostEnvironment _env;

    public MultiPictureModuleController(
        IModuleService moduleService,
        IWebHostEnvironment env,
        IMultiPictureModuleService multiPictureModuleService,
        IMultiPictureItemService multiPictureItemService,
        IMenuService menuService,
        IModulePageService modulePageService,
        IPositionService positionService
    )
    {
        _moduleService = moduleService;
        _menuService = menuService;
        _modulePageService = modulePageService;
        _multiPictureModuleService = multiPictureModuleService;
        _multiPictureItemService = multiPictureItemService;
        _positionService = positionService;
        _env = env;
    }

    public IActionResult Index()
    {
        return RedirectToAction(
            nameof(Index),
            new { Controller = "Module" }
        );
    }


    public async Task<IActionResult> Create(int? id)
    {
        ViewData["PositionId"] = new SelectList(
            await _positionService.GetAll(),
            "Id",
            "Title"
        );

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MultiPictureModuleViewModel multiPictureModuleVm)
    {
        if (ModelState.IsValid)
        {
            var module = new Module
            {
                Title = multiPictureModuleVm.ModuleTitle,
                PositionId = multiPictureModuleVm.PositionId!.Value,
                IsActive = multiPictureModuleVm.IsActive,
                Accessibility = multiPictureModuleVm.Accisibility,
                ComponentId = int.Parse(Request.Form["componentId"])
            };

            foreach (var item in await _menuService.Menus())
            {
                if (!Request.Form["Page[" + item.Id + "]"].Any())
                    continue;

                var modulePage = new ModulePage()
                {
                    MenuId = item.Id,
                };

                module.ModulePages.Add(modulePage);
            }

            module.MultiPictureModule = new MultiPictureModule()
            {
                Cover = multiPictureModuleVm.Cover,
                ModuleId = multiPictureModuleVm.ModuleId,
                Description = multiPictureModuleVm.Description,
                Title = multiPictureModuleVm.Title,
                TitleBold = multiPictureModuleVm.TitleBold,
                Link = multiPictureModuleVm.Link,
                LinkMore = multiPictureModuleVm.LinkMore,
                Image = multiPictureModuleVm.ModuleImage,
            };

            await _moduleService.Add(module);

            return RedirectToAction(nameof(Index));
        }

        ViewData["PositionId"] = new SelectList(
            await _positionService.GetAll(),
            "Id",
            "Title"
        );

        return View(multiPictureModuleVm);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var module = await _moduleService.GetHtmlModuleById(id);
        if (module == null)
            return NotFound();

        var multiPictureModuleVm = new MultiPictureModuleViewModel()
        {
            ModuleId = module.Id,
            ModuleTitle = module.Title,
            IsActive = module.IsActive,
            PositionId = module.PositionId,
            DisplayOrder = module.DisplayOrder,
            Title = module.MultiPictureModule.Title,
            TitleBold = module.MultiPictureModule.TitleBold,
            Link = module.MultiPictureModule.Link,
            LinkMore = module.MultiPictureModule.LinkMore,
            Cover = module.MultiPictureModule.Cover,
            Description = module.MultiPictureModule.Description,
            ModuleImage = module.MultiPictureModule.Image,
        };

        ViewData["PositionId"] = new SelectList(
            await _positionService.GetAll(),
            "Id",
            "Title"
        );

        return View(multiPictureModuleVm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        MultiPictureModuleViewModel multiPictureModuleVm,
        int pastDisOrder,
        int pastPosition
    )
    {
        if (ModelState.IsValid)
        {
            try
            {
                var module = await _moduleService.GetById(multiPictureModuleVm.ModuleId);
                if (module != null)
                {
                    module.Title = multiPictureModuleVm.ModuleTitle;
                    module.PositionId = multiPictureModuleVm.PositionId!.Value;
                    module.IsActive = multiPictureModuleVm.IsActive;
                    module.Accessibility = multiPictureModuleVm.Accisibility;
                    module.DisplayOrder = multiPictureModuleVm.DisplayOrder;

                    var modulePageAddList = new List<ModulePage>();
                    var modulePageRemoveList = new List<ModulePage>();

                    foreach (var item in await _menuService.Menus())
                    {
                        if (Request.Form["Page[" + item.Id + "]"].Any() &&
                            !await _modulePageService.IsExist(
                                multiPictureModuleVm.ModuleId,
                                item.Id
                            ))
                        {
                            var modulePage = new ModulePage()
                            {
                                MenuId = item.Id,
                                ModuleId = multiPictureModuleVm.ModuleId,
                            };

                            modulePageAddList.Add(modulePage);
                        }
                        else if (!Request.Form["Page[" + item.Id.ToString() + "]"].Any() &&
                                 await _modulePageService.IsExist(
                                     multiPictureModuleVm.ModuleId,
                                     item.Id
                                 ))
                        {
                            var pageRemove = await _modulePageService.GetByMenuModule(
                                multiPictureModuleVm.ModuleId,
                                item.Id
                            );

                            modulePageRemoveList.Add(pageRemove);
                        }
                    }

                    await _modulePageService.Add(modulePageAddList);
                    await _modulePageService.Remove(modulePageRemoveList);

                    var uploads = Path.Combine(
                        _env.WebRootPath,
                        "Upload\\ModuleImages"
                    );

                    var uploadsThumb = Path.Combine(
                        _env.WebRootPath,
                        "Upload\\ModuleImages\\thumbnail"
                    );

                    if (multiPictureModuleVm.ModuleImage != module.MultiPictureModule.Image &&
                        module.MultiPictureModule.Image != "no-photo.jpg")
                    {
                        if (System.IO.File.Exists(
                                Path.Combine(
                                    uploads,
                                    module.MultiPictureModule.Image
                                )
                            ))
                            System.IO.File.Delete(
                                Path.Combine(
                                    uploads,
                                    module.MultiPictureModule.Image
                                )
                            );
                        if (System.IO.File.Exists(
                                Path.Combine(
                                    uploadsThumb,
                                    module.MultiPictureModule.Image
                                )
                            ))
                            System.IO.File.Delete(
                                Path.Combine(
                                    uploadsThumb,
                                    module.MultiPictureModule.Image
                                )
                            );
                    }

                    var multiPictureModule =
                        await _multiPictureModuleService.GetByModuleId(multiPictureModuleVm.ModuleId);
                    if (multiPictureModule != null)
                    {
                        multiPictureModule.Title = multiPictureModuleVm.Title;
                        multiPictureModule.TitleBold = multiPictureModuleVm.TitleBold;
                        multiPictureModule.Description = multiPictureModuleVm.Description;
                        multiPictureModule.Cover = multiPictureModuleVm.Cover;
                        multiPictureModule.Link = multiPictureModuleVm.Link;
                        multiPictureModule.LinkMore = multiPictureModuleVm.LinkMore;
                        multiPictureModule.Image = multiPictureModuleVm.ModuleImage;
                        await _multiPictureModuleService.Edit(multiPictureModule);
                    }

                    await _moduleService.Edit(
                        module,
                        pastPosition,
                        pastDisOrder
                    );
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["PositionId"] = new SelectList(
            await _positionService.GetAll(),
            "Id",
            "Title"
        );

        return View(multiPictureModuleVm);
    }

    [HttpPost]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        if (file == null) return Json(new { status = "Error" });
        var uploads = Path.Combine(
            _env.WebRootPath,
            "Upload\\ModuleImages"
        );

        var fileName = Guid.NewGuid().ToString().Replace(
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

        //---------------------resize Images ----------------------
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

        return Json(
            new
            {
                status = "Done", src = Path.Combine(
                    "\\Upload\\ModuleImages",
                    fileName
                ),
                ImageName = fileName
            }
        );
    }

    public async Task<IActionResult> ItemsList(int? id)
    {
        if (id == null)
            return NotFound();

        var multiPictures = await _multiPictureItemService.GetMultiPictureItems(id!.Value);
        return PartialView(multiPictures);
    }

    public IActionResult CreateItems(int? moduleId)
    {
        if (moduleId == null)
            return NotFound();

        ViewBag.moduleId = moduleId;
        return PartialView();
    }

    [HttpPost]
    public async Task<JsonResult> CreateItems(MultiPictureItem multiPictureItem)
    {
        await _multiPictureItemService.Add(multiPictureItem);
        return Json(true);
    }

    public async Task<IActionResult> EditItems(int? multiPictureItemsId)
    {
        var multiPictureItem = await _multiPictureItemService.GetItemsById(multiPictureItemsId);
        return PartialView(multiPictureItem);
    }

    [HttpPost]
    public async Task<JsonResult> EditItems(MultiPictureItem multiPictureItem)
    {
        await _multiPictureItemService.Edit(multiPictureItem);
        return Json(true);
    }

    [HttpGet]
    public async Task<ActionResult> UploadImage(int multiPictureItemsId)
    {
        var multiPictureItem = await _multiPictureItemService.GetItemsById(multiPictureItemsId);
        return PartialView(multiPictureItem);
    }

    [HttpPost]
    public async Task<JsonResult> UploadImage(
        int id,
        string imageName
    )
    {
        var uploads = Path.Combine(
            _env.WebRootPath,
            "Upload\\ModuleImages"
        );
        var uploadsThumb = Path.Combine(
            _env.WebRootPath,
            "Upload\\ModuleImages\\thumbnail"
        );
        var multiPictureItem = await _multiPictureItemService.GetItemsById(id);
        if (multiPictureItem.Image != imageName && multiPictureItem.Image != "no-photo.jpg" &&
            multiPictureItem.Image != null)
        {
            if (System.IO.File.Exists(
                    Path.Combine(
                        uploads,
                        multiPictureItem.Image
                    )
                ))
                System.IO.File.Delete(
                    Path.Combine(
                        uploads,
                        multiPictureItem.Image
                    )
                );
            if (System.IO.File.Exists(
                    Path.Combine(
                        uploadsThumb,
                        multiPictureItem.Image
                    )
                ))
                System.IO.File.Delete(
                    Path.Combine(
                        uploadsThumb,
                        multiPictureItem.Image
                    )
                );
        }

        multiPictureItem.Image = imageName;
        await _multiPictureItemService.Edit(multiPictureItem);
        return Json(true);
    }


    public async Task<JsonResult> DeleteItems(int id)
    {
        MultiPictureItem multiPictureItem = await _multiPictureItemService.GetItemsById(id);
        await _multiPictureItemService.Remove(multiPictureItem);
        var uploads = Path.Combine(
            _env.WebRootPath,
            "Upload\\ModuleImages"
        );
        var uploadsThumb = Path.Combine(
            _env.WebRootPath,
            "Upload\\ModuleImages\\thumbnail"
        );
        if (multiPictureItem.Image != "no-photo.jpg")
        {
            if (System.IO.File.Exists(
                    Path.Combine(
                        uploads,
                        multiPictureItem.Image
                    )
                ))
                System.IO.File.Delete(
                    Path.Combine(
                        uploads,
                        multiPictureItem.Image
                    )
                );
            if (System.IO.File.Exists(
                    Path.Combine(
                        uploadsThumb,
                        multiPictureItem.Image
                    )
                ))
                System.IO.File.Delete(
                    Path.Combine(
                        uploadsThumb,
                        multiPictureItem.Image
                    )
                );
        }

        return Json(true);
    }
}