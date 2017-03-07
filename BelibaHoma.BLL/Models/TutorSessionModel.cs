using System;
using System.ComponentModel.DataAnnotations;
using BelibaHoma.BLL.Enums;
using BelibaHoma.DAL;
using Generic.GenericModel.Models;
using Generic.Models;
using System.Collections.Generic;


namespace BelibaHoma.BLL.Models
{
    public class TutorSessionModel : GenericModel
    {
        public int Id { get; set; }

        [Display(Name = "תאריך המפגש")]
        [Required(ErrorMessage = "תאריך מפגש זה שדה חובה")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime MeetingDate { get; set; }

        [Display(Name = "זמן תחילת המפגש")]
        [Required(ErrorMessage = "זמן תחילת המפגש זה שדה חובה")]
        [RegularExpression("^^([0-2]?[0-3]?):([0-5]?[0-9]?):([0-5]?[0-9]?)", ErrorMessage = "נא להזין פורמט זמן בלבד")]
        public TimeSpan StartTime { get; set; }

        [Display(Name = "זמן סיום המפגש")]
        //TODO: Atalia and manor Time Validation
        [Required(ErrorMessage = "זמן סיום המפגש זה שדה חובה")]
        [RegularExpression("^([0-2]?[0-3]?):([0-5]?[0-9]?):([0-5]?[0-9]?)", ErrorMessage = "נא להזין פורמט זמן בלבד")]
        public TimeSpan EndTime { get; set; }

        [Display(Name = "מספר שעות חברותה")]
        //TODO: Atalia and manor Time (or other relevant) Validation
        [Required(ErrorMessage = "מספר שעות חברותה זה שדה חובה")]
        [RegularExpression("^([0-9]*[.])?[0-9]+$", ErrorMessage = "נא להזין ספרות בלבד")]
        public double NumBondingHours { get; set; }

        
        public int TutorReportId { get; set; }

        public TutorReportModel TutorReport { get; set; }


        [Display(Name = "מקום המפגש")]
        [Required(ErrorMessage = "מקום מפגש זה שדה חובה")]
        public string MeetingPlace { get; set; }

        public TutorSessionModel(TutorSession entity)
            :base(entity)
        {

        }

        public TutorSessionModel()
        {

        }
    }
}