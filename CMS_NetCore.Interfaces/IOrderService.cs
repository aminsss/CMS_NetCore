using System.Collections.Generic;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.ViewModels;

namespace CMS_NetCore.Interfaces;

public interface IOrderService
{
    Task<DataGridViewModel<Order>> GetBySearch(
        int page,
        int pageSize,
        string searchString
    );

    Task<Order> GetById(int? id);
    Task Add(Order order);
    Task Edit(Order order);
    Task Remove(Order order);
    Task<IEnumerable<Order>> GetAll();
}