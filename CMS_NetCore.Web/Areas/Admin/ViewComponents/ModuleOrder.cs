using CMS_NetCore.Interfaces;
using CMS_NetCore.DomainClasses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS_NetCore.Web.ViewComponents
{
    [ViewComponent(Name = "ModuleOrder")]
    public class ModuleOrder : ViewComponent
    {
        private IModuleService _moduleService;

        public ModuleOrder(IModuleService moduleService)
        {
            _moduleService = moduleService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int positionId)
        {
            var list = await GetModuleOrder(positionId);
            return View(list);
        }
        // GET: Admin/Partial
        public async Task<IEnumerable<Module>> GetModuleOrder(int id) =>
             await _moduleService.GetByPositionId(id);
    }
}
