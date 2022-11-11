using System;
using System.Linq;
using System.Threading.Tasks;
using CMS_NetCore.DataLayer;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using CMS_NetCore.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.ServiceLayer;

public class EfNewsService : RepositoryBase<News>, INewsService
{
    public EfNewsService(AppDbContext context) : base(context)
    {
    }

    public async Task<DataGridViewModel<News>> GetBySearch(
        int page,
        int pageSize,
        string searchString
    )
    {
        return new DataGridViewModel<News>
        {
            Records = await FindByCondition(
                    news =>
                        news.NewsTitle.Contains(searchString) ||
                        news.NewsDescription.Contains(searchString)
                )
                .OrderBy(news => news.Id)
                .Include(news => news.NewsGroup)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(),

            TotalCount = await FindByCondition(
                    news =>
                        news.NewsTitle.Contains(searchString) ||
                        news.NewsDescription.Contains(searchString)
                )
                .OrderBy(news => news.Id)
                .Include(news => news.NewsGroup)
                .CountAsync(),
        };
    }

    public async Task Add(News news)
    {
        news.CreatedDate = DateTime.Now;
        news.ModifiedDate = DateTime.Now;
        Create(news);
        await SaveAsync();
    }

    public async Task Remove(News news)
    {
        Delete(news);
        await SaveAsync();
    }

    public async Task Edit(News news)
    {
        news.ModifiedDate = DateTime.Now;
        Update(news);
        await SaveAsync();
    }

    public async Task<News> GetById(int? id) =>
        await FindByCondition(news => news.Id.Equals(id)).FirstOrDefaultAsync();

    public async Task<bool> UniqueAlias(
        string aliasName,
        int? newsId
    ) =>
        await FindByCondition(
            news =>
                news.AliasName == aliasName &&
                news.Id != newsId
        ).AnyAsync();

    public async Task<bool> IsExist(int? id) =>
        await FindByCondition(news => news.Id.Equals(id)).AnyAsync();
}