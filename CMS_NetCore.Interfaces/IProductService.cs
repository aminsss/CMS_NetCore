using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using  CMS_NetCore.ViewModels;
using CMS_NetCore.DomainClasses;

namespace CMS_NetCore.Interfaces
{
    public interface IProductService
    {
        DataGridViewModel<Product> GetBySearch(int page,int pageSize,string searchString);
        Product GetById(int? id);
        void Add(Product product);
        void Edit(Product product);
        void Delete(Product product);
        void Delete(int id);
        bool UniqueAlias(string AliasName, int? ProductId);
        IEnumerable<Product> Products();

        //product Tags
        void DeleteTagsByProduct(int? productId);
        void AddTags(IList<ProductTag> productTags);

        //product Gallery
        void DeleteGallery(ProductGallery productGallery);
        ProductGallery GetProductGalleryById(int id);
        void AddGalleries(IList<ProductGallery> productGalleries);

       
    }
}
