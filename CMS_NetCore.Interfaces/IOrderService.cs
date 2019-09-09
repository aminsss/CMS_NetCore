using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using  CMS_NetCore.ViewModels;

namespace CMS_NetCore.Interfaces
{
    public interface IOrderService
    {
        DataGridViewModel<Order> GetBySearch(int page, int pageSize, string searchString);
        Order GetById(int? id);
        void Add(Order order);
        void Edit(Order order);
        void Delete(Order order);
        void Delete(int? id);
        IEnumerable<Order> Orders();

        //orderDetail
        IList<OrderDetail> GetOrderDetail(int? id);
        DataGridViewModel<OrderDetail> GetAllDetail(int page, int pageSize, string searchString);
    }
}
