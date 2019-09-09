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
        DataGridViewModel<DetailItem> GetByDetGrpId(int? detailGroupId);
        DetailItem GetById(int? id);
        void Add(DetailItem detailItem);
        void Edit(DetailItem detailItem);
        void Delete(DetailItem detailItem);
        void Delete(int? id);
        IList<DetailItem> GetDetItemByProduct(Product product);
    }
}
