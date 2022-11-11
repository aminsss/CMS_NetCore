using System.Collections.Generic;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.ViewModels;

namespace CMS_NetCore.Interfaces;

public interface INewsGroupService
{
    Task<IEnumerable<NewsGroup>> GetAll();

    Task<DataGridViewModel<NewsGroup>> GetBySearch(
        int page,
        int pageSize,
        string searchString
    );

    Task<NewsGroup> GetById(int? id);
    Task Add(NewsGroup newsGroup);
    Task Edit(NewsGroup newsGroup);
    Task Remove(NewsGroup newsGroup);

    Task<bool> UniqueAlias(
        string aliasName,
        int? newsGroupId
    );

    Task<bool> NewsGroupExistence(int? id);
}