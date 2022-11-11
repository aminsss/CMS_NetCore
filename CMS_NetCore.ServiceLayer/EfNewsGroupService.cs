using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS_NetCore.DataLayer;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using CMS_NetCore.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.ServiceLayer;

public class EfNewsGroupService : RepositoryBase<NewsGroup>, INewsGroupService
{
    public EfNewsGroupService(AppDbContext context) : base(context)
    {
    }

    public async Task<DataGridViewModel<NewsGroup>> GetBySearch(
        int page,
        int pageSize,
        string searchString
    )
    {
        return new DataGridViewModel<NewsGroup>
        {
            Records = await FindByCondition(
                    newsGroup => newsGroup.GroupTitle
                        .Contains(searchString)
                )
                .OrderBy(newsGroup => newsGroup.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync()
        };
    }

    public async Task<NewsGroup> GetById(int? id) =>
        await FindByCondition(newsGroup => newsGroup.Id.Equals(id))
            .FirstOrDefaultAsync();

    public async Task Add(NewsGroup newsGroup)
    {
        newsGroup.CreatedDate = DateTime.Now;
        newsGroup.ModifiedDate = DateTime.Now;
        Create(newsGroup);
        await SaveAsync();
    }

    public async Task Edit(NewsGroup newsGroup)
    {
        newsGroup.ModifiedDate = DateTime.Now;
        Update(newsGroup);
        await EditChild(newsGroup);
        await SaveAsync();
    }


    private async Task EditChild(NewsGroup newsGroup)
    {
        foreach (var child in await FindByCondition(
                         newsGroupCondition =>
                             newsGroupCondition.ParentId == newsGroup.Id
                     )
                     .ToListAsync())
        {
            child.Depth = newsGroup.Depth + 1;
            child.Path = newsGroup.Id + "/" + newsGroup.Path;
            Update(child);
            await EditChild(child);
        }
    }

    public async Task Remove(NewsGroup newsGroup)
    {
        await RemoveChild(newsGroup);
        Delete(newsGroup);
        await SaveAsync();
    }

    private async Task RemoveChild(NewsGroup newsGroup)
    {
        foreach (var child in await FindByCondition(
                         newsGroupCondition =>
                             newsGroupCondition.ParentId == newsGroup.Id
                     )
                     .ToListAsync())
        {
            Delete(child);
            await RemoveChild(child);
        }
    }

    public async Task<IEnumerable<NewsGroup>> GetAll() =>
        await FindAll().ToListAsync();

    public async Task<bool> UniqueAlias(
        string aliasName,
        int? newsGroupId
    ) =>
        await FindByCondition(
                newsGroup =>
                    newsGroup.AliasName == aliasName &&
                    newsGroup.Id != newsGroupId
            )
            .AnyAsync();

    public async Task<bool> NewsGroupExistence(int? id) =>
        await FindByCondition(newsGroup => newsGroup.Id.Equals(id)).AnyAsync();
}