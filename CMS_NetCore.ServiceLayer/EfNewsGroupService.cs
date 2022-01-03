using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using CMS_NetCore.DataLayer;
using CMS_NetCore.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.ServiceLayer
{
    public class EfNewsGroupService : RepositoryBase<NewsGroup>,INewsGroupService
    {
        

        public EfNewsGroupService(AppDbContext context) : base(context)
        {
        }

        public async Task<DataGridViewModel<NewsGroup>> GetBySearch(int page, int pageSize, string searchString)
        {
            var dataGridView = new DataGridViewModel<NewsGroup>
            {
                Records = await FindByCondition(x=>x.GroupTitle.Contains(searchString))
                .OrderBy(o=>o.NewsGroupId).Skip((page-1) *pageSize).Take(pageSize).ToListAsync()
            };

            return dataGridView;
        }

        public async Task<NewsGroup> GetById(int? id) =>
            await FindByCondition(x=>x.NewsGroupId.Equals(id)).FirstOrDefaultAsync();

        public async Task Add(NewsGroup newsGroup)
        {
            newsGroup.AddedDate = DateTime.Now;
            newsGroup.ModifiedDate = DateTime.Now;
            Create(newsGroup);
            await SaveAsync();
        }

        public async Task Edit(NewsGroup newsGroup)
        {
            newsGroup.ModifiedDate = DateTime.Now;
            Update(newsGroup);
            //Modify children of Edited Group
            await EditChild(newsGroup);
            await SaveAsync();
        }


        public async Task EditChild(NewsGroup newsGroup)
        {
            foreach (var child in await FindByCondition(x => x.ParentId == newsGroup.NewsGroupId).ToListAsync())
            {
                child.Depth = newsGroup.Depth + 1;
                child.Path = newsGroup.NewsGroupId + "/" + newsGroup.Path;
                Update(child);
                await EditChild(child);
            }
        }

        public async Task Remove(NewsGroup newsGroup)
        {
            //First Delete children of selected Group
            await RemoveChild(newsGroup);
            //Delete Selected Group
            Delete(newsGroup);
            await SaveAsync();
        }

        public async Task RemoveChild(NewsGroup newsGroup)
        {
            foreach (var child in await FindByCondition(x => x.ParentId == newsGroup.NewsGroupId).ToListAsync())
            {
                Delete(child);
                await RemoveChild(child);
            }
        }

        public async Task<IEnumerable<NewsGroup>> GetAll() =>
            await FindAll().ToListAsync();
        
        public async Task<bool> UniqueAlias(string aliasName, int? newsGroupId) =>
             await FindByCondition(s => s.AliasName == aliasName && s.NewsGroupId != newsGroupId).AnyAsync();

        public async Task<bool> NewsGroupExistence(int? id) =>
            await FindByCondition(x => x.NewsGroupId.Equals(id)).AnyAsync();
    }
}
