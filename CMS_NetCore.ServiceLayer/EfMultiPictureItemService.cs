using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CMS_NetCore.DataLayer;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.ServiceLayer;

public class EfMultiPictureItemService : RepositoryBase<MultiPictureItem>, IMultiPictureItemService
{
    public EfMultiPictureItemService(AppDbContext context) : base(context)
    {
    }

    public async Task Add(MultiPictureItem multiPictureItem)
    {
        multiPictureItem.CreatedDate = DateTime.Now;
        multiPictureItem.Image = "no-photo.jpg";
        Create(multiPictureItem);
        await SaveAsync();
    }

    public async Task Edit(MultiPictureItem multiPictureItem)
    {
        multiPictureItem.ModifiedDate = DateTime.Now;
        Update(multiPictureItem);
        await SaveAsync();
    }

    public async Task<IList<MultiPictureItem>> GetMultiPictureItems(int? id) =>
        await FindByCondition(multiPictureItem => multiPictureItem.ModuleId == id)
            .ToListAsync();

    public async Task<MultiPictureItem> GetItemsById(int? id) =>
        await FindByCondition(multiPictureItem => multiPictureItem.Id == id)
            .FirstOrDefaultAsync();

    public async Task Remove(MultiPictureItem multiPictureItem)
    {
        Delete(multiPictureItem);
        await SaveAsync();
    }
}