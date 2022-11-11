using System.Collections.Generic;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;

namespace CMS_NetCore.Interfaces;

public interface IMultiPictureItemService
{
    Task<IList<MultiPictureItem>> GetMultiPictureItems(int? id);
    Task Add(MultiPictureItem multiPictureItem);
    Task Edit(MultiPictureItem multiPictureItem);
    Task<MultiPictureItem> GetItemsById(int? id);
    Task Remove(MultiPictureItem multiPictureItem);
}