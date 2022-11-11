using System.Collections.Generic;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.ViewModels;

namespace CMS_NetCore.Interfaces;

public interface IProductService
{
    Task<DataGridViewModel<Product>> GetBySearch(
        int page,
        int pageSize,
        string searchString
    );

    Task<Product> GetById(int? id);

    Task Add(
        Product product,
        string galleyFiles,
        string tags
    );

    Task Edit(
        Product product,
        string galleryFiles,
        string tags
    );

    Task Remove(
        Product product,
        string webRootPath
    );

    Task<bool> UniqueAlias(
        string aliasName,
        int? productId
    );

    Task<IEnumerable<Product>> GetAll();
    Task<bool> IsExist(int? productId);
    Task<Product> GetIncludeById(int? id);

    Task DeleteImage(
        int id,
        string webRootPath
    );
}