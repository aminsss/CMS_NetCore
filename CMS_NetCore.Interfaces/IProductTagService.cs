using System.Collections.Generic;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;

namespace CMS_NetCore.Interfaces;

public interface IProductTagService
{
    Task DeleteByProductId(int? productId);
    void Add(IList<ProductTag> productTags);
}