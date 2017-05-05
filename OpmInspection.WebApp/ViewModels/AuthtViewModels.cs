using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OpmInspection.WebApp.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email address")]
        [EmailAddress(ErrorMessage = "รูปแบบอีเมลไม่ถูกต้อง")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
