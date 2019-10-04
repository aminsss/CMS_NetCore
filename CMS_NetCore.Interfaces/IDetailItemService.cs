using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using  CMS_NetCore.ViewModels;
using CMS_NetCore.DomainClasses;

namespace CMS_NetCore.Interfaces
{
    public interface IDetailItemService
    {
        Task<DataGridViewModel<DetailItem>> GetByDetailGroupId(int? detailGroupId);
        Task<DetailItem> GetById(int? id);
        Task Add(DetailItem detailItem);
        Task Edit(DetailItem detailItem);
        Task Remove(DetailItem detailItem);
        Task<IList<DetailItem>> GetByProduct(Product product);
        Task<bool> DetailItemExistence(int? id);
    }
}
