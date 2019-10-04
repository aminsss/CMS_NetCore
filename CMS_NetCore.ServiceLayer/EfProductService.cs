using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.DataLayer;
using CMS_NetCore.Interfaces;
using CMS_NetCore.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.ServiceLayer
{
    public class EfProductService : RepositoryBase<Product>,IProductService
    {

        public EfProductService(AppDbContext context) : base(context)
        {
        }

        public async Task<DataGridViewModel<Product>> GetBySearch(int page, int pageSize, string searchString)
        {
            var DataGridView = new DataGridViewModel<Product>
            {
                Records = await FindByCondition(s=>s.ProductName.Contains(searchString) ||
                s.ProductTitle.Contains(searchString)).OrderBy(o=>o.ModifiedDate).Include(x=>x.ProductGroup)
                .Skip((page -1) * pageSize).Take(pageSize).ToListAsync(),

                TotalCount = await FindByCondition(s => s.ProductName.Contains(searchString) ||
                s.ProductTitle.Contains(searchString)).OrderBy(o => o.ModifiedDate)
                .Include(x => x.ProductGroup).CountAsync()
            };

            return DataGridView;
        }
        public async Task Add(Product product)
        {
            product.AddedDate = DateTime.Now;
            product.ModifiedDate = DateTime.Now;
            Create(product);
            await SaveAsync();
        }

        public async Task Remove(Product product)
        {
            Delete(product);
            await SaveAsync();
        }

        public async Task Edit(Product product)
        {
            product.ModifiedDate = DateTime.Now;
            Update(product);
            await SaveAsync();
        }

        public async Task<Product> GetById(int? id) =>
            await FindByCondition(x=>x.ProductId.Equals(id)).DefaultIfEmpty(new Product()).SingleAsync();

        public async Task<bool> UniqueAlias(string aliasName, int? productId) =>
            await FindByCondition(s => s.AliasName == aliasName && s.ProductId != productId).AnyAsync();

        public async Task<IEnumerable<Product>> GetAll() =>
            await FindAll().ToListAsync();
    }
}
