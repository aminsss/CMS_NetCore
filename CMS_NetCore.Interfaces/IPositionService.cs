using System.Collections.Generic;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;

namespace CMS_NetCore.Interfaces;

public interface IPositionService
{
    Task<IEnumerable<Position>> GetAll();
}