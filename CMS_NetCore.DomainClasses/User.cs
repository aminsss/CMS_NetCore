using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS_NetCore.DomainClasses;

public sealed class User : BaseEntity
{
    public User()
    {
        UserAddresses = new HashSet<UserAddress>();
        ContactPersons = new HashSet<ContactPerson>();
        MessagesFrom = new HashSet<Message>();
        MessagesTo = new HashSet<Message>();
        News = new HashSet<News>();
        Orders = new HashSet<Order>();
        UserStores = new HashSet<UserStore>();
        ProductRequests = new HashSet<ProductRequest>();
        TicketMessages = new HashSet<TicketMessage>();
        Tickets = new HashSet<Ticket>();
        StoreFollowers = new HashSet<StoreFollower>();
    }

    public int Id { get; set; }

    [Display(Name = "نام")]
    [StringLength(
        50,
        ErrorMessage = "تعداد کاراکترها را رعایت کنید"
    )]
    public string Name { get; set; }

    [Display(Name = "نام کاربری")]
    [StringLength(
        50,
        ErrorMessage = "تعداد کاراکترها را رعایت کنید"
    )]
    public string Username { get; set; }

    [Display(Name = "رمز عبور")]
    [StringLength(
        50,
        ErrorMessage = "تعداد کاراکترها را رعایت کنید"
    )]
    public string Password { get; set; }

    public string Token { get; set; }

    [Display(Name = "کد فعالسازی")]
    [StringLength(
        100,
        ErrorMessage = "تعداد کاراکترها را رعایت کنید"
    )]
    public string ActiveCode { get; set; }

    [Display(Name = "نقش کاربر")]
    public int RoleId { get; set; }

    [Display(Name = "وضعیت ثبت نام")]
    public bool? IsActive { get; set; }

    [Display(Name = "ایمیل")]
    [StringLength(
        100,
        ErrorMessage = "تعداد کاراکترها را رعایت کنید"
    )]
    public string Email { get; set; }

    [Display(Name = "تصویر پروفایل")]
    [StringLength(
        50,
        ErrorMessage = "تعداد کاراکترها را رعایت کنید"
    )]
    public string Profile { get; set; }

    [Display(Name = "شماره ملی")]
    [StringLength(
        50,
        ErrorMessage = "تعداد کاراکترها را رعایت کنید"
    )]
    public string NationalCode { get; set; }

    [Display(Name = "تاریخ تولد")]
    [StringLength(
        50,
        ErrorMessage = "تعداد کاراکترها را رعایت کنید"
    )]
    public string BirthDate { get; set; }

    [Display(Name = "شماره موبایل")]
    [StringLength(
        50,
        ErrorMessage = "تعداد کاراکترها را رعایت کنید"
    )]
    public string Mobile { get; set; }

    [Display(Name = "شماره تلفن")]
    [StringLength(
        50,
        ErrorMessage = "تعداد کاراکترها را رعایت کنید"
    )]
    public string PhoneNo { get; set; }

    [Display(Name = "استان")]
    public int? State { get; set; }

    [Display(Name = "شهر")]
    public int? City { get; set; }

    [Display(Name = "آدرس")]
    [StringLength(
        50,
        ErrorMessage = "تعداد کاراکترها را رعایت کنید"
    )]
    public string Address { get; set; }

    public int? ChartPostId { get; set; }

    public DateTime? ActiveCodeDate { get; set; }

    public ICollection<UserAddress> UserAddresses { get; set; }
    public ICollection<ContactPerson> ContactPersons { get; set; }
    public ICollection<Message> MessagesFrom { get; set; }
    public ICollection<Message> MessagesTo { get; set; }
    public ICollection<News> News { get; set; }
    public ICollection<Order> Orders { get; set; }
    public ICollection<UserStore> UserStores { get; set; }
    public ICollection<ProductRequest> ProductRequests { get; set; }
    public ICollection<TicketMessage> TicketMessages { get; set; }
    public ICollection<Ticket> Tickets { get; set; }
    public ICollection<StoreFollower> StoreFollowers { get; set; }
    public Role Role { get; set; }
    public ChartPost ChartPost { get; set; }
}