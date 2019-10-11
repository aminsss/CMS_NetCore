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

namespace CMS_NetCore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MenuModulesController : Controller
    {
        private readonly IModuleService _moduleService;
        private readonly IMenuModuleService _menuModuleService;
        private readonly IMenuService _menuService;
        private readonly IMenuGroupService _menuGroupService;
        private readonly IModulePageService _modulePageService;
        private readonly IPositionService _positionService;

        public MenuModulesController(IModuleService moduleService, IMenuModuleService menuModuleService,IPositionService positionService
            , IMenuService menuService, IMenuGroupService menuGroupService, IModulePageService modulePageService)
        {
            _moduleService = moduleService;
            _menuService = menuService;
            _menuModuleService = menuModuleService;
            _menuGroupService = menuGroupService;
            _modulePageService = modulePageService;
            _positionService = positionService;
        }

        // GET: Admin/MenuModules
        public IActionResult Index()
        {
            return RedirectToAction(nameof(Index), new { Controller = "Modules" });
        }

        // GET: Admin/MenuModules/Create
        public async Task<IActionResult> Create()
        {
            ViewData["MenuGroupId"] = new SelectList(await _menuGroupService.MenuGroup() , "MenuGroupId", "MenuTitile");
            ViewData["PositionId"] = new SelectList(await _positionService.GetAll(), "PositionId", "PositionTitle");
            return View();
        }

        // POST: Admin/MenuModules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MenuModulViewModel menuModulViewModel)
        {
            if (ModelState.IsValid)
            {
                Module module = new Module()
                {
                    ModuleTitle = menuModulViewModel.ModuleTitle,
                    PositionId = (int)menuModulViewModel.PositionId,
                    IsActive = menuModulViewModel.IsActive,
                    Accisibility = menuModulViewModel.Accisibility,
                    ComponentId = 1,
                };


                //Method for selecting menus for modules
                foreach (var item in await _menuService.Menus())
                {
                    if (Request.Form["Page[" + item.MenuId.ToString() + "]"].Any())
                    {
                        ModulePage modulePage = new ModulePage()
                        {
                            MenuId = item.MenuId,
                        };
                        module.ModulePage.Add(modulePage);
                    }
                }

                //for menuModule inserting
                module.MenuModule = new MenuModule()
                {
                    MenuGroupId = menuModulViewModel.MenuGroupId,
                };

                //Add the Module
                await _moduleService.Add(module);
                return RedirectToAction(nameof(Index));
            }
            ViewData["MenuGroupId"] = new SelectList(await _menuGroupService.MenuGroup(), "MenuGroupId", "MenuTitile");
            ViewData["PositionId"] = new SelectList(await _positionService.GetAll(), "PositionId", "PositionTitle");
            return View(menuModulViewModel);
        }

        // GET: Admin/MenuModules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //GetById included MenuModule
            Module module = await _moduleService.GetMenuModuleById(id);
            if (module == null)
            {
                return NotFound();
            }
            MenuModulViewModel menuModulViewModel = new MenuModulViewModel
            {
                ModuleId = module.ModuleId,
                ModuleTitle = module.ModuleTitle,
                IsActive = module.IsActive,
                PositionId = module.PositionId,
                MenuGroupId = module.MenuModule.MenuGroupId,
                DisplayOrder = module.DisplayOrder
            };
            ViewData["MenuGroupId"] = new SelectList(await _menuGroupService.MenuGroup(), "MenuGroupId", "MenuTitile");
            ViewData["PositionId"] = new SelectList(await _positionService.GetAll(), "PositionId", "PositionTitle");
            return View(menuModulViewModel);
        }

        // POST: Admin/MenuModules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MenuModulViewModel menuModulViewModel, int pastDisOrder, int pastPosition)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Module module = await _moduleService.GetById(menuModulViewModel.ModuleId);
                    if (module != null)
                    {
                        module.ModuleTitle = menuModulViewModel.ModuleTitle;
                        module.PositionId = (int)menuModulViewModel.PositionId;
                        module.IsActive = menuModulViewModel.IsActive;
                        module.Accisibility = menuModulViewModel.Accisibility;
                        module.DisplayOrder = menuModulViewModel.DisplayOrder;


                        //Method for selecting menus for modules
                        List<ModulePage> modulePageAddList = new List<ModulePage>();
                        List<ModulePage> modulePageRemoveList = new List<ModulePage>();
                        foreach (var item in await _menuService.Menus())
                        {
                            //add menues that wasnt added before
                            if (Request.Form["Page[" + item.MenuId.ToString() + "]"].Any() && !await _modulePageService.ExistModulePage(menuModulViewModel.ModuleId, item.MenuId))
                            {
                                ModulePage modulePage = new ModulePage()
                                {
                                    MenuId = item.MenuId,
                                    ModuleId = menuModulViewModel.ModuleId,
                                };
                                modulePageAddList.Add(modulePage);
                            }
                            //remove menues that was added before
                            else if (!Request.Form["Page[" + item.MenuId.ToString() + "]"].Any() && await _modulePageService.ExistModulePage(menuModulViewModel.ModuleId, item.MenuId))
                            {
                                ModulePage PageRemove = await _modulePageService.GetByMenuModule(menuModulViewModel.ModuleId, item.MenuId);
                                modulePageRemoveList.Add(PageRemove);
                            }
                        }
                        await _modulePageService.Add(modulePageAddList);
                        await _modulePageService.Remove(modulePageRemoveList);

                        //editing MenuModule GroupId if it's changed
                        var EditMenuModule = await _menuModuleService.GetByModuleId(menuModulViewModel.ModuleId);
                        if (EditMenuModule.MenuGroupId != menuModulViewModel.MenuGroupId)
                        {
                            EditMenuModule.MenuGroupId = menuModulViewModel.MenuGroupId;
                            await _menuModuleService.Edit(EditMenuModule);
                        }

                        //Editing the Module
                        await _moduleService.Edit(module, pastPosition, pastDisOrder);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MenuGroupId"] = new SelectList(await _menuGroupService.MenuGroup(), "MenuGroupId", "MenuTitile");
            ViewData["PositionId"] = new SelectList(await _positionService.GetAll(), "PositionId", "PositionTitle");
            return View(menuModulViewModel);
        }

    }
}
