using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using CMS_NetCore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CMS_NetCore.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class MenuModuleController : Controller
{
    private readonly IModuleService _moduleService;
    private readonly IMenuModuleService _menuModuleService;
    private readonly IMenuService _menuService;
    private readonly IMenuGroupService _menuGroupService;
    private readonly IModulePageService _modulePageService;
    private readonly IPositionService _positionService;

    public MenuModuleController(
        IModuleService moduleService,
        IMenuModuleService menuModuleService,
        IPositionService positionService,
        IMenuService menuService,
        IMenuGroupService menuGroupService,
        IModulePageService modulePageService
    )
    {
        _moduleService = moduleService;
        _menuService = menuService;
        _menuModuleService = menuModuleService;
        _menuGroupService = menuGroupService;
        _modulePageService = modulePageService;
        _positionService = positionService;
    }

    public IActionResult Index()
    {
        return RedirectToAction(
            nameof(Index),
            new { Controller = "Module" }
        );
    }

    public async Task<IActionResult> Create()
    {
        ViewData["MenuGroupId"] = new SelectList(
            await _menuGroupService.MenuGroup(),
            "MenuGroupId",
            "Title"
        );

        ViewData["PositionId"] = new SelectList(
            await _positionService.GetAll(),
            "Id",
            "Title"
        );

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MenuModuleViewModel menuModuleViewModel)
    {
        if (ModelState.IsValid)
        {
            var module = new Module
            {
                Title = menuModuleViewModel.ModuleTitle,
                PositionId = menuModuleViewModel.PositionId!.Value,
                IsActive = menuModuleViewModel.IsActive,
                Accessibility = menuModuleViewModel.Accisibility,
                ComponentId = 1,
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

            module.MenuModule = new MenuModule()
            {
                MenuGroupId = menuModuleViewModel.MenuGroupId,
            };

            await _moduleService.Add(module);

            return RedirectToAction(nameof(Index));
        }

        ViewData["MenuGroupId"] = new SelectList(
            await _menuGroupService.MenuGroup(),
            "Id",
            "Title"
        );

        ViewData["PositionId"] = new SelectList(
            await _positionService.GetAll(),
            "Id",
            "Title"
        );

        return View(menuModuleViewModel);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var module = await _moduleService.GetMenuModuleById(id);

        if (module == null)
            return NotFound();

        var menuModuleViewModel = new MenuModuleViewModel
        {
            ModuleId = module.Id,
            ModuleTitle = module.Title,
            IsActive = module.IsActive,
            PositionId = module.PositionId,
            MenuGroupId = module.MenuModule.MenuGroupId,
            DisplayOrder = module.DisplayOrder
        };

        ViewData["MenuGroupId"] = new SelectList(
            await _menuGroupService.MenuGroup(),
            "Id",
            "Title"
        );

        ViewData["PositionId"] = new SelectList(
            await _positionService.GetAll(),
            "Id",
            "Title"
        );

        return View(menuModuleViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        MenuModuleViewModel menuModuleViewModel,
        int pastDisOrder,
        int pastPosition
    )
    {
        if (ModelState.IsValid)
        {
            try
            {
                var module = await _moduleService.GetById(menuModuleViewModel.ModuleId);

                if (module != null)
                {
                    module.Title = menuModuleViewModel.ModuleTitle;
                    module.PositionId = menuModuleViewModel.PositionId!.Value;
                    module.IsActive = menuModuleViewModel.IsActive;
                    module.Accessibility = menuModuleViewModel.Accisibility;
                    module.DisplayOrder = menuModuleViewModel.DisplayOrder;

                    var modulePageAddList = new List<ModulePage>();
                    var modulePageRemoveList = new List<ModulePage>();

                    foreach (var item in await _menuService.Menus())
                    {
                        if (Request.Form["Page[" + item.Id + "]"].Any() &&
                            !await _modulePageService.IsExist(
                                menuModuleViewModel.ModuleId,
                                item.Id
                            ))
                        {
                            var modulePage = new ModulePage()
                            {
                                MenuId = item.Id,
                                ModuleId = menuModuleViewModel.ModuleId,
                            };

                            modulePageAddList.Add(modulePage);
                        }
                        else if (!Request.Form["Page[" + item.Id + "]"].Any() &&
                                 await _modulePageService.IsExist(
                                     menuModuleViewModel.ModuleId,
                                     item.Id
                                 ))
                        {
                            var pageRemove = await _modulePageService.GetByMenuModule(
                                menuModuleViewModel.ModuleId,
                                item.Id
                            );
                            modulePageRemoveList.Add(pageRemove);
                        }
                    }

                    await _modulePageService.Add(modulePageAddList);
                    await _modulePageService.Remove(modulePageRemoveList);

                    var editMenuModule = await _menuModuleService.GetByModuleId(menuModuleViewModel.ModuleId);

                    if (editMenuModule.MenuGroupId != menuModuleViewModel.MenuGroupId)
                    {
                        editMenuModule.MenuGroupId = menuModuleViewModel.MenuGroupId;
                        await _menuModuleService.Edit(editMenuModule);
                    }

                    await _moduleService.Edit(
                        module,
                        pastPosition,
                        pastDisOrder
                    );
                }
            }
            catch
            {
                //ignore
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["MenuGroupId"] = new SelectList(
            await _menuGroupService.MenuGroup(),
            "Id",
            "Title"
        );

        ViewData["PositionId"] = new SelectList(
            await _positionService.GetAll(),
            "Id",
            "Title"
        );

        return View(menuModuleViewModel);
    }
}