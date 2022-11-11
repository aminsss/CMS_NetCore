using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;

namespace CMS_NetCore.Interfaces;

public interface IMultiPictureModuleService
{
    Task<MultiPictureModule> GetByModuleId(int? moduleId);
    Task Edit(MultiPictureModule htmlModule);
}