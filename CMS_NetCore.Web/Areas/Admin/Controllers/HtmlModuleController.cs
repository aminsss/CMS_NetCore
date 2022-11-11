using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using CMS_NetCore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HtmlModuleController : Controller
    {
        private readonly IModuleService _moduleService;
        private readonly IMenuService _menuService;
        private readonly IModulePageService _modulePageService;
        private readonly IHtmlModuleService _htmlModuleService;
        private readonly IPositionService _positionService;

        public HtmlModuleController(
            IModuleService moduleService,
            IHtmlModuleService htmlModuleService,
            IMenuService menuService,
            IModulePageService modulePageService,
            IPositionService positionService
        )
        {
            _moduleService = moduleService;
            _menuService = menuService;
            _modulePageService = modulePageService;
            _htmlModuleService = htmlModuleService;
            _positionService = positionService;
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
        public async Task<IActionResult> Create(HtmlModuleViewModel htmlModuleViewModel)
        {
            if (ModelState.IsValid)
            {
                var module = new Module
                {
                    Title = htmlModuleViewModel.ModuleTitle,
                    PositionId = htmlModuleViewModel.PositionId!.Value,
                    IsActive = htmlModuleViewModel.IsActive,
                    Accessibility = htmlModuleViewModel.Accisibility,
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

                module.HtmlModule = new HtmlModule()
                {
                    HtmlText = htmlModuleViewModel.HtmlText,
                };

                await _moduleService.Add(module);
                return RedirectToAction(nameof(Index));
            }

            ViewData["PositionId"] = new SelectList(
                await _positionService.GetAll(),
                "Id",
                "Title"
            );

            return View(htmlModuleViewModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var module = await _moduleService.GetHtmlModuleById(id);

            if (module == null)
                return NotFound();

            var htmlModuleViewModel = new HtmlModuleViewModel()
            {
                ModuleId = module.Id,
                ModuleTitle = module.Title,
                IsActive = module.IsActive,
                PositionId = module.PositionId,
                DisplayOrder = module.DisplayOrder,
                HtmlText = module.HtmlModule.HtmlText,
            };

            ViewData["PositionId"] = new SelectList(
                await _positionService.GetAll(),
                "Id",
                "Title"
            );

            return View(htmlModuleViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            HtmlModuleViewModel htmlModuleViewModel,
            int pastDisOrder,
            int pastPosition
        )
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Module module = await _moduleService.GetById(htmlModuleViewModel.ModuleId);
                    if (module != null)
                    {
                        module.Title = htmlModuleViewModel.ModuleTitle;
                        module.PositionId = htmlModuleViewModel.PositionId!.Value;
                        module.IsActive = htmlModuleViewModel.IsActive;
                        module.Accessibility = htmlModuleViewModel.Accisibility;
                        module.DisplayOrder = htmlModuleViewModel.DisplayOrder;

                        var modulePageAddList = new List<ModulePage>();
                        var modulePageRemoveList = new List<ModulePage>();

                        foreach (var item in await _menuService.Menus())
                        {
                            if (Request.Form["Page[" + item.Id.ToString() + "]"].Any() &&
                                !await _modulePageService.IsExist(
                                    htmlModuleViewModel.ModuleId,
                                    item.Id
                                ))
                            {
                                var modulePage = new ModulePage()
                                {
                                    MenuId = item.Id,
                                    ModuleId = htmlModuleViewModel.ModuleId,
                                };

                                modulePageAddList.Add(modulePage);
                            }
                            else if (!Request.Form["Page[" + item.Id + "]"].Any() &&
                                     await _modulePageService.IsExist(
                                         htmlModuleViewModel.ModuleId,
                                         item.Id
                                     ))
                            {
                                var pageRemove = await _modulePageService.GetByMenuModule(
                                    htmlModuleViewModel.ModuleId,
                                    item.Id
                                );

                                modulePageRemoveList.Add(pageRemove);
                            }
                        }

                        await _modulePageService.Add(modulePageAddList);
                        await _modulePageService.Remove(modulePageRemoveList);

                        var htmlModule = await _htmlModuleService.GetByModuleId(htmlModuleViewModel.ModuleId);

                        if (htmlModule != null)
                        {
                            htmlModule.HtmlText = htmlModuleViewModel.HtmlText;
                            await _htmlModuleService.Edit(htmlModule);
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

            return View(htmlModuleViewModel);
        }
    }
}