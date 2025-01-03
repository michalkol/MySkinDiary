using System.ComponentModel.DataAnnotations;

namespace Diary.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "User name is required.")]
        public string UserName { get; set; }

        //[Required(ErrorMessage = "Email is required.")]
        //[EmailAddress]
        //public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        //public string ReturnUrl { get; set; }
    }
}
