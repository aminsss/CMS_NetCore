using System.Threading.Tasks;
using CMS_NetCore.DataLayer;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.ServiceLayer
{
    public class EfMultiPictureModuleService : RepositoryBase<MultiPictureModule>, IMultiPictureModuleService
    {
        public EfMultiPictureModuleService(AppDbContext context) : base(context)
        {
        }

        public async Task Edit(MultiPictureModule htmlModule)
        {
            Update(htmlModule);
            await SaveAsync();
        }

        public async Task<MultiPictureModule> GetByModuleId(int? moduleId) =>
            await FindByCondition(multiPictureModule => multiPictureModule.ModuleId == moduleId)
                .FirstOrDefaultAsync();
    }
}