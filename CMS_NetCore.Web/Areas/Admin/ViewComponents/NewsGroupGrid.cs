using System.Collections.Generic;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CMS_NetCore.Web.Areas.Admin.ViewComponents;

[ViewComponent(Name = "NewsGroupGrid")]
public class NewsGroupGrid : ViewComponent
{
    private readonly INewsGroupService _newsGroupService;

    public NewsGroupGrid(INewsGroupService newsGroupService)
    {
        _newsGroupService = newsGroupService;
    }

    public async Task<IViewComponentResult> InvokeAsync(
        int depth,
        int parentId
    )
    {
        ViewBag.Depth = depth;
        ViewBag.ParentId = parentId;
        var list = await GetNewsGroup();
        return null;
    }

    // GET: Admin/Partial
    public async Task<IEnumerable<NewsGroup>> GetNewsGroup()
    {
        return await _newsGroupService.GetAll();
    }
}