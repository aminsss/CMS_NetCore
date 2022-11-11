using System.ComponentModel.DataAnnotations;

namespace CMS_NetCore.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Key]
        public int UserId { get; set; }

        [Display(Name = "رمز عبور پیشین")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید!")]
        [DataType(DataType.Password)]
        public string OldPass { get; set; }

        [Display(Name = "رمز عبور جدید")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید!")]
        [DataType(DataType.Password)]
        public string Pass { get; set; }

        [Display(Name = "تکرار رمز عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید!")]
        [DataType(DataType.Password)]
        [Compare(
            "pass",
            ErrorMessage = "کلمه عبور با هم مغایرت دارند"
        )]
        public string ConfirmPass { get; set; }
    }
}