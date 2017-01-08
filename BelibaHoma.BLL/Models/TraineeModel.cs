using System;
using System.ComponentModel.DataAnnotations;
using BelibaHoma.BLL.Enums;
using BelibaHoma.DAL;
using Generic.GenericModel.Models;
using Generic.Models;
using log4net;
using Ninject.Planning.Bindings;

namespace BelibaHoma.BLL.Models
{
    public class TraineeModel : GenericModel
    {
        public int UserId { get; set; }


        //[StringLength(9, MinimumLength = 9, ErrorMessage = "נא להזין ת.ז. כולל ספרת ביקורת")]

        [Display(Name = "כתובת מגורים")]
        public string Address { get; set; }

        [Required(ErrorMessage = "זהו שדה חובה")]
        [Display(Name = "מגדר")]
        public Gender Gender { get; set; }

        [Display(Name = "מוסד לימוד")]
        [Required(ErrorMessage = "זהו שדה חובה")]
        public AcademicInstitutionModel AcademicInstitution { get; set; }

        [Display(Name = "תאריך לידה")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "זהו שדה חובה")]
        public System.DateTime Birthday { get; set; }

        [Display(Name = "שנה בתואר (מבחינת ותק)")]
        [Required(ErrorMessage = "זהו שדה חובה")]
        [Range(0, 8, ErrorMessage = "Can only be between 0 .. 8")]
        //[StringLength(2, MinimumLength = 1, ErrorMessage = "נא להזין מספר תקין)")]
        //[RegularExpression("^[0-9]*$", ErrorMessage = "נא להזין ספרות בלבד")]
        public int AcademicYear { get; set; }

        [Display(Name = "מסלול לימוד ראשי")]
        [Required(ErrorMessage = "זהו שדה חובה")]
        public AcademicMajorModel AcademicMajor { get; set; }

        [Display(Name = "מסלול לימוד משני")]
        public AcademicMajorModel AcademicMajor1 { get; set; }

        [Display(Name = "מספר טלפון")]
        [Required(ErrorMessage = "זהו שדה חובה")]
        [StringLength(10, MinimumLength = 9, ErrorMessage = "נא להזין מספר טלפון תקין וכולל קידומת)")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "נא להזין ספרות בלבד")]
        public string PhoneNumber { get; set; }
        
        [Display(Name = "סטטוס משפחתי")]
        [Required(ErrorMessage = "זהו שדה חובה")]
        public MaritalStatus MaritalStatus { get; set; }
        
        [Display(Name = "סטטוס תעסוקתי")]
        public EmploymentStatus? EmploymentStatus { get; set; }
        
        [Display(Name = "שעות חברותא (השנה)")]
        public double TutorHoursBonding { get; set; }
        
        [Display(Name = "שעות חניכה (השנה)")]
        public double TutorHours { get; set; }
        
        [Display(Name = "מסלול לימוד נוסף בו נדרשת חניכה")]
        public AcademicMajorModel AcademicMajor2 { get; set; }
        
        [Display(Name = "תיאור העזרה הנדרשת")]
        [Required(ErrorMessage = "זהו שדה חובה")]
        public string NeededHelpDescription { get; set; }
        
        [Display(Name = "רמת חניכה נדרשת בפיזיקה")]
        [Required(ErrorMessage = "זהו שדה חובה")]
        public LevelsCoreClasses PhysicsLevel { get; set; }
        
        [Display(Name = "רמת חניכה נדרשת במתמטיקה")]
        [Required(ErrorMessage = "זהו שדה חובה")]
        public LevelsCoreClasses MathLevel { get; set; }
        
        [Display(Name = "רמת חניכה נדרשת באנגלית")]
        [Required(ErrorMessage = "זהו שדה חובה")]
        public LevelsCoreClasses EnglishLevel { get; set; }

        [Display(Name = "מספר סמסטרים מבחינת התקדמות בקורסים")]
        [Required(ErrorMessage = "זהו שדה חובה")]
        [Range(0, 16, ErrorMessage = "Can only be between 0 .. 16")]
        //[StringLength(2, MinimumLength = 1, ErrorMessage = "נא להזין מספר תקין)")]
        //[RegularExpression("^[0-9]*$", ErrorMessage = "נא להזין ספרות בלבד")]
        public int SemesterNumber { get; set; }
        
        [Display(Name = "משתמש")]
        [Required]
        public UserModel User { get; set; }

        [Display(Name = "האם נשר")]
        public bool DroppedOut { get; set; }

        [Display(Name = "שם מלא")]
        public string FullName { get { return String.Format("{0}, {1}", User.LastName, User.FirstName); } }

        //TODO: WHAAAAT? understand what are these 2 and why?
        //public ICollection<Grade> Grade { get; set; }
        //public ICollection<TutorTrainee> TutorTrainee { get; set; }

        public double Pasam { get; set; }
        public DateTime LastPasamCalculation { get; set; }

        public double PazamInYears
        {
            get { return Pasam/365.0; }
        }

        public TraineeModel(Trainee entity)
            :base(entity)
        {

        }

        public TraineeModel()
        {

        }
    }
}

