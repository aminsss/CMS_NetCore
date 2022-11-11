using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS_NetCore.DataLayer;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;

namespace CMS_NetCore.ServiceLayer;

public class EfNewsTagService : RepositoryBase<NewsTag>, INewsTagService
{
    public EfNewsTagService(AppDbContext context) : base(context)
    {
    }

    public async Task RemoveByNewsId(int? newsId)
    {
        FindByCondition(newsTag => newsTag.NewsId == newsId)
            .ToList()
            .ForEach(Delete);

        await SaveAsync();
    }

    public async Task Add(IEnumerable<NewsTag> newsTags)
    {
        foreach (var newsTag in newsTags)
        {
            Create(newsTag);
        }

        await SaveAsync();
    }
}