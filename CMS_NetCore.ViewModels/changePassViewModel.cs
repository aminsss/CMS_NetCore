using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace  CMS_NetCore.ViewModels
{
    public class changePassViewModel
    {
        [Key]
        public int UserId { get; set; }

        [Display(Name = "رمز عبور پیشین")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید!")]
        //[System.Web.Mvc.Remote("lastPass", "Users", ErrorMessage = "رمز را به درستی وارد نمایید!", AdditionalFields = "UserID")]
        [DataType(DataType.Password)]
        public string Oldpass { get; set; }

        [Display(Name = "رمز عبور جدید")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید!")]
        [DataType(DataType.Password)]
        public string pass { get; set; }

        [Display(Name = "تکرار رمز عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید!")]
        [DataType(DataType.Password)]
        [Compare("pass", ErrorMessage = "کلمه عبور با هم مغایرت دارند")]
        public string repass { get; set; }
    }
}