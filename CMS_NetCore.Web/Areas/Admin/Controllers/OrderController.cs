using System;
using System.Linq;
using System.Threading.Tasks;
using CMS_NetCore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CMS_NetCore.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class OrderController : Controller
{
    private readonly IOrderService _orderService;
    private readonly IOrderDetailService _orderDetailService;

    public OrderController(
        IOrderService orderService,
        IOrderDetailService orderDetailService
    )
    {
        _orderService = orderService;
        _orderDetailService = orderDetailService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders(
        int page = 1,
        int pageSize = 5,
        string searchString = ""
    )
    {
        searchString ??= string.Empty;

        var orders = await _orderService.GetBySearch(
            page,
            pageSize,
            searchString
        );
        var totalCount = orders.TotalCount;
        var numPages = (int)Math.Ceiling((float)totalCount / pageSize);

        var getList = (from order in orders.Records
            select new
            {
                order.User.Name,
                order.IsFinally,
                order.CreatedDate,
                OrderId = order.Id,
            });

        return Json(new { getList, totalCount, numPages });
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var orderDetail = await _orderDetailService.GetById(id);
        if (orderDetail == null)
            return NotFound();

        return View(orderDetail);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var order = await _orderService.GetById(id);
        if (order == null)
            return NotFound();

        return View(order);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var order = await _orderService.GetById(id);
        await _orderService.Remove(order);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult AllDetails()
    {
        return View();
    }

    [HttpGet]
    public async Task<JsonResult> GetAllDetails(
        string searchString,
        int page = 1,
        int pageSize = 5
    )
    {
        searchString ??= string.Empty;

        var orderDetail = await _orderDetailService.GetBySearch(
            page,
            pageSize,
            searchString
        );

        var totalCount = orderDetail.TotalCount;
        var numPages = (int)Math.Ceiling((float)totalCount / pageSize);

        return Json(new { orderDetail.Records, totalCount, numPages });
    }
}