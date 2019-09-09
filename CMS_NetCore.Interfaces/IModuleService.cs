using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using  CMS_NetCore.ViewModels;

namespace CMS_NetCore.Interfaces
{
    public interface IModuleService
    {
        DataGridViewModel<Module> GetBySearch(string searchString);
        void Add(Module module);
        void Edit(Module module,int? pastPosition,int? pastDisOrder);
        void Delete(int id);
        Module GetById(int? id);
        Module GetLastByPosition(int? id);
        IList<Module> GetByPositionId(int? id);


        //position
        IEnumerable<Position> Positions();

    }
}
