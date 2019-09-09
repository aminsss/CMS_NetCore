using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  CMS_NetCore.ViewModels
{
    public class DataGridViewModel<T>
    {
        public List<T> Records { get; set; }
        public int TotalCount { get; set; }
    }
}
