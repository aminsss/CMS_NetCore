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
    
    public class EfOrderDetailService : RepositoryBase<OrderDetail>, IOrderDetailService
    {

        public EfOrderDetailService(AppDbContext context ) : base(context)
        {
        }

        public async Task<DataGridViewModel<OrderDetail>> GetBySearch(int page, int pageSize, string searchString)
        {
            var dataGridView = new DataGridViewModel<OrderDetail>()
            {
                Records = await FindByCondition(x => x.OrderId.ToString().Contains(searchString))
                .OrderBy(o => o.OrderId).Include(x=>x.Order).ThenInclude(x=>x.User).Include(x=>x.Product)
                .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(),

                TotalCount = await FindByCondition(x => x.OrderId.ToString().Contains(searchString))
                .OrderBy(o => o.OrderId).Include(x => x.Order).ThenInclude(x=>x.User).Include(x => x.Product)
                .CountAsync()
            };

            return dataGridView;
        }

        public async Task<IList<OrderDetail>> GetById(int? id) =>
            await FindByCondition(x => x.OrderId.Equals(id)).DefaultIfEmpty(new OrderDetail())
            .Include(x=>x.Product).Include(x=>x.Order).ToListAsync();

    }
}
