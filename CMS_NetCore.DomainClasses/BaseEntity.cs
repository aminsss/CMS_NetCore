using System;
using System.ComponentModel.DataAnnotations;

namespace CMS_NetCore.DomainClasses
{
    public abstract class BaseEntity
    {
        [DisplayFormat(DataFormatString = "{0: yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "تاریخ ایجاد")]
        public DateTime? CreatedDate { get; set; }

        [DisplayFormat(DataFormatString = "{0: yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "تاریخ ویرایش")]
        public DateTime? ModifiedDate { get; set; }

        [MaxLength(100)] public string Ip { get; set; }
    }
}