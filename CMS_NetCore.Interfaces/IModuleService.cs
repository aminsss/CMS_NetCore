﻿using System.Collections.Generic;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.ViewModels;

namespace CMS_NetCore.Interfaces;

public interface IModuleService
{
    Task<DataGridViewModel<Module>> GetBySearch(string searchString);
    Task Add(Module module);

    Task Edit(
        Module module,
        int? pastPosition,
        int? pastDisOrder
    );

    Task Remove(Module module);
    Task<Module> GetById(int? id);
    Task<Module> GetLastByPosition(int? id);
    Task<IList<Module>> GetByPositionId(int? id);
    Task<bool> IsExist(int? moduleId);
    Task<Module> GetMenuModuleById(int? id);
    Task<Module> GetHtmlModuleById(int? id);
    Task<Module> GetContactModuleById(int? id);
}