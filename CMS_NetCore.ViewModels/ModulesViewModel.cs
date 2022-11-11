using System.ComponentModel.DataAnnotations;
using CMS_NetCore.DomainClasses;

namespace CMS_NetCore.ViewModels
{
    public class HtmlModuleViewModel
    {
        [Key]
        public int ModuleId { get; set; }

        [StringLength(
            50,
            ErrorMessage = "لطفا تعداد کاراکترها را رعایت کنید",
            MinimumLength = 3
        )]
        [Required(ErrorMessage = "عنوان ماژول را وارد کنید")]
        [Display(Name = "عنوان ماژول")]
        public string ModuleTitle { get; set; }

        [Display(Name = "مکان ماژول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int? PositionId { get; set; }

        [Display(Name = "وضعیت ماژول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public bool? IsActive { get; set; }

        public string Accisibility { get; set; }

        [Display(Name = "نوع کامپوننت")]
        public int ComponentId { get; set; }

        [Display(Name = "محتوای ماژول")]
        [DataType(DataType.MultilineText)]
        public string HtmlText { get; set; }

        public virtual Position Positions { get; set; }

        [Display(Name = "مرتب سازی")]
        public int? DisplayOrder { get; set; }
    }
}