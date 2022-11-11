using System.ComponentModel.DataAnnotations;
using CMS_NetCore.DomainClasses;

namespace CMS_NetCore.ViewModels;

public sealed class MenuModuleViewModel : HtmlModuleViewModel
{
    [Display(Name = "انتخاب منو")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    public int MenuGroupId { get; set; }

    public MenuModule MenuModules { get; set; }
}