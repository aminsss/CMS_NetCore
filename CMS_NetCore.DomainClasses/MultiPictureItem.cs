using System.ComponentModel.DataAnnotations;

namespace CMS_NetCore.DomainClasses;

public sealed class MultiPictureItem : BaseEntity
{
    public int Id { get; set; }

    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [Display(Name = "عنوان")]
    public string Title { get; set; }

    [Display(Name = "عنوان دوم")]
    public string TitleBold { get; set; }

    [Display(Name = "توضیحات")]
    [DataType(DataType.MultilineText)]
    public string Description { get; set; }

    [Display(Name = "لینک")]
    public string Link { get; set; }

    [Display(Name = "لینک بیشتر")]
    public string LinkMore { get; set; }

    [Display(Name = "عکس")]
    public string Image { get; set; }

    public int ModuleId { get; set; }
    public MultiPictureModule MultiPictureModule { get; set; }
}