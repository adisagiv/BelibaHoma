using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BelibaHoma.Models
{
    public class ChangePasswordViewModel
    {
        [Display(Name = "סיסמא נוחכית")]
        [Required(ErrorMessage = "נא להזין סיסמא נוחכית")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Display(Name = "סיסמא חדשה")]
        [Required(ErrorMessage = "נא להזין סיסמא חדשה")]
        [DataType(DataType.Password)]
        [MinLength(6)]
        [MaxLength(20)]
        public string NewPassword { get; set; }

        [Display(Name = "הזן סיסמא חדשה שוב")]
        [Required(ErrorMessage = "נא להזין סיסמא חדשה שוב")]
        [DataType(DataType.Password)]
        [MinLength(6)]
        [MaxLength(20)]
        public string ReTypePassword { get; set; }
    }
}