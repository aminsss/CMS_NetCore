using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS_NetCore.Interfaces;
using CMS_NetCore.DataLayer;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.ViewModels;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.ServiceLayer
{
    public class EfChartPostService : RepositoryBase<chartPost>, IChartPost
    {
       
        public EfChartPostService(AppDbContext contex)
        : base(contex)
        {
        }

        public async Task<IEnumerable<chartPost>> chartPosts() =>
                    await FindAll().ToListAsync();

    }
}
