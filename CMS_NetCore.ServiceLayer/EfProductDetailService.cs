using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS_NetCore.DataLayer;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.ServiceLayer
{
    public class EfProductDetailService : RepositoryBase<ProductDetail>,IProductDetailService
    {

        public EfProductDetailService(AppDbContext context) : base(context)
        {
        }

        public async Task<ProductDetail> GetProductDetail(int? productId, int? detailItemId) =>
            await FindByCondition(x => x.ProductId == productId && x.DetailItemId == detailItemId).FirstOrDefaultAsync();

        public async Task Remove(ProductDetail productDetail)
        {
            Delete(productDetail);
            await SaveAsync();
        }

        public async Task Edit(ProductDetail productDetail)
        {
            Update(productDetail);
            await SaveAsync();
        }

        public async Task Add(IList<ProductDetail> productDetails)
        {
            foreach (var productDetail in productDetails)
            {
                Create(productDetail);
            }
            await SaveAsync();
        }
    }
}
