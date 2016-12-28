using System;
using System.ComponentModel.DataAnnotations;
using BelibaHoma.BLL.Enums;
using BelibaHoma.DAL;
using Generic.GenericModel.Models;
using Generic.Models;
using System.Collections.Generic;


namespace BelibaHoma.BLL.Models
{
    public class AlertModel : GenericModel
    {
        public int Id { get; set; }

        [Display(Name = "תאריך יצירה")]
        public DateTime CreationTime { get; set; }

        [Display(Name = "עדכון אחרון")]
        public DateTime UpdateTime { get; set; }

        [Display(Name = "סטטוס טיפול")]
        public AlertStatus Status { get; set; }

        [Display(Name = "הערות")]
        public string Notes { get; set; }

        [Display(Name = "סוג התרעה")]
        public AlertType AlertType { get; set; }

        [Display(Name = "חניך")]
        public Trainee Trainee { get; set; }

        [Display(Name = "חונך")]
        public Tutor Tutor { get; set; }

        [Display(Name = "דיווח")]
        public TutorReport TutorReport { get; set; }

        public AlertModel(Alert entity)
            : base(entity)
        {

        }

        public AlertModel()
        {

        }
    }
}