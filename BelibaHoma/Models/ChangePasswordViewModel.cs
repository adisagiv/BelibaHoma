using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BelibaHoma.Models
{
    public class ChangePasswordViewModel
    {
        [Display(Name = "סיסמא נוכחית")]
        [Required(ErrorMessage = "יש להזין את הסיסמא הנוכחית")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Display(Name = "סיסמא חדשה")]
        [Required(ErrorMessage = "נא להזין סיסמא חדשה")]
        [DataType(DataType.Password)]
        [MinLength(6)]
        [MaxLength(20)]
        public string NewPassword { get; set; }

        [Display(Name = "הזן סיסמא חדשה שנית")]
        [Required(ErrorMessage = "יש להזין את הסיסמא החדשה פעם נוספת")]
        [DataType(DataType.Password)]
        [MinLength(6)]
        [MaxLength(20)]
        public string ReTypePassword { get; set; }
    }
}