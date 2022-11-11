using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS_NetCore.DomainClasses;

public sealed class MenuGroup
{
    public MenuGroup()
    {
        Menus = new HashSet<Menu>();
        MenuModules = new HashSet<MenuModule>();
    }

    public int Id { get; set; }

    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [StringLength(
        50,
        ErrorMessage = "لطفا تعداد کاراکترها را رعایت کنید",
        MinimumLength = 3
    )]
    [Display(Name = "عنوان گروه منو")]
    public string Title { get; set; }

    [Display(Name = "نوع منو")]
    [StringLength(50)]
    public string Type { get; set; }

    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [Display(Name = "وضعیت گرووه")]
    public bool? IsActive { get; set; }

    public ICollection<Menu> Menus { get; set; }
    public ICollection<MenuModule> MenuModules { get; set; }
}