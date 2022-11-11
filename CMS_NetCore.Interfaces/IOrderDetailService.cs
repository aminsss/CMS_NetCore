using System.Collections.Generic;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.ViewModels;

namespace CMS_NetCore.Interfaces;

public interface IOrderDetailService
{
    Task<IList<OrderDetail>> GetById(int? id);

    Task<DataGridViewModel<OrderDetail>> GetBySearch(
        int page,
        int pageSize,
        string searchString
    );
}