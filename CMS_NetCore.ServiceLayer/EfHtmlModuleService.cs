using System.Threading.Tasks;
using CMS_NetCore.DataLayer;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.ServiceLayer;

public class EfHtmlModuleService : RepositoryBase<HtmlModule>, IHtmlModuleService
{
    public EfHtmlModuleService(AppDbContext context) : base(context)
    {
    }

    public async Task Edit(HtmlModule htmlModule)
    {
        Update(htmlModule);
        await SaveAsync();
    }

    public async Task<HtmlModule> GetByModuleId(int? moduleId) =>
        await FindByCondition(htmlModule => htmlModule.Id == moduleId).FirstOrDefaultAsync();
}