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
        DataGridViewModel<DetailGroup> GetBySearch(int page, int pageSize, string searchString);
        void Add(DetailGroup detailGroup);
        void Edit(DetailGroup detailGroup);
        void Delete(DetailGroup detailGroup);
        void Delete(int? id);
        DetailGroup GetById(int? id);
        IEnumerable<DetailGroup> DetailGroup();

        //ProductController
        IList<DetailGroup> GetByProductGroup(int productGroupId);
    }
}
