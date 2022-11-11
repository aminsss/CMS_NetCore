using System.ComponentModel.DataAnnotations;

namespace CMS_NetCore.DomainClasses;

public sealed class Message : BaseEntity
{
    public int Id { get; set; }

    [Display(Name = "فرستنده")]
    public int? FromUser { get; set; }

    [Display(Name = "گیرنده")]
    public int? ToUser { get; set; }

    [Display(Name = "موضوع پیام")]
    [StringLength(50, ErrorMessage = "تعداد کاراکترها را رعایت کنید")]
    public string Subject { get; set; }

    [Display(Name = "محتوای پیام")]
    [DataType(DataType.MultilineText)]
    public string ContentMessage { get; set; }

    [Display(Name = "ایمیل")]
    [StringLength(100, ErrorMessage = "تعداد کاراکترها را رعایت کنید")]
    [DataType(DataType.EmailAddress, ErrorMessage = "لطفا ایمیل را به درستی وارد کنید")]
    public string Email { get; set; }

    public bool? IsRead { get; set; }

    [Display(Name = "نام فرستنده")]
    [StringLength(50, ErrorMessage = "تعداد کاراکترها را رعایت کنید")]
    public string SenderName { get; set; }

    public User UserFrom { get; set; }
    public User UserTo { get; set; }
}