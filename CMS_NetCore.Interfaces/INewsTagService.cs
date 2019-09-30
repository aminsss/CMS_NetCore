using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using  CMS_NetCore.ViewModels;

namespace CMS_NetCore.Interfaces
{
    public interface INewsTagService
    {
        Task RemoveByNewsId(int? newsId);
        Task Add(IList<NewsTag> newsTags);
    }
}
