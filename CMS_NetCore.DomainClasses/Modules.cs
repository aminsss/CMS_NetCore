using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS_NetCore.DomainClasses;

public sealed class Module : BaseEntity
{
    public Module()
    {
        ModulePages = new HashSet<ModulePage>();
    }

    public int Id { get; set; }

    [Display(Name = "عنوان ماژول")]
    [StringLength(50, ErrorMessage = "تعداد کاراکترها را رعایت کنید")]
    public string Title { get; set; }

    [Display(Name = "مکان ماژول")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    public int PositionId { get; set; }

    [Display(Name = "وضعیت ماژول")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    public bool? IsActive { get; set; }

    [Display(Name = "سطح دسترسی")]
    public string Accessibility { get; set; }

    [Display(Name = "نوع کامپوننت")]
    public int ComponentId { get; set; }

    public int? DisplayOrder { get; set; }
    public ICollection<ModulePage> ModulePages { get; set; }
    public Component Component { get; set; }
    public Position Position { get; set; }
    public MenuModule MenuModule { get; set; }
    public HtmlModule HtmlModule { get; set; }
    public ContactModule ContactModule { get; set; }
    public MultiPictureModule MultiPictureModule { get; set; }
}