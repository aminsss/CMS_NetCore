using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS_NetCore.DomainClasses;

public sealed class MultiPictureModule : BaseEntity
{
    public MultiPictureModule()
    {
        MultiPictureItems = new HashSet<MultiPictureItem>();
    }

    [Key]
    public int ModuleId { get; set; }

    [Display(Name = "عنوان")]
    public string Title { get; set; }

    [Display(Name = "عنوان دوم")]
    public string TitleBold { get; set; }

    [Display(Name = "توضیحات")]
    public string Description { get; set; }

    [Display(Name = "کاور")]
    public string Cover { get; set; }

    [Display(Name = "لینک")]
    public string Link { get; set; }

    [Display(Name = "لینک بیشتر")]
    public string LinkMore { get; set; }

    [Display(Name = "عکس")]
    public string Image { get; set; }

    public Module Module { get; set; }
    public ICollection<MultiPictureItem> MultiPictureItems { get; set; }
}