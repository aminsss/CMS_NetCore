using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.ViewModels;
using CMS_NetCore.Interfaces;
using CMS_NetCore.DataLayer;
using Microsoft.EntityFrameworkCore;


namespace CMS_NetCore.ServiceLayer
{
    public class EfPositionService : RepositoryBase<Position>,IPositionService
    {

        public EfPositionService(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Position>> GetAll() =>
            await FindAll().ToListAsync();
    }
}
