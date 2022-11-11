using System.Collections.Generic;
using System.Threading.Tasks;
using CMS_NetCore.DataLayer;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.ServiceLayer;

public class EfNewsGalleryService : RepositoryBase<NewsGallery>, INewsGalleryService
{
    public EfNewsGalleryService(AppDbContext context) : base(context)
    {
    }

    public async Task<NewsGallery> GetById(int? id) =>
        await FindByCondition(newsGallery => newsGallery.Id.Equals(id))
            .FirstOrDefaultAsync();

    public async Task Remove(NewsGallery newsGallery)
    {
        Delete(newsGallery);
        await SaveAsync();
    }

    public async Task Add(IList<NewsGallery> newsGalleries)
    {
        foreach (var newsGallery in newsGalleries)
            Create(newsGallery);

        await SaveAsync();
    }
}