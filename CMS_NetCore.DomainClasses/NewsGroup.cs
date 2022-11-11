using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS_NetCore.DomainClasses;

public sealed class NewsGroup : BaseEntity
{
    public NewsGroup()
    {
        News = new HashSet<News>();
    }

    public int Id { get; set; }

    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [StringLength(
        50,
        ErrorMessage = "تعداد کاراکترها را رعایت کنید"
    )]
    [Display(Name = "عنوان گروه")]
    public string GroupTitle { get; set; }

    public int? Depth { get; set; }

    public string Path { get; set; }

    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [Display(Name = "وضعیت گروه")]
    public bool? IsActive { get; set; }

    public int? DisplayOrder { get; set; }

    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [Display(Name = "گروه والد")]
    public int? ParentId { get; set; }

    [Display(Name = "نام مستعار")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [StringLength(
        100,
        ErrorMessage = "تعداد کاراکترها را رعایت کنید"
    )]
    public string AliasName { get; set; }

    public ICollection<News> News { get; set; }
}