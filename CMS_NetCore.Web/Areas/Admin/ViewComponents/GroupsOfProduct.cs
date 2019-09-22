using CMS_NetCore.Interfaces;
using CMS_NetCore.DomainClasses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS_NetCore.Web.ViewComponents
{
    [ViewComponent(Name = "GroupsOfProduct")]
    public class GroupsOfProduct : ViewComponent
    {
        private IProductGroupService _productGroupService ;

        public GroupsOfProduct(IProductGroupService productGroupService)
        {
            _productGroupService = productGroupService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var list = await ProductGroups();
            return View(list);
        }
        // GET: Admin/Partial
        public async Task<IEnumerable<ProductGroup>> ProductGroups()
        {
            return await _productGroupService.ProductGroups();
        }

    }
}
