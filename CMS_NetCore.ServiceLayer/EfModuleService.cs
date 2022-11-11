using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS_NetCore.DataLayer;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using CMS_NetCore.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.ServiceLayer;

public class EfModuleService : RepositoryBase<Module>, IModuleService
{
    public EfModuleService(AppDbContext context) : base(context)
    {
    }

    public async Task<DataGridViewModel<Module>> GetBySearch(string searchString)
    {
        return new DataGridViewModel<Module>
        {
            Records = await FindByCondition(module => module.Title.Contains(searchString))
                .Include(module => module.Component)
                .Include(module => module.Position)
                .OrderBy(module => module.DisplayOrder)
                .ThenBy(module => module.PositionId)
                .ToListAsync()
        };
    }

    public async Task Add(Module module)
    {
        module.CreatedDate = DateTime.Now;
        module.ModifiedDate = DateTime.Now;

        //method for assign order in create
        var last = await GetLastByPosition(module.PositionId);
        if (last == null)
            module.DisplayOrder = 1;
        else
            module.DisplayOrder = last.DisplayOrder + 1;

        Create(module);
        await SaveAsync();
    }


    public async Task Edit(
        Module module,
        int? pastPosition,
        int? pastDisOrder
    )
    {
        module.ModifiedDate = DateTime.Now;

        // ordering => if new order is lower than this module
        if (pastPosition == module.PositionId && module.DisplayOrder < pastDisOrder)
        {
            foreach (var moduleEntity in await FindByCondition(
                             moduleCondition =>
                                 moduleCondition.PositionId == module.PositionId &&
                                 moduleCondition.DisplayOrder >= module.DisplayOrder &&
                                 moduleCondition.DisplayOrder < pastDisOrder
                         ).OrderBy(moduleCondition => moduleCondition.DisplayOrder)
                         .ToListAsync())
            {
                moduleEntity.DisplayOrder += 1;
                Update(moduleEntity);
            }
        }
        //if new order is higher than this module
        else if (pastPosition == module.PositionId && module.DisplayOrder > pastDisOrder)
        {
            foreach (var moduleEntity in await FindByCondition(
                             moduleCondition =>
                                 moduleCondition.PositionId == module.PositionId &&
                                 moduleCondition.DisplayOrder <= module.DisplayOrder &&
                                 moduleCondition.DisplayOrder > pastDisOrder
                         ).OrderBy(o => o.DisplayOrder)
                         .ToListAsync())
            {
                moduleEntity.DisplayOrder -= 1;
                Update(moduleEntity);
            }
        }
        //if menu choose another position
        else if (pastPosition != module.PositionId)
        {
            //ordering display order of past position 
            foreach (var moduleEntity in await FindByCondition(
                             moduleCondition =>
                                 moduleCondition.PositionId == pastPosition &&
                                 moduleCondition.DisplayOrder > pastDisOrder
                         ).OrderBy(o => o.DisplayOrder)
                         .ToListAsync())
            {
                moduleEntity.DisplayOrder -= 1;
                Update(moduleEntity);
            }

            //making the last child of new position 
            var last = await GetLastByPosition(module.PositionId);
            if (last == null)
                module.DisplayOrder = 1;
            else
                module.DisplayOrder = last.DisplayOrder + 1;
        }

        Update(module);
        await SaveAsync();
    }

    public async Task Remove(Module module)
    {
        //editing order of modules with bigger display Order in current Position
        foreach (var moduleEntity in await FindByCondition(
                         moduleCondition =>
                             moduleCondition.PositionId == module.PositionId &&
                             moduleCondition.DisplayOrder > module.DisplayOrder
                     ).OrderBy(o => o.DisplayOrder)
                     .ToListAsync())
        {
            moduleEntity.DisplayOrder -= 1;
            Update(moduleEntity);
        }

        Delete(module);
        await SaveAsync();
    }

    public async Task<Module> GetLastByPosition(int? positionId) =>
        await FindByCondition(module => module.PositionId == positionId)
            .OrderByDescending(module => module.DisplayOrder)
            .FirstOrDefaultAsync();

    public async Task<IList<Module>> GetByPositionId(int? id) =>
        await FindByCondition(module => module.PositionId == id).ToListAsync();

    public async Task<Module> GetById(int? id) =>
        await FindByCondition(module => module.Id.Equals(id))
            .Include(module => module.MultiPictureModule)
            .FirstOrDefaultAsync();

    public async Task<bool> IsExist(int? id) =>
        await FindByCondition(module => module.Id.Equals(id)).AnyAsync();

    public async Task<Module> GetMenuModuleById(int? id) =>
        await FindByCondition(module => module.Id.Equals(id))
            .Include(x => x.MenuModule)
            .FirstOrDefaultAsync();

    public async Task<Module> GetHtmlModuleById(int? id) =>
        await FindByCondition(module => module.Id.Equals(id))
            .Include(module => module.HtmlModule)
            .Include(module => module.MultiPictureModule)
            .FirstOrDefaultAsync();

    public async Task<Module> GetContactModuleById(int? id) =>
        await FindByCondition(module => module.Id.Equals(id))
            .Include(x => x.ContactModule)
            .FirstOrDefaultAsync();
}