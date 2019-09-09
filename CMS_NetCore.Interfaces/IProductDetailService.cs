using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;

namespace CMS_NetCore.Interfaces
{
    public interface IProductDetailService
    {
        ProductDetail GetProductDetail(int? productId, int? detailItemId);
        void Delete(ProductDetail productDetail );
        void Edit(ProductDetail productDetail);
        void Add(IList<ProductDetail> productDetails);
    }
}
