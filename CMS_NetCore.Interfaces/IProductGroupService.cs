﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using  CMS_NetCore.ViewModels;

namespace CMS_NetCore.Interfaces
{
    public interface IProductGroupService
    {
        Task<DataGridViewModel<ProductGroup>> GetBySearch(int? page,int? pageSize,string searchString);
        Task<ProductGroup> GetById(int? id);
        Task<IList<ProductGroup>> GetByType(string type);
        Task Edit(ProductGroup productGroup);
        Task Remove(ProductGroup productGroup);
        Task Add(ProductGroup productGroup);
        Task<bool> UniqueAlias(string aliasName, int? productGroupId);
        Task<bool> ProductGroupExistense(int id);
        Task<IEnumerable<ProductGroup>> ProductGroups();
    }
}
