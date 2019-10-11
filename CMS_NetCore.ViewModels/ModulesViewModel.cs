using CMS_NetCore.DomainClasses;
using System;
using System.ComponentModel.DataAnnotations;

namespace  CMS_NetCore.ViewModels
{
    public class MenuModulViewModel
    {
        public int ModuleId { get; set; }
        [StringLength(50, ErrorMessage = "لطفا تعداد کاراکترها را رعایت کنید", MinimumLength = 3)]
        [Required(ErrorMessage ="عنوان ماژول را وارد کنید")]
        [Display(Name = "عنوان ماژول")]
        public string ModuleTitle { get; set; }
        [Display(Name = "مکان ماژول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public Nullable<int> PositionId { get; set; }

        [Display(Name = "وضعیت ماژول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public Nullable<bool> IsActive { get; set; }
        public string Accisibility { get; set; }

        [Display(Name = "نوع کامپوننت")]
        public int ComponentID { get; set; }

        [Display(Name = "انتخاب منو")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int MenuGroupId { get; set; }
        public virtual Position Positions { get; set; }
        public virtual MenuModule MenuModules { get; set; }

        [Display(Name = "مرتب سازی")]
        public Nullable<int> DisplayOrder { get; set; }

    }

    public class HtmlModulViewModel
    {
        [Key]
        public int ModuleId { get; set; }
        [StringLength(50, ErrorMessage = "لطفا تعداد کاراکترها را رعایت کنید", MinimumLength = 3)]
        [Required(ErrorMessage = "عنوان ماژول را وارد کنید")]
        [Display(Name = "عنوان ماژول")]
        public string ModuleTitle { get; set; }
        [Display(Name = "مکان ماژول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public Nullable<int> PositionId { get; set; }

        [Display(Name = "وضعیت ماژول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public Nullable<bool> IsActive { get; set; }
        public string Accisibility { get; set; }

        [Display(Name = "نوع کامپوننت")]
        public int ComponentId { get; set; }

        [Display(Name = "محتوای ماژول")]
        //[AllowHtml]
        [DataType(DataType.MultilineText)]
        public string HtmlText { get; set; }
        public virtual Position Positions { get; set; }

        [Display(Name = "مرتب سازی")]
        public Nullable<int> DisplayOrder { get; set; }

    }

    public class ContactModuleViewModel
    {
        [Key]
        public int ModuleId { get; set; }

        [StringLength(50, ErrorMessage = "لطفا تعداد کاراکترها را رعایت کنید", MinimumLength = 3)]
        [Required(ErrorMessage = "عنوان ماژول را وارد کنید")]
        [Display(Name = "عنوان ماژول")]
        public string ModuleTitle { get; set; }

        [Display(Name = "مکان ماژول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public Nullable<int> PositionId { get; set; }

        [Display(Name = "وضعیت ماژول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public Nullable<bool> IsActive { get; set; }
        public string Accisibility { get; set; }

        [Display(Name = "نوع کامپوننت")]
        public int ComponentId { get; set; }

        [Display(Name = "مرتب سازی")]
        public Nullable<int> DisplayOrder { get; set; }


        [Display(Name = "آدرس ایمیل")]
        public string Email { get; set; }

        [Display(Name = "شماره تماس")]
        public string PhoneNum { get; set; }

        [Display(Name = "شماره موبایل")]
        public string MobileNum { get; set; }

        [Display(Name = "آدرس")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [Display(Name = "کد پستی")]
        public string PostCode { get; set; }

        [Display(Name = "توضیحات")]
        //[AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public virtual User Users { get; set; }


    }
}