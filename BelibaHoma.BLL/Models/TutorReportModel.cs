using System;
using System.ComponentModel.DataAnnotations;
using BelibaHoma.BLL.Enums;
using BelibaHoma.DAL;
using Generic.GenericModel.Models;
using Generic.Models;
using System.Collections.Generic;


namespace BelibaHoma.BLL.Models
{
    public class TutorReportModel : GenericModel
    {
        public int Id { get; set; }

        public int TutorTraineeId { get; set; }

        public TutorTraineeModel TutorTrainee { get; set; }


        [Display(Name = "שעות חונכות")]
        [Required(ErrorMessage = "שעות חונכות זה שדה חובה")]
        public double TutorHours { get; set; }

        [Display(Name = "שעות חברותא")]
        [Required]
        public double TutorHoursBonding { get; set; }

        [Display(Name = "תיאור המפגש")]
        [Required(ErrorMessage = "תיאור המפגש זה שדה חובה")]
        public string MeetingsDescription { get; set; }

        [Display(Name = "דרושה התערבות?")]
        //[Required(ErrorMessage = "דרושה התערבות זה שדה חובה")]
        public bool IsProblem { get; set; }



        [Display(Name = "זמן יצירה")]
        [Required(ErrorMessage = "זמן יצירה זה שדה חובה")]
        public DateTime CreationTime { get; set; }

        public TutorReportModel(TutorReport entity)
            : base(entity)
        {

        }

        public TutorReportModel()
        {

        }
    }
}