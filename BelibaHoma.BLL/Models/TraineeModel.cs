using System;
using System.ComponentModel.DataAnnotations;
using BelibaHoma.BLL.Enums;
using BelibaHoma.DAL;
using Generic.Models;
using log4net;
using Ninject.Planning.Bindings;

namespace BelibaHoma.BLL.Models
{
    public class TraineeModel : GenericModel<TraineeModel>
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
        [Required(ErrorMessage = "זהו שדה חובה")]
        public System.DateTime Birthday { get; set; }

        [Display(Name = "שנה בתואר מבחינת התקדמות בקורסים")]
        [Required(ErrorMessage = "זהו שדה חובה")]
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
        [Required(ErrorMessage = "זהו שדה חובה")]
        public EmploymentStatus EmploymentStatus { get; set; }
        
        [Display(Name = "שעות חברותא (השנה)")]
        public int TutorHoursBonding { get; set; }
        
        [Display(Name = "שעות חניכה (השנה)")]
        public int TutorHours { get; set; }
        
        [Display(Name = "מסלול לימוד נוסף בו נדרשת חניכה")]
        public AcademicMajorModel AcademicMajor2 { get; set; }
        
        [Display(Name = "תיאור מילולי למקצוע בו נדרשת עזרה")]
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
        
        [Display(Name = "מספר סמסטרים מתחילת התואר (ותק)")]
        [Required(ErrorMessage = "זהו שדה חובה")]
        public int SemesterNumber { get; set; }
        
        [Display(Name = "משתמש")]
        [Required]
        public UserModel User { get; set; }

        [Display(Name = "שם מלא")]
        public string FullName { get { return String.Format("{0}, {1}", User.LastName, User.FirstName); } }

        //TODO: WHAAAAT? understand what are these 2 and why?
        //public ICollection<Grade> Grade { get; set; }
        //public ICollection<TutorTrainee> TutorTrainee { get; set; }

        //[Required(ErrorMessage = "שם פרטי זהו שדה חובה")]
        //public string FirstName { get; set; }
        //[Display(Name = "שם משפחה")]
        //[Required(ErrorMessage = "שם משפחה זהו שדה חובה")]
        //public string LastName { get; set; }
        //[Display(Name = "סיסמא")]
        //[Required(ErrorMessage = "נא להזין סיסמא")]
        //[DataType(DataType.Password)]
        //[MinLength(6)]
        //[MaxLength(20)]
        //public string Password { get; set; }
        //[Display(Name = "זמן יצירה")]
        //public DateTime CreationTime { get; set; }
        //[Display(Name = "סוג משתמש")]
        //[Required(ErrorMessage = "נא לבחור את סוג המשתמש")]
        //public UserRole UserRole { get; set; }
        //[Display(Name = "זמן עדכון")]
        //public DateTime UpdateTime { get; set; }
        //[Display(Name = "כתובת מייל")]
        //[EmailAddress(ErrorMessage = "נא להזין כתובת מייל תקינה")]
        //public string Email { get; set; }
        //[Display(Name = "משתמש פעיל")]
        //public bool IsActive { get; set; }
        //[Display(Name = "אזור פעילות")]
        //public Area? Area { get; set; }

        public TraineeModel(Trainee entity)
            :base(entity)
        {

        }

        public TraineeModel()
        {

        }
    }
}

