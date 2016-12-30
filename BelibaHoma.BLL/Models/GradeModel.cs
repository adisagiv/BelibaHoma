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
        [Display(Name = "מספר סמסטר")]
        public int SemesterNumber { get; set; }


        //TODO: how to define it as PK and FK??
        public int TraineeId { get; set; }

        public Trainee Trainee { get; set; }


        [Display(Name = "ממוצע ציונים בסמסטר")]
        [Required(ErrorMessage = "ממוצע ציונים זה שדה חובה")]
        public int Grade1 { get; set; }

        [Display(Name = "שנה")]
        [Required(ErrorMessage = "שנה זה שדה חובה")]
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