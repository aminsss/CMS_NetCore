using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;


namespace CMS_NetCore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ModulesController : Controller
    {
        private IModuleService _moduleService;
        private IComponentService _componentService;
        private IMenuGroupService _menuGroupService;

        public ModulesController(IModuleService moduleService, IComponentService componentService, IMenuGroupService menuGroupService)
        {
            _moduleService = moduleService;
            _componentService = componentService;
            _menuGroupService = menuGroupService;
        }

        // GET: Admin/Modules
        public async Task<IActionResult> Index(string searchString = "")
        {
            var modules = await _moduleService.GetBySearch(searchString);
            return View(modules.Records);
        }

        

        // GET: Admin/Modules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var module = await _moduleService.GetById(id);

            if (module == null)
            {
                return NotFound();
            }

            return View(module);
        }

        // POST: Admin/Modules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var module = await _moduleService.GetById(id);
            await _moduleService.Remove(module);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ComponentList()
        {
            return PartialView(await _componentService.GetAll());
        }


        public async Task<IActionResult> ModuleMenuShow(int? moduleId)
        {
            ViewBag.ModuleId = moduleId;
            return PartialView(await _menuGroupService.MenuGroup());
        }
    }
}
