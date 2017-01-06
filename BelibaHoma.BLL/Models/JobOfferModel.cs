using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using BelibaHoma.BLL.Enums;
using BelibaHoma.DAL;
using Generic.GenericModel.Models;


namespace BelibaHoma.BLL.Models
{
    public class JobOfferModel : GenericModel
    {
        public int Id { get; set; }

        [Display(Name = "שם המשרה")]
        [Required(ErrorMessage = "שם משרה זהו שדה חובה")]
        public string JobTitle { get; set; }

        [Display(Name = "זמן העדכון")]
        [Required(ErrorMessage = "זמן העדכון זהו שדה חובה")]
        public DateTime UpdateTime { get; set; } //add relevent function

        [Display(Name = "זמן יצירת המשרה")]
        [Required(ErrorMessage = "זמן יצירת המשרה זהו שדה חובה")]
        public DateTime CreationTime { get; set; } //add relevent function

        [Display(Name = "מיקום המשרה")]
        [Required]
        public JobArea JobArea { get; set; }

        [Display(Name = "דרישות המשרה")]
        [Required(ErrorMessage = "דרישות המשרה זהו שדה חובה")]
        public string Requirements { get; set; }

        [Display(Name = "תיאור המשרה")]
        [Required(ErrorMessage = "תיאור המשרה זהו שדה חובה")]
        public string Description { get; set; }

        //foreignKey:
        //[ForeignKey("AcademicMajorModel")]
        //[Required(ErrorMessage = "זהו שדה חובה")]
        //[ForeignKey("RelevantMajorRef2")]
       // public AcademicMajorModel  { get; set; }
        //public AcademicMajorModel AcademicMajorModel { get; set; }

        [Required(ErrorMessage = "מסלול לימודים זהו שדה חובה")]
        public int RelevantMajorId1 { get; set; }
        public int? RelevantMajorId2 { get; set; }
        public int? RelevantMajorId3 { get; set; }

        public AcademicMajorModel AcademicMajor { get; set; }
        public AcademicMajorModel AcademicMajor1 { get; set; }
        public AcademicMajorModel AcademicMajor2 { get; set; }
        //public int RelevantMajorRef2 { get; set; }
        //[ForeignKey("RelevantMajorRef2")]
        //public AcademicMajorModel RelevantMajor2 { get; set; }

        //public int RelevantMajorRef3 { get; set; }
        //[ForeignKey("RelevantMajorRef3")]
        //public AcademicMajorModel RelevantMajor3 { get; set; }

        [Display(Name = "שם החברה")]
        [Required(ErrorMessage = "שם החברה זהו שדה חובה")]
        public string Organization { get; set; }

        [Display(Name = "סוג המשרה")]
        [Required]
        public JobType JobType { get; set; }


        [Display(Name = "כתובת החברה")]
        [Required(ErrorMessage = "כתובת החברה זהו שדה חובה")]
        public string Address { get; set; }

        [Display(Name = "מספר עובדים הנדרשים למשרה")]
        //[Required]
        public int? NumEmployees { get; set; }

        [Display(Name = "שם איש הקשר")]
        [Required(ErrorMessage = "שם איש הקשר זהו שדה חובה")]
        public string ContactName { get; set; }
        
        [Display(Name = "מייל התקשרות")]
        [Required(ErrorMessage = "מייל התקשרות זהו שדה חובה")]
        [EmailAddress(ErrorMessage = "נא להזין כתובת מייל תקינה")]
        public string ContactMail { get; set; }
        
        [Display(Name = "טלפון התקשרות")]
        [Required(ErrorMessage = "טלפון התקשרות זהו שדה חובה")]
        [StringLength(10, MinimumLength = 9, ErrorMessage = "נא להזין מספר טלפון חוקי")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "נא להזין ספרות בלבד")]
        public string ContactPhone { get; set; }

        [Display(Name = "תפקיד איש הקשר")]
        [Required(ErrorMessage = "תפקיד איש הקשר זהו שדה חובה")]
        public string ContactJobPosition { get; set; }



    //    [Display(Name = "אשכול לימוד")]
     //   [Required] לא עשוי מפתח זר- לא יודעים איך לעשות!
      //  public AcademicCluster AcademicCluster { get; set; }

        [Display(Name = "סטטוס המשרה")]
        [Required]
        public JobStatus JobStatus { get; set; }


        public JobOfferModel(JobOffer entity)
            :base(entity)
        {

        }

        public JobOfferModel()
        {
            Id = 0;
            AcademicMajor = new AcademicMajorModel();
            AcademicMajor1 = new AcademicMajorModel();
            AcademicMajor2 = new AcademicMajorModel();
            JobArea = 0;
            JobTitle = "";
            CreationTime = DateTime.Now;
            UpdateTime = DateTime.Now;
            JobStatus = 0;
            ContactJobPosition = "";
            ContactMail = "";
            ContactName = "";
            ContactPhone = "";
            NumEmployees = 0;
            Address = "";
            Organization = "";
            Requirements = "";
            Description = "";
            RelevantMajorId1 = 0;
            RelevantMajorId2 = 0;
            RelevantMajorId3 = 0;

        }
    }
}