using System.Collections.Generic;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.ViewModels;

namespace CMS_NetCore.Interfaces;

public interface IDetailItemService
{
    Task<DetailItem> GetById(int? id);
    Task<DataGridViewModel<DetailItem>> GetByDetailGroupId(int? detailGroupId);
    Task<IList<DetailItem>> GetByProductGroupId(Product product);
    Task Add(DetailItem detailItem);
    Task Edit(DetailItem detailItem);
    Task Remove(DetailItem detailItem);
    Task<bool> IsExist(int? detailItemId);
}