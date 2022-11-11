using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS_NetCore.DomainClasses;

public sealed class Role
{
    public Role()
    {
        Users = new HashSet<User>();
    }

    public int Id { get; set; }

    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [Display(Name = "عنوان نقش")]
    [StringLength(
        50,
        ErrorMessage = "تعداد کاراکترها را رعایت کنید"
    )]
    public string Title { get; set; }

    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [Display(Name = "نقش سیستمی")]
    [StringLength(
        50,
        ErrorMessage = "تعداد کاراکترها را رعایت کنید"
    )]
    public string Name { get; set; }

    public ICollection<User> Users { get; set; }
}