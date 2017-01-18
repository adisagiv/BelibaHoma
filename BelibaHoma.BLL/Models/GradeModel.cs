using System;
using System.ComponentModel.DataAnnotations;
using BelibaHoma.BLL.Enums;
using BelibaHoma.DAL;
using Generic.GenericModel.Models;
using Generic.Models;
using System.Collections.Generic;


namespace BelibaHoma.BLL.Models
{
    public class GradeModel : GenericModel
    {
        [Display(Name = " מספר סמסטר, סמסטר 0 משמע מכינה")]
        //TODO: manor ant atalia: validations dont work
        [Range(0, 20, ErrorMessage = "מספר סמסטר חייב להיות חיובי וקטן מ 20")]
        [Required(ErrorMessage = "מספר סמסטר זה שדה חובה")]
        public int SemesterNumber { get; set; }


        public int TraineeId { get; set; }

        public Trainee Trainee { get; set; }


        [Display(Name = "ממוצע ציונים בסמסטר")]
        [Required(ErrorMessage = "ממוצע ציונים זה שדה חובה")]
        //TODO: manor ant atalia: validations dont work
        [Range(0, 100, ErrorMessage = "ממוצע ציונים בסמסטר יכול להיות בין 0 .. 100 בלבד")]
        public int Grade1 { get; set; }

        [Display(Name = "שנה")]
        [Required(ErrorMessage = "שנה זה שדה חובה")]
        [Range(2000, 2050, ErrorMessage = "נא לשים שנה חוקית")]
        public int Year { get; set; }

        [Display(Name = "תאריך עדכון")]
        [Required]
        public DateTime UpdateDate { get; set; }

        [Display(Name = "סוג סמסטר")]
        [Required(ErrorMessage = "סוג סמסטר זה שדה חובה")]
        public SemesterType SemesterType { get; set; }


        public GradeModel(Grade entity)
            : base(entity)
        {

        }

        public GradeModel()
        {

        }
    }
}