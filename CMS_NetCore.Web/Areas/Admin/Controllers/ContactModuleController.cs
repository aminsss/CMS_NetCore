using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using CMS_NetCore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class ContactModuleController : Controller
{
    private readonly IModuleService _moduleService;
    private readonly IMenuService _menuService;
    private readonly IModulePageService _modulePageService;
    private readonly IContactModuleService _contactModuleService;
    private readonly IContactPersonService _contactPersonService;
    private readonly IUserService _userService;
    private readonly IPositionService _positionService;


    public ContactModuleController(
        IModuleService moduleService,
        IContactModuleService contactModuleService,
        IPositionService positionService,
        IMenuService menuService,
        IModulePageService modulePageService,
        IContactPersonService contactPersonService,
        IUserService userService
    )
    {
        _moduleService = moduleService;
        _menuService = menuService;
        _modulePageService = modulePageService;
        _contactModuleService = contactModuleService;
        _userService = userService;
        _contactPersonService = contactPersonService;
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
        ViewData["PositionId"] = new SelectList(
            await _positionService.GetAll(),
            "Id",
            "Title"
        );
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ContactModuleViewModel contactModuleViewModel)
    {
        if (ModelState.IsValid)
        {
            var module = new Module()
            {
                Title = contactModuleViewModel.ModuleTitle,
                PositionId = contactModuleViewModel.PositionId!.Value,
                IsActive = contactModuleViewModel.IsActive,
                Accessibility = contactModuleViewModel.Accisibility,
                ComponentId = 4
            };

            foreach (var item in await _menuService.Menus())
            {
                if (!Request.Form["Page[" + item.Id + "]"].Any()) continue;
                var modulePage = new ModulePage()
                {
                    MenuId = item.Id,
                };

                module.ModulePages.Add(modulePage);
            }

            module.ContactModule = new ContactModule()
            {
                Email = contactModuleViewModel.Email,
                PostCode = contactModuleViewModel.PostCode,
                PhoneNo = contactModuleViewModel.PhoneNum,
                MobileNo = contactModuleViewModel.MobileNum,
                Description = contactModuleViewModel.Description,
                Address = contactModuleViewModel.Address,
            };

            foreach (var item in await _userService.GetAllAdmin())
            {
                if (!Request.Form["User[" + item.Id + "]"].Any()) continue;
                var contactPerson = new ContactPerson()
                {
                    UserId = item.Id,
                };

                module.ContactModule.ContactPersons.Add(contactPerson);
            }

            await _moduleService.Add(module);
            return RedirectToAction(nameof(Index));
        }

        ViewData["PositionId"] = new SelectList(
            await _positionService.GetAll(),
            "Id",
            "Title"
        );
        return View(contactModuleViewModel);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var module = await _moduleService.GetContactModuleById(id);
        if (module == null)
            return NotFound();

        var contactModuleViewModel = new ContactModuleViewModel()
        {
            ModuleId = module.Id,
            ModuleTitle = module.Title,
            IsActive = module.IsActive,
            PositionId = module.PositionId,
            DisplayOrder = module.DisplayOrder,
            Address = module.ContactModule.Address,
            Email = module.ContactModule.Email,
            MobileNum = module.ContactModule.MobileNo,
            PhoneNum = module.ContactModule.PhoneNo,
            PostCode = module.ContactModule.PostCode,
            Description = module.ContactModule.Description
        };

        ViewData["PositionId"] = new SelectList(
            await _positionService.GetAll(),
            "Id",
            "Title"
        );

        return View(contactModuleViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        ContactModuleViewModel contactModuleViewModel,
        int pastDisOrder,
        int pastPosition
    )
    {
        if (ModelState.IsValid)
        {
            try
            {
                var module = await _moduleService.GetById(contactModuleViewModel.ModuleId);

                if (module != null)
                {
                    module.Title = contactModuleViewModel.ModuleTitle;
                    module.PositionId = contactModuleViewModel.PositionId!.Value;
                    module.IsActive = contactModuleViewModel.IsActive;
                    module.Accessibility = contactModuleViewModel.Accisibility;
                    module.DisplayOrder = contactModuleViewModel.DisplayOrder;

                    var modulePageAddList = new List<ModulePage>();
                    var modulePageRemoveList = new List<ModulePage>();

                    foreach (var item in await _menuService.Menus())
                    {
                        if (Request.Form["Page[" + item.Id + "]"].Any() &&
                            !await _modulePageService.IsExist(
                                contactModuleViewModel.ModuleId,
                                item.Id
                            ))
                        {
                            var modulePage = new ModulePage()
                            {
                                MenuId = item.Id,
                                ModuleId = contactModuleViewModel.ModuleId,
                            };

                            modulePageAddList.Add(modulePage);
                        }
                        else if (!Request.Form["Page[" + item.Id + "]"].Any() &&
                                 await _modulePageService.IsExist(
                                     contactModuleViewModel.ModuleId,
                                     item.Id
                                 ))
                        {
                            var pageRemove = await _modulePageService.GetByMenuModule(
                                contactModuleViewModel.ModuleId,
                                item.Id
                            );

                            modulePageRemoveList.Add(pageRemove);
                        }
                    }

                    await _modulePageService.Add(modulePageAddList);
                    await _modulePageService.Remove(modulePageRemoveList);

                    //editing HtmlModule
                    var contactModule =
                        await _contactModuleService.GetByModuleId(contactModuleViewModel.ModuleId);

                    if (contactModule != null)
                    {
                        contactModule.Email = contactModuleViewModel.Email;
                        contactModule.PostCode = contactModuleViewModel.PostCode;
                        contactModule.PhoneNo = contactModuleViewModel.PhoneNum;
                        contactModule.MobileNo = contactModuleViewModel.MobileNum;
                        contactModule.Description = contactModuleViewModel.Description;
                        contactModule.Address = contactModuleViewModel.Address;
                        await _contactModuleService.Edit(contactModule);
                    }

                    var contactPeopleAddList = new List<ContactPerson>();
                    var contactPeopleRemoveList = new List<ContactPerson>();

                    foreach (var item in await _userService.GetAllAdmin())
                    {
                        if (Request.Form["User[" + item.Id + "]"].Any() &&
                            !await _contactPersonService.IsExist(
                                contactModuleViewModel.ModuleId,
                                item.Id
                            ))
                        {
                            var contactPerson = new ContactPerson()
                            {
                                UserId = item.Id,
                                ContactModuleId = contactModuleViewModel.ModuleId,
                            };

                            contactPeopleAddList.Add(contactPerson);
                        }
                        else if (!Request.Form["User[" + item.Id + "]"].Any() &&
                                 await _contactPersonService.IsExist(
                                     contactModuleViewModel.ModuleId,
                                     item.Id
                                 ))
                        {
                            var contactRemove = await _contactPersonService.GetByModuleUser(
                                contactModuleViewModel.ModuleId,
                                item.Id
                            );

                            contactPeopleRemoveList.Add(contactRemove);
                        }
                    }

                    await _contactPersonService.Add(contactPeopleAddList);
                    await _contactPersonService.Remove(contactPeopleRemoveList);

                    await _moduleService.Edit(
                        module,
                        pastPosition,
                        pastDisOrder
                    );
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["PositionId"] = new SelectList(
            await _positionService.GetAll(),
            "Id",
            "Title"
        );

        return View(contactModuleViewModel);
    }
}