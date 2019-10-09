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
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IUserService _userService;

        public OrdersController(IOrderService orderService,IOrderDetailService orderDetailService, IUserService userService)
        {
            _orderService = orderService;
            _orderDetailService = orderDetailService;
            _userService = userService;
        }

        // GET: Admin/Orders
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders(int page = 1, int pageSize = 5, string searchString = "")
        {
            searchString = searchString ?? string.Empty;

            var list = await _orderService.GetBySearch(page, pageSize, searchString);
            int totalCount = list.TotalCount;
            int numPages = (int)Math.Ceiling((float)totalCount / pageSize);

            var getList = (from obj in list.Records
                           select new
                           {
                               Name = obj.User.Name,
                               IsFinally = obj.IsFinally,
                               AddedDate = obj.AddedDate,
                               OrderId = obj.OrderId,
                           });

            return Json(new { getList, totalCount, numPages });
        }

        // GET: Admin/Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetail = await _orderDetailService.GetById(id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // GET: Admin/Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _orderService.GetById(id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Admin/Orders/Delete/5
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
        public async Task<JsonResult> GetAllDetails( string searchString,int page = 1, int pageSize = 5)
        { 
            searchString = searchString ?? string.Empty;
            var list = await _orderDetailService.GetBySearch(page, pageSize, searchString);

            int totalCount = list.TotalCount;
            int numPages = (int)Math.Ceiling((float)totalCount / pageSize);

            var getList = (from obj in list.Records
                           select new
                           {
                               Name = obj.Order.User.Name,
                               ProductTitle = obj.Product.ProductTitle,
                               ProductId = obj.ProductId,
                               ProductCount = obj.ProductCount,
                               Sum = obj.Sum,
                               IsFinally = obj.Order.IsFinally,
                               AddedDate = obj.Order.AddedDate,
                               OrderId = obj.OrderId,
                           });

            return Json(new { getList, totalCount, numPages });
        }

    }
}
