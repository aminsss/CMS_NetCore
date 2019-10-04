using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using  CMS_NetCore.ViewModels;

namespace CMS_NetCore.Interfaces
{
    public interface IDetailGroupService
    {
        Task<DataGridViewModel<DetailGroup>> GetBySearch(int page, int pageSize, string searchString);
        Task Add(DetailGroup detailGroup);
        Task Edit(DetailGroup detailGroup);
        Task Remove(DetailGroup detailGroup);
        Task<DetailGroup> GetById(int? id);
        Task<IEnumerable<DetailGroup>> GetAll();
        Task<IList<DetailGroup>> GetByProductGroupId(int productGroupId);
        Task<bool> DetailGroupExistence(int? id);
    }
}
