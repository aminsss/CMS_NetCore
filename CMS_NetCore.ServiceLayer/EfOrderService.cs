using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS_NetCore.DataLayer;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using CMS_NetCore.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.ServiceLayer;

public class EfOrderService : RepositoryBase<Order>, IOrderService
{
    public EfOrderService(AppDbContext context) : base(context)
    {
    }


    public async Task<DataGridViewModel<Order>> GetBySearch(
        int page,
        int pageSize,
        string searchString
    )
    {
        return new DataGridViewModel<Order>()
        {
            Records = await FindByCondition(
                    order => order.Id.ToString()
                        .Contains(searchString)
                )
                .Include(order => order.User)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .OrderBy(order => order.Id)
                .ToListAsync(),

            TotalCount = await FindByCondition(
                    order => order.Id.ToString()
                        .Contains(searchString)
                )
                .Include(order => order.User)
                .CountAsync()
        };
    }

    public async Task Add(Order order)
    {
        order.CreatedDate = DateTime.Now;
        order.ModifiedDate = DateTime.Now;
        Create(order);
        await SaveAsync();
    }

    public async Task Remove(Order order)
    {
        Delete(order);
        await SaveAsync();
    }

    public async Task Edit(Order order)
    {
        order.ModifiedDate = DateTime.Now;
        Update(order);
        await SaveAsync();
    }

    public async Task<Order> GetById(int? id) =>
        await FindByCondition(order => order.Id.Equals(id)).FirstOrDefaultAsync();

    public async Task<IEnumerable<Order>> GetAll() =>
        await FindAll().ToListAsync();
}