using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CMS_NetCore.DomainClasses;

namespace CMS_NetCore.ViewModels;

public class MultiPictureModuleViewModel : HtmlModuleViewModel
{
    [Display(Name = "عنوان متن")]
    public string Title { get; set; }

    [Display(Name = "عنوان دوم")]
    public string TitleBold { get; set; }

    [Display(Name = "توضیحات")]
    [DataType(DataType.MultilineText)]
    public string Description { get; set; }

    [Display(Name = "کاور")]
    public string Cover { get; set; }

    [Display(Name = "لینک")]
    public string Link { get; set; }

    [Display(Name = "لینک بیشتر")]
    public string LinkMore { get; set; }

    [Display(Name = "عکس")]
    public string ModuleImage { get; set; }

    public ICollection<MultiPictureItem> MultiPictureItems { get; set; }
}