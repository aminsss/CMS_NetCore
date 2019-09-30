using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS_NetCore.DataLayer;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using CMS_NetCore.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.ServiceLayer
{
    public class EfNewsService : RepositoryBase<News>, INewsService
    {

        public EfNewsService(AppDbContext context) :base(context)
        {
        }

        public async Task<DataGridViewModel<News>> GetBySearch(int page, int pageSize, string searchString)
        {
            var dataGridView = new DataGridViewModel<News>
            {
                Records = await FindByCondition(x => x.NewsTitle.Contains(searchString) || x.NewsDescription.Contains(searchString))
                .OrderBy(x => x.NewsId).Include(x=> x.NewsGroup).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(),

                TotalCount  = await FindByCondition(x => x.NewsTitle.Contains(searchString) || x.NewsDescription.Contains(searchString))
                .OrderBy(x => x.NewsId).Include(x=>x.NewsGroup).CountAsync(),
            };

            return dataGridView;
        }
        public async Task Add(News news)
        {
            news.AddedDate = DateTime.Now;
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
           await FindByCondition(x => x.NewsId.Equals(id)).DefaultIfEmpty(new News()).SingleAsync();

        public async Task<bool> UniqueAlias(string aliasName, int? newsId) =>
            await FindByCondition(x => x.AliasName == aliasName && x.NewsId != newsId).AnyAsync();

        public async Task<bool> NewsExistence(int? id) =>
           await FindByCondition(x => x.NewsId.Equals(id)).AnyAsync();
    }
}
