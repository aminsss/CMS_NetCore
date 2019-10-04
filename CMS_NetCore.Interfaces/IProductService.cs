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
        Task<DataGridViewModel<Product>> GetBySearch(int page,int pageSize,string searchString);
        Task<Product> GetById(int? id);
        Task Add(Product product);
        Task Edit(Product product);
        Task Remove(Product product);
        Task<bool> UniqueAlias(string aliasName, int? productId);
        Task<IEnumerable<Product>> GetAll();
    }
}
