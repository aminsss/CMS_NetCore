using System.Collections.Generic;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CMS_NetCore.Web.Areas.Admin.ViewComponents;

[ViewComponent(Name = "ProductsGroupGrid")]
public class ProductGroupGrid : ViewComponent
{
    private readonly IProductGroupService _productGroupService;

    public ProductGroupGrid(IProductGroupService productGroupService)
    {
        _productGroupService = productGroupService;
    }

    public async Task<IViewComponentResult> InvokeAsync(
        int depth,
        int parentId
    )
    {
        ViewBag.Depth = depth;
        ViewBag.ParentId = parentId;
        var list = await ProductGroups();
        return null;
    }

    // GET: Admin/Partial
    public async Task<IEnumerable<ProductGroup>> ProductGroups()
    {
        return await _productGroupService.ProductGroups();
    }
}