using System.Collections.Generic;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CMS_NetCore.Web.Areas.Admin.ViewComponents;

[ViewComponent(Name = "NewsGroupList")]
public class NewsGroupList : ViewComponent
{
    private readonly INewsGroupService _newsGroupService;

    public NewsGroupList(INewsGroupService newsGroupService)
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
        return View(list);
    }

    // GET: Admin/Partial
    public async Task<IEnumerable<NewsGroup>> GetNewsGroup()
    {
        return await _newsGroupService.GetAll();
    }
}