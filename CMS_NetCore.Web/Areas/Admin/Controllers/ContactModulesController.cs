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
    public class ContactModulesController : Controller
    {
        private readonly IModuleService _moduleService;
        private readonly IMenuService _menuService;
        private readonly IModulePageService _modulePageService;
        private readonly IContactModuleService _contactModuleService;
        private readonly IContactPersonService _contactPersonService;
        private readonly IUserService _userService;
        private readonly IPositionService _positionService;
        

        public ContactModulesController(IModuleService moduleService, IContactModuleService contactModuleService,IPositionService positionService
                , IMenuService menuService, IModulePageService modulePageService, IContactPersonService contactPersonService, IUserService userService)
        {
            _moduleService = moduleService;
            _menuService = menuService;
            _modulePageService = modulePageService;
            _contactModuleService = contactModuleService;
            _userService = userService;
            _contactPersonService = contactPersonService;
            _positionService = positionService;
        }

        // GET: Admin/ContactModules
        public IActionResult Index()
        {
            return RedirectToAction(nameof(Index), new { Controller = "Modules" });

        }

        // GET: Admin/ContactModules/Create
        public async Task<IActionResult> Create()
        {
            ViewData["PositionId"] = new SelectList(await _positionService.GetAll(), "PositionId", "PositionTitle");
            return View();
        }

        // POST: Admin/ContactModules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContactModuleViewModel contactModuleViewModel)
        {
            if (ModelState.IsValid)
            {
                Module module = new Module()
                {
                    ModuleTitle = contactModuleViewModel.ModuleTitle,
                    PositionId = (int)contactModuleViewModel.PositionId,
                    IsActive = contactModuleViewModel.IsActive,
                    Accisibility = contactModuleViewModel.Accisibility,
                    ComponentId = 4
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

                //for ContactModule inserting
                module.ContactModule = new ContactModule()
                {
                    Email = contactModuleViewModel.Email,
                    PostCode = contactModuleViewModel.PostCode,
                    PhoneNum = contactModuleViewModel.PhoneNum,
                    MobileNum = contactModuleViewModel.MobileNum,
                    Description = contactModuleViewModel.Description,
                    Address = contactModuleViewModel.Address,
                };

                //for Users included contactModule inserting
                List<ContactPerson> contactPeople = new List<ContactPerson>();
                foreach (var item in await _userService.GetAllAdmin())
                {
                    if (Request.Form["User[" + item.UserId.ToString() + "]"].Any())
                    {
                        ContactPerson contactPerson = new ContactPerson()
                        {
                            UserId = item.UserId,
                        };
                        module.ContactModule.ContactPerson.Add(contactPerson);
                    }
                }

                //Add the Module
                await _moduleService.Add(module);
                return RedirectToAction(nameof(Index));
            }
            ViewData["PositionId"] = new SelectList(await _positionService.GetAll(), "PositionId", "PositionTitle");
            return View(contactModuleViewModel);
        }

        // GET: Admin/ContactModules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Module module = await _moduleService.GetContactModuleById(id);
            if (module == null)
            {
                return NotFound();
            }
            ContactModuleViewModel contactModuleViewModel = new ContactModuleViewModel()
            {
                ModuleId = module.ModuleId,
                ModuleTitle = module.ModuleTitle,
                IsActive = module.IsActive,
                PositionId = module.PositionId,
                DisplayOrder = module.DisplayOrder,
                Address = module.ContactModule.Address,
                Email = module.ContactModule.Email,
                MobileNum = module.ContactModule.MobileNum,
                PhoneNum = module.ContactModule.PhoneNum,
                PostCode = module.ContactModule.PostCode,
                Description = module.ContactModule.Description
            };
            ViewData["PositionId"] = new SelectList(await _positionService.GetAll(), "PositionId", "PositionTitle");
            return View(contactModuleViewModel);
        }

        // POST: Admin/ContactModules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ContactModuleViewModel contactModuleViewModel, int pastDisOrder, int pastPosition)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Module module = await _moduleService.GetById(contactModuleViewModel.ModuleId);
                    if (module != null)
                    {
                        module.ModuleTitle = contactModuleViewModel.ModuleTitle;
                        module.PositionId = (int)contactModuleViewModel.PositionId;
                        module.IsActive = contactModuleViewModel.IsActive;
                        module.Accisibility = contactModuleViewModel.Accisibility;
                        module.DisplayOrder = contactModuleViewModel.DisplayOrder;

                        //Method for selecting menus for modules
                        List<ModulePage> modulePageAddList = new List<ModulePage>();
                        List<ModulePage> modulePageRemoveList = new List<ModulePage>();
                        foreach (var item in await _menuService.Menus())
                        {
                            if (Request.Form["Page[" + item.MenuId.ToString() + "]"].Any() && !await _modulePageService.ExistModulePage(contactModuleViewModel.ModuleId, item.MenuId))
                            {
                                ModulePage modulePage = new ModulePage()
                                {
                                    MenuId = item.MenuId,
                                    ModuleId = contactModuleViewModel.ModuleId,
                                };
                                modulePageAddList.Add(modulePage);
                            }
                            else if (!Request.Form["Page[" + item.MenuId.ToString() + "]"].Any() && await _modulePageService.ExistModulePage(contactModuleViewModel.ModuleId, item.MenuId))
                            {
                                ModulePage PageRemove = await _modulePageService.GetByMenuModule(contactModuleViewModel.ModuleId, item.MenuId);
                                modulePageRemoveList.Add(PageRemove);
                            }
                        }
                        await _modulePageService.Add(modulePageAddList);
                        await _modulePageService.Remove(modulePageRemoveList);

                        //editing HtmlModule
                        ContactModule contactModule = await _contactModuleService.GetByModuleId(contactModuleViewModel.ModuleId);
                        if (contactModule != null)
                        {
                            contactModule.Email = contactModuleViewModel.Email;
                            contactModule.PostCode = contactModuleViewModel.PostCode;
                            contactModule.PhoneNum = contactModuleViewModel.PhoneNum;
                            contactModule.MobileNum = contactModuleViewModel.MobileNum;
                            contactModule.Description = contactModuleViewModel.Description;
                            contactModule.Address = contactModuleViewModel.Address;
                            await _contactModuleService.Edit(contactModule);
                        }

                        //for Users Editing contactModule updating
                        List<ContactPerson> contactPeopleAddList = new List<ContactPerson>();
                        List<ContactPerson> contactPeopleRemoveList = new List<ContactPerson>();
                        foreach (var item in await _userService.GetAllAdmin())
                        {
                            if (Request.Form["User[" + item.UserId.ToString() + "]"].Any() && !await _contactPersonService.ExistContactPerson(contactModuleViewModel.ModuleId, item.UserId))
                            {
                                ContactPerson contactPerson = new ContactPerson()
                                {
                                    UserId = item.UserId,
                                    ContactModuleId = contactModuleViewModel.ModuleId,
                                };
                                contactPeopleAddList.Add(contactPerson);
                            }
                            else if (!Request.Form["User[" + item.UserId.ToString() + "]"].Any() && await _contactPersonService.ExistContactPerson(contactModuleViewModel.ModuleId, item.UserId))
                            {
                                ContactPerson contactRemove = await _contactPersonService.GetByModuleUser(contactModuleViewModel.ModuleId, item.UserId);
                                contactPeopleRemoveList.Add(contactRemove);
                            }
                        }
                        await _contactPersonService.Add(contactPeopleAddList);
                        await _contactPersonService.Remove(contactPeopleRemoveList);


                        //Editing the Module
                        await _moduleService.Edit(module, pastPosition, pastDisOrder);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PositionId"] = new SelectList(await _positionService.GetAll(), "PositionId", "PositionTitle");
            return View(contactModuleViewModel);
        }

       


    }
}
