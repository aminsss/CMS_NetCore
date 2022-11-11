using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CMS_NetCore.DomainClasses;

[DisplayName("اخبار")]
public sealed class News : BaseEntity
{
    public News()
    {
        NewsGalleries = new HashSet<NewsGallery>();
        NewsTags = new HashSet<NewsTag>();
    }

    public int Id { get; set; }

    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [Display(Name = "گروه خبری")]
    public int NewsGroupId { get; set; }

    [Display(Name = "عنوان خبر")]
    [StringLength(
        50,
        ErrorMessage = "تعداد کاراکترها را رعایت کنید"
    )]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    public string NewsTitle { get; set; }

    [Display(Name = "توضیحات محصول")]
    [DataType(DataType.MultilineText)]
    public string NewsDescription { get; set; }

    [Display(Name = "تصویر محصول")]
    [StringLength(
        150,
        ErrorMessage = "تعداد کاراکترها را رعایت کنید"
    )]
    public string NewsImage { get; set; }

    public int UserId { get; set; }

    [Display(Name = "نام مستعار")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [StringLength(
        150,
        ErrorMessage = "تعداد کاراکترها را رعایت کنید"
    )]
    public string AliasName { get; set; }

    public ICollection<NewsGallery> NewsGalleries { get; set; }
    public ICollection<NewsTag> NewsTags { get; set; }
    public NewsGroup NewsGroup { get; set; }
    public User User { get; set; }
}