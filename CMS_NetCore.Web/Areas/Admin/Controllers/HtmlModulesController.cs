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
    public class HtmlModulesController : Controller
    {
        private readonly IModuleService _moduleService;
        private readonly IMenuService _menuService;
        private readonly IModulePageService _modulePageService;
        private readonly IHtmlModuleService _htmlModuleService;
        private readonly IPositionService _positionService;

        public HtmlModulesController(IModuleService moduleService, IHtmlModuleService htmlModuleService
            , IMenuService menuService, IModulePageService modulePageService,IPositionService positionService)
        {
            _moduleService = moduleService;
            _menuService = menuService;
            _modulePageService = modulePageService;
            _htmlModuleService = htmlModuleService;
            _positionService = positionService;
        }

        // GET: Admin/HtmlModules
        public IActionResult Index()
        {
            return RedirectToAction(nameof(Index), new { Controller = "Modules" });
        }


        // GET: Admin/HtmlModules/Create
        public async Task<IActionResult> Create(int? id)
        {
            ViewData["PositionId"] = new SelectList(await _positionService.GetAll(), "PositionId", "PositionTitle");
            return View();
        }

        // POST: Admin/HtmlModules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HtmlModulViewModel htmlModulViewModel)
        {
            if (ModelState.IsValid)
            {
                Module module = new Module()
                {
                    ModuleTitle = htmlModulViewModel.ModuleTitle,
                    PositionId = (int)htmlModulViewModel.PositionId,
                    IsActive = htmlModulViewModel.IsActive,
                    Accisibility = htmlModulViewModel.Accisibility,
                    ComponentId = int.Parse(Request.Form["componentId"])
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

                //for HtmlModule inserting
                module.HtmlModule = new HtmlModule()
                {
                    HtmlText = htmlModulViewModel.HtmlText,
                };

                //Add the Module
                await _moduleService.Add(module);
                return RedirectToAction(nameof(Index));
            }
            ViewData["PositionId"] = new SelectList(await _positionService.GetAll(), "PositionId", "PositionTitle");
            return View(htmlModulViewModel);
        }

        // GET: Admin/HtmlModules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Module module = await _moduleService.GetHtmlModuleById(id);
            if (module == null)
            {
                return NotFound();
            }
            HtmlModulViewModel htmlModulViewModel = new HtmlModulViewModel()
            {
                ModuleId = module.ModuleId,
                ModuleTitle = module.ModuleTitle,
                IsActive = module.IsActive,
                PositionId = module.PositionId,
                DisplayOrder = module.DisplayOrder,
                HtmlText = module.HtmlModule.HtmlText,
            };

            ViewData["PositionId"] = new SelectList(await _positionService.GetAll(), "PositionId", "PositionTitle");
            return View(htmlModulViewModel);
        }

        // POST: Admin/HtmlModules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(HtmlModulViewModel htmlModulViewModel, int pastDisOrder, int pastPosition)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Module module = await _moduleService.GetById(htmlModulViewModel.ModuleId);
                    if (module != null)
                    {
                        module.ModuleTitle = htmlModulViewModel.ModuleTitle;
                        module.PositionId = (int)htmlModulViewModel.PositionId;
                        module.IsActive = htmlModulViewModel.IsActive;
                        module.Accisibility = htmlModulViewModel.Accisibility;
                        module.DisplayOrder = htmlModulViewModel.DisplayOrder;

                        //Method for selecting menus for modules
                        List<ModulePage> modulePageAddList = new List<ModulePage>();
                        List<ModulePage> modulePageRemoveList = new List<ModulePage>();
                        foreach (var item in await _menuService.Menus())
                        {
                            if (Request.Form["Page[" + item.MenuId.ToString() + "]"].Any() && !await _modulePageService.ExistModulePage(htmlModulViewModel.ModuleId, item.MenuId))
                            {
                                ModulePage modulePage = new ModulePage()
                                {
                                    MenuId = item.MenuId,
                                    ModuleId = htmlModulViewModel.ModuleId,
                                };
                                modulePageAddList.Add(modulePage);
                            }
                            else if (!Request.Form["Page[" + item.MenuId.ToString() + "]"].Any() && await _modulePageService.ExistModulePage(htmlModulViewModel.ModuleId, item.MenuId))
                            {
                                ModulePage PageRemove = await _modulePageService.GetByMenuModule(htmlModulViewModel.ModuleId, item.MenuId);
                                modulePageRemoveList.Add(PageRemove);
                            }
                        }
                        await _modulePageService.Add(modulePageAddList);
                        await _modulePageService.Remove(modulePageRemoveList);

                        //editing HtmlModule
                        HtmlModule htmlModule = await _htmlModuleService.GetByModuleId(htmlModulViewModel.ModuleId);
                        if (htmlModule != null)
                        {
                            htmlModule.HtmlText = htmlModulViewModel.HtmlText;
                            await _htmlModuleService.Edit(htmlModule);
                        }

                        //Editing the Module
                        await _moduleService.Edit(module, pastPosition, pastDisOrder);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PositionId"] = new SelectList(await _positionService.GetAll(), "PositionId", "PositionTitle");
            return View(htmlModulViewModel);
        }
    }
}
