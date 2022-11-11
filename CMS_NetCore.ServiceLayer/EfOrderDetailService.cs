using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS_NetCore.DataLayer;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using CMS_NetCore.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.ServiceLayer;

public class EfOrderDetailService : RepositoryBase<OrderDetail>, IOrderDetailService
{
    public EfOrderDetailService(AppDbContext context) : base(context)
    {
    }

    public async Task<DataGridViewModel<OrderDetail>> GetBySearch(
        int page,
        int pageSize,
        string searchString
    )
    {
        return new DataGridViewModel<OrderDetail>()
        {
            Records = await FindByCondition(
                    orderDetail => orderDetail.OrderId.ToString()
                        .Contains(searchString)
                )
                .Include(orderDetail => orderDetail.Order)
                .ThenInclude(order => order.User)
                .Include(orderDetail => orderDetail.Product)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .OrderBy(orderDetail => orderDetail.OrderId)
                .ToListAsync(),

            TotalCount = await FindByCondition(
                    orderDetail => orderDetail.OrderId.ToString()
                        .Contains(searchString)
                )
                .OrderBy(orderDetail => orderDetail.OrderId)
                .Include(orderDetail => orderDetail.Order)
                .ThenInclude(order => order.User)
                .Include(orderDetail => orderDetail.Product)
                .CountAsync()
        };
    }

    public async Task<IList<OrderDetail>> GetById(int? id) =>
        await FindByCondition(orderDetail => orderDetail.OrderId.Equals(id))
            .DefaultIfEmpty(new OrderDetail())
            .Include(orderDetail => orderDetail.Product)
            .Include(orderDetail => orderDetail.Order)
            .ToListAsync();
}