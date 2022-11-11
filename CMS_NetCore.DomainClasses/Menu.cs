using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS_NetCore.DomainClasses;

public sealed class Menu
{
    public Menu()
    {
        ModulePages = new HashSet<ModulePage>();
    }

    public int Id { get; set; }

    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [StringLength(
        50,
        ErrorMessage = "لطفا تعداد کاراکترها را رعایت کنید",
        MinimumLength = 3
    )]
    [Display(Name = "عنوان منو")]
    public string Title { get; set; }

    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [StringLength(
        50,
        ErrorMessage = "لطفا تعداد کاراکترها را رعایت کنید",
        MinimumLength = 3
    )]
    [Display(Name = "نام مستعار")]
    public string PageName { get; set; }

    public int? Depth { get; set; }

    [StringLength(100)]
    public string Path { get; set; }

    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [Display(Name = "وضعیت گزینه منو")]
    public bool? IsActive { get; set; }

    [Display(Name = "ترتیب منوها")]
    public int? DisplayOrder { get; set; }

    [Display(Name = "والد گزینه منو")]
    public int? ParentId { get; set; }

    [Display(Name = "توضیحات صفحه")]
    [DataType(DataType.MultilineText)]
    public string Description { get; set; }

    [StringLength(100)]
    [Display(Name = "نوع آیتم منو")]
    public string PageType { get; set; }

    [Display(Name = "محتوای صفحه منو")]
    [StringLength(100)]
    public string PageContent { get; set; }

    [Display(Name = "موقعیت منو")]
    public int MenuGroupId { get; set; }

    public MenuGroup MenuGroup { get; set; }

    public ICollection<ModulePage> ModulePages { get; set; }
}