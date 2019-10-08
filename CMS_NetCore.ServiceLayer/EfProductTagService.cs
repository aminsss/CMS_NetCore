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
    public class EfProductTagService : RepositoryBase<ProductTag>,IProductTagService
    {

        public EfProductTagService(AppDbContext context) : base(context)
        {
        }

        public async Task DeleteByProductId(int? productId)
        {
            var list = await FindByCondition(t => t.ProductId == productId).ToListAsync();
            foreach (var item in list)
            {
                Delete(item);
            }
            await SaveAsync();
        }

        public void Add(IList<ProductTag> productTags)
        {
            foreach (var productTag in productTags)
            {
                Create(productTag);
            }
        }
    }
}
