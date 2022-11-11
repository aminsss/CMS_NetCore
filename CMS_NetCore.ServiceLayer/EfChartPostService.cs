using System.Collections.Generic;
using System.Threading.Tasks;
using CMS_NetCore.DataLayer;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.ServiceLayer;

public class EfChartPostService : RepositoryBase<ChartPost>, IChartPost
{
    public EfChartPostService(AppDbContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<ChartPost>> ChartPosts() =>
        await FindAll().ToListAsync();
}