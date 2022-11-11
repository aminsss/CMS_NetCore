using System.ComponentModel.DataAnnotations;
using CMS_NetCore.DomainClasses;

namespace CMS_NetCore.ViewModels;

public sealed class ContactModuleViewModel : HtmlModuleViewModel
{
    [Display(Name = "آدرس ایمیل")]
    public string Email { get; set; }

    [Display(Name = "شماره تماس")]
    public string PhoneNum { get; set; }

    [Display(Name = "شماره موبایل")]
    public string MobileNum { get; set; }

    [Display(Name = "آدرس")]
    [DataType(DataType.MultilineText)]
    public string Address { get; set; }

    [Display(Name = "کد پستی")]
    public string PostCode { get; set; }

    [Display(Name = "توضیحات")]
    [DataType(DataType.MultilineText)]
    public string Description { get; set; }

    public User Users { get; set; }
}