using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace  CMS_NetCore.ViewModels
{
    public class RegisterViewModel
    {
        [Display(Name = "نام")]
        [StringLength(50, ErrorMessage = "لطفا تعداد کاراکترها را رعایت کنید", MinimumLength = 3)]
        public string firstName { get; set; }

        [Display(Name = "نام")]
        [StringLength(50, ErrorMessage = "لطفا تعداد کاراکترها را رعایت کنید", MinimumLength = 3)]
        public string LastName { get; set; }

        [Display(Name = "موبایل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید!")]
        //[Remote("Uniqueusermobile", "Account", ErrorMessage = "شماره موبایل قبلا در سیستم ثبت‌ شده است!")]
        [StringLength(11, ErrorMessage = "لطفا شماره را به درستی وارد کنید", MinimumLength = 11)]
        public string moblie { get; set; }


        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید!")]
        [DataType(DataType.Password)]
        public string pass { get; set; }

        [Display(Name = "تکرار رمز عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید!")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("pass", ErrorMessage = "کلمه عبور با هم مغایرت دارند")]
        public string repass { get; set; }



        //[Range(typeof(bool), "true", "true", ErrorMessage = "ابتدا  شرایط و قوانین سایت را بپذیرید")]
        //public bool TermsAcpt { get; set; }
    }


    public class LoginViewModel
    {
        [Display(Name = "موبایل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید!")]
        [Display(Name = "رمز عبور")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "مرا به خاطر بسپار")]
        public bool Remember { get; set; }
    }

    public class ForgetPassViewModel
    {
        public string id { get; set; }

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید!")]
        [DataType(DataType.Password)]
        public string pass { get; set; }

        [Display(Name = "تکرار رمز عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید!")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("pass", ErrorMessage = "کلمه عبور با هم مغایرت دارند")]
        public string repass { get; set; }
    }

    public class UserpanelViewmodel
    {
        public int UserId { get; set; }

        [Display(Name = "نقش کاربر")]
        public int RoleID { get; set; }

      
        [Display(Name = "رمز عبور")]
        [StringLength(50, ErrorMessage = "لطفا تعداد کاراکترها را رعایت کنید", MinimumLength = 3)]
        public string Password { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید!")]
        //[Remote("UniqueEmail", "Users", ErrorMessage = "این ایمیل قبلا ثبت شده است", AdditionalFields = "UserID")]
        [Display(Name = "ایمیل")]
        [EmailAddress(ErrorMessage = "لطفا {0} را به درستی و بدون خط فاصله وارد کنید!")]
        public string Email { get; set; }

        [Display(Name = "تاریخ ثبت نام")]
        [DisplayFormat(DataFormatString = "{0: yyyy/MM/dd}")]
        public DateTime CreateTime { get; set; }

        [Display(Name = "کد فعالسازی")]
        public string ActiveCode { get; set; }

        [Display(Name = "وضعیت ثبت نام")]
        public bool ISActive { get; set; }

        [Display(Name = "تصویر پروفایل")]
        public string Profile { get; set; }

        [Display(Name = "نام")]
        [StringLength(50, ErrorMessage = "لطفا تعداد کاراکترها را رعایت کنید", MinimumLength = 3)]
        public string Name { get; set; }

        [Display(Name = "استان")]
        public Nullable<int> State { get; set; }

        [Required(ErrorMessage = "لطفا {0} خود را وارد کنید!")]
        [Display(Name = "شهر")]
        public Nullable<int> City { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید!")]
        [Display(Name = "شماره ملی")]
        [StringLength(10, ErrorMessage = "لطفا {0} را به درستی وارد کنید!", MinimumLength = 10)]
        public string MeliID { get; set; }

        [Required(ErrorMessage = "لطفا {0} خود را وارد کنید!")]
        [Display(Name = "تاریخ تولد")]
        public string BirthDate { get; set; }

        [Required(ErrorMessage = "لطفا {0} خود را وارد کنید!")]
        [Display(Name = "آدرس")]
        public string Adress { get; set; }

        //[Remote("Uniquemoblie", "Users", ErrorMessage = "این شماره قبلا گرفته شده است، لطفا شماره دیگری انتخاب کنید!", AdditionalFields = "UserID")]
        [Display(Name = "موبایل")]
        [StringLength(11, ErrorMessage = "لطفا تعداد شماره را رعایت کنید", MinimumLength = 11)]
        public string moblie { get; set; }
        public string phoneNo { get; set; }
    }
}