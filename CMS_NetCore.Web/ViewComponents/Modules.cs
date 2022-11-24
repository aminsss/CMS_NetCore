using CMS_NetCore.Interfaces;
using CMS_NetCore.DomainClasses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS_NetCore.Web.ViewComponents
{
    [ViewComponent(Name = "Modules")]
    public class Modules : ViewComponent
    {
        private IModuleService _moduleService;
        //private IMultiPictureModuleService _multiPictureModuleService;

        public Modules(IModuleService moduleService)
        {
            _moduleService = moduleService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id, string moduleName ,string moduleType)
        {
            var list = await GetModule(id, moduleType);
            return View(moduleName, list);
        }
        // GET: Admin/Partial
        public async Task<Module> GetModule(int id,string moduleType)
        {
            return moduleType switch
            {
                "1" => await _moduleService.GetById(id),
                _ => null
            };
        }
    }
}
