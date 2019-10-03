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
    public class EfDetailGroupService :  RepositoryBase<DetailGroup>,IDetailGroupService
    {

        public EfDetailGroupService(AppDbContext context) : base(context)
        {
        }

        public async Task<DataGridViewModel<DetailGroup>> GetBySearch(int page, int pageSize, string searchString)
        {
            var dataGridView = new DataGridViewModel<DetailGroup>
            {
                Records = await FindByCondition(s => s.Name.Contains(searchString)).OrderBy(x => x.DetailGroupId)
                .Include(x=>x.ProductGroup).Take(pageSize).Skip((page-1)*pageSize).ToListAsync(),
            };

            return dataGridView;
        }


        public async Task Add(DetailGroup detailGroup)
        {
            Create(detailGroup);
            await SaveAsync();
        }

        public async Task Remove(DetailGroup detailGroup)
        {
            Delete(detailGroup);
            await SaveAsync();
        }

        public async Task Edit(DetailGroup detailGroup)
        {
            Update(detailGroup);
            await SaveAsync();
        }

        public async Task<DetailGroup> GetById(int? id) =>
            await FindByCondition(x=>x.DetailGroupId.Equals(id)).DefaultIfEmpty(new DetailGroup()).SingleAsync();

        public async Task<IList<DetailGroup>> GetByProductGroupId(int productGroupId) =>
            await FindByCondition(x => x.ProductGroupId.Equals(productGroupId)).ToListAsync();

        public async Task<IEnumerable<DetailGroup>> GetAll() =>
           await FindAll().ToListAsync();
    }
}
