using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_NetCore.DomainClasses
{
    public abstract class BaseEntity
    {
        [DisplayFormat(DataFormatString = "{0: yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "تاریخ ایجاد")]
        public Nullable<DateTime> AddedDate { get; set; }

        [DisplayFormat(DataFormatString = "{0: yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "تاریخ ویرایش")]
        public Nullable<DateTime> ModifiedDate { get; set; }

        [MaxLength(100)]
        public string IP { get; set; }
    }
}
