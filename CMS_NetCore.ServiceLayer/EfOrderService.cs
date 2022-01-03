using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.DataLayer;
using CMS_NetCore.ViewModels;
using CMS_NetCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.ServiceLayer
{

    public class EfOrderService : RepositoryBase<Order>, IOrderService
    {

        public EfOrderService(AppDbContext context) : base(context)
        {
        }


        public async Task<DataGridViewModel<Order>> GetBySearch(int page, int pageSize, string searchString)
        {
            var dataGridView = new DataGridViewModel<Order>()
            {
                Records = await FindByCondition(x=>x.OrderId.ToString().Contains(searchString))
                .OrderBy(o=>o.OrderId).Include(x=>x.User).Skip((page-1)*pageSize).Take(pageSize).ToListAsync(),

                TotalCount = await FindByCondition(x => x.OrderId.ToString().Contains(searchString))
                .OrderBy(o => o.OrderId).Include(x=>x.User).CountAsync()
            };

            return dataGridView;
        }
       
        public async Task Add(Order order)
        {
            order.AddedDate = DateTime.Now;
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
            await FindByCondition(x => x.OrderId.Equals(id)).FirstOrDefaultAsync();

        public async Task<IEnumerable<Order>> GetAll () =>
            await FindAll().ToListAsync();
    }
}
