using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using  CMS_NetCore.ViewModels;

namespace CMS_NetCore.Interfaces
{
    public interface IProductAttributeService
    {
        Task<Product_Attribut> GetProductAttribute(int? productId, int? atrributeGrpId);
        Task Remove(Product_Attribut product_Attribut);
        Task Edit(Product_Attribut product_Attribut);
        Task Add(IList<Product_Attribut> product_Attributs);

    }
}
