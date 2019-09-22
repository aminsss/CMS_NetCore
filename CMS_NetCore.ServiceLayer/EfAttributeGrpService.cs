using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS_NetCore.Interfaces;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.ViewModels;
using CMS_NetCore.DataLayer;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.ServiceLayer
{
    public class EfAttributeGrpService : RepositoryBase<AttributGrp>, IAttributeGrpService
    {
        public EfAttributeGrpService(AppDbContext context )
        : base(context)
        { }

        public async Task Add(AttributGrp attributGrp)
        {
            Create(attributGrp);
            await SaveAsync();
        }

        public async Task Remove(AttributGrp attributGrp)
        {
            Delete(attributGrp);
            await SaveAsync();
        }

        public async Task Edit(AttributGrp attributGrp)
        {
            Update(attributGrp);
            await SaveAsync();
        }

        public async Task<IEnumerable<AttributGrp>> GetAll() =>
             await FindAll().ToListAsync();
        
        public async Task<DataGridViewModel<AttributGrp>> GetBySearch(int page, int pageSize, string searchString)
        {
            var DataGridView = new DataGridViewModel<AttributGrp>
            {
                Records = await FindByCondition(x => x.Name.Contains(searchString)).Include(x=>x.ProductGroup)
                .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(),

                TotalCount = await FindByCondition(x => x.Name.Contains(searchString)).Include(x => x.ProductGroup)
                .Skip((page - 1) * pageSize).Take(pageSize).CountAsync(),
            };

            return DataGridView;
        }

        public async Task<AttributGrp> GetById(int? id) =>
            await FindByCondition(x=>x.ProductGroupId.Equals(id)).Include(x=>x.ProductGroup)
            .DefaultIfEmpty(new AttributGrp()).SingleAsync();

        public async Task<bool> AttributeGrpExistence(int? id) =>
            await FindByCondition(x => x.AttributGrpId.Equals(id)).AnyAsync();
    }
}
