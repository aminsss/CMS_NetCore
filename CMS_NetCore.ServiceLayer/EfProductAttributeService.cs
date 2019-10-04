using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.ViewModels;
using CMS_NetCore.Interfaces;
using CMS_NetCore.DataLayer;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.ServiceLayer
{
    public class EfProductAttributeService : RepositoryBase<Product_Attribut>,IProductAttributeService
    {

        public EfProductAttributeService(AppDbContext context) : base(context)
        {
        }


        public async Task<Product_Attribut> GetProductAttribute(int? productId, int? atrributeGrpId) =>
           await FindByCondition(x => x.ProductId == productId && x.AttributItem.AttributGrpId == atrributeGrpId).FirstOrDefaultAsync();

        public async Task Remove(Product_Attribut product_Attribut)
        {
            Delete(product_Attribut);
            await SaveAsync();
        }

        public async Task Edit(Product_Attribut product_Attribut)
        {
            Update(product_Attribut);
            await SaveAsync();
        }

        public async Task Add(IList<Product_Attribut> product_Attributs)
        {
            foreach (var product_Attribut in product_Attributs)
            {
                Create(product_Attribut);
            }
            await SaveAsync();
        }
    }
}
