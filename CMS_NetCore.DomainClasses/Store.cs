using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS_NetCore.DomainClasses;

public sealed class Store : BaseEntity
{
    public Store()
    {
        UserStores = new HashSet<UserStore>();
        StoreTimes = new HashSet<StoreTime>();
        StoreFollowers = new HashSet<StoreFollower>();
        StoresProducts = new HashSet<StoreProduct>();
    }

    public string Id { get; set; }

    [Required(ErrorMessage = "لطفا {0} را وارد نمایید!")]
    [Display(Name = "شهر")]
    public int? CityId { get; set; }

    [Required(ErrorMessage = "لطفا {0} را وارد نمایید!")]
    [Display(Name = "نام فروشگاه")]
    [StringLength(
        50,
        ErrorMessage = "تعداد کاراکترها را رعایت کنید"
    )]
    public string Name { get; set; }

    [Required(ErrorMessage = "لطفا {0} را وارد نمایید!")]
    [Display(Name = "آدرس")]
    [StringLength(
        300,
        ErrorMessage = "تعداد کاراکترها را رعایت کنید"
    )]
    public string Address { get; set; }

    public string Description { get; set; }

    [StringLength(
        50,
        ErrorMessage = "تعداد کاراکترها را رعایت کنید"
    )]
    public string Icon { get; set; }

    public bool? IsActive { get; set; }

    [Required(ErrorMessage = "لطفا {0} را وارد نمایید!")]
    [Display(Name = "آدرس فروشگاه")]
    [StringLength(
        30,
        ErrorMessage = "لطفا تعداد کاراکترها را رعایت نمایید!",
        MinimumLength = 3
    )]
    public string SiteName { get; set; }

    [Required(ErrorMessage = "لطفا {0} را وارد نمایید!")]
    [Display(Name = "شماره تماس")]
    [StringLength(
        25,
        ErrorMessage = "لطفا {0} را به درستی وارد کنید!(نمونه : 09101112233)",
        MinimumLength = 10
    )]
    public string PhoneNo { get; set; }

    [StringLength(
        250,
        ErrorMessage = "تعداد کاراکترها را رعایت کنید"
    )]
    public string SiteAddress { get; set; }

    public int? Favorite { get; set; }
    public int? SeeStore { get; set; }
    public ICollection<UserStore> UserStores { get; set; }
    public ICollection<StoreTime> StoreTimes { get; set; }
    public ICollection<StoreFollower> StoreFollowers { get; set; }
    public ICollection<StoreProduct> StoresProducts { get; set; }
    public City City { get; set; }
    public StoreInfo StoreInfo { get; set; }
}