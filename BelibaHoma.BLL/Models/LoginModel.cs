using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BelibaHoma.BLL.Models
{
    public class LoginModel
    {
        // TODO : Add error message for validation
        [Required(ErrorMessage = "dfgdfg ")]
        [Display(Name = "שם משתמש")]
        public string Username { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "סיסמא")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string UrlRedirect { get; set; }
    }
}