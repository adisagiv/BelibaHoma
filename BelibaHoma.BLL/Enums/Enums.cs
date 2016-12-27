using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BelibaHoma.BLL.Enums
{
    // TODO: Add Display attribute like in area->north to all relevent enum
    public enum Area
    {
        [Description("צפון")]
        [Display(Name = "צפון")]
        North = 0,
        [Description("גוש דן")]
        [Display(Name = "גוש דן")]
        GushDan = 1,
        [Description("ירושלים")]
        [Display(Name = "ירושלים")]
        Jerusalem = 2,
        [Description("דרום")]
        [Display(Name = "דרום")]
        South = 3
    }    
    public enum InstitutionType
    {
        [Display(Name = "מכינה")]
        [Description("מכינה")]
        Mechina = 0,
        [Description("מכללה")]
        [Display(Name = "מכללה")]
        College = 1,
        [Description("אוניברסיטה")]
        [Display(Name = "אוניברסיטה")]
        University = 2
    }

    public enum UserRole
    {
        [Description("אדמין")]
        [Display(Name = "אדמין")]
        Admin = 0,
        [Description("רכז אזורי")]
        [Display(Name = "רכז אזורי")]
        Rackaz = 1,
        [Description("חונך")]
        [Display(Name = "חונך")]
        Tutor = 2,
        [Description("חניך")]
        [Display(Name = "חניך")]
        Trainee = 3 
    }

    public enum AcademicCluster
    {
        [Description("מקצועות ההנדסה ומדעים מדויקים")]
        [Display(Name = "מקצועות ההנדסה ומדעים מדויקים")]
        ExactScienceEngineering = 0,
        [Description("מקצועות טיפוליים")]
        [Display(Name = "מקצועות טיפוליים")]
        Therapeutic = 1,
        [Description("מקצועות פיננסיים")]
        [Display(Name = "מקצועות פיננסיים")]
        Financial = 2,
        [Description("מדעי החברה והרוח")]
        [Display(Name = "מדעי החברה והרוח")]
        SocialSciences = 3
    }

    public enum MaritalStatus
    {
        [Description("רווק/ה")]
        [Display(Name = "רווק/ה")]
        Single = 0,
        [Description("נשוי/אה")]
        [Display(Name = "נשוי/אה")]
        Married = 1,
        [Description("גרוש/ה")]
        [Display(Name = "גרוש/ה")]
        Divorced = 2,
        [Description("אלמן/ה")]
        [Display(Name = "אלמן/ה")]
        Widowed = 3
    }

    public enum EmploymentStatus
    {
        [Description("מובטל")]
        [Display(Name = "מובטל")]
        Unemployed = 0,
        [Description("מועסק אך לא במקצוע הלימוד")]
        [Display(Name = "מועסק אך לא במקצוע הלימוד")]
        TempJob = 1,
        [Description("מועסק במקצוע הלימוד")]
        [Display(Name = "מועסק במקצוע הלימוד")]
        Employed = 2
    }

    public enum LevelsCoreClasses
    {
        [Description("לא רלוונטי")]
        [Display(Name = "לא רלוונטי")]
        Irrelevant = 0,
        [Description("3 יחידות")]
        [Display(Name = "3 יחידות")]
        Three = 3,
        [Description("4 יחידות")]
        [Display(Name = "4 יחידות")]
        Four = 4,
        [Description("5 יחידות")]
        [Display(Name = "5 יחידות")]
        Five = 5
    }

    public enum Gender
    {
        [Description("זכר")]
        [Display(Name = "זכר")]
        Boy = 0,
        [Description("נקבה")]
        [Display(Name = "נקבה")]
        Girl = 1
    }

    public enum JobType
    {
        [Description("משרת סטודנט")]
        [Display(Name = "משרת סטודנט")]
        Student = 0,
        [Description("משרה חלקית")]
        [Display(Name = "משרה חלקית")]
        PartTime = 1,
        [Description("משרה מלאה")]
        [Display(Name = "משרה מלאה")]
        FullTime = 2,
        [Description("התמחות")]
        [Display(Name = "התמחות")]
        Internship = 3
    }

    public enum JobStatus
    {
        [Description("פעיל")]
        [Display(Name = "פעיל")]
        Active = 0,
        [Description("אוייש")]
        [Display(Name = "אוייש")]
        Taken = 1,
        [Description("לא רלוונטי")]
        [Display(Name = "לא רלוונטי")]
        NotRelevant = 2,
    }

    public enum JobArea
    {
        [Description("צפון")]
        [Display(Name = "צפון")]
        North = 0,
        [Description("חיפה והקריות")]
        [Display(Name = "חיפה והקריות")]
        HaifaAndKraiot = 1,
        [Description("השרון")]
        [Display(Name = "השרון")]
        Hasharon = 2,
        [Description("גוש דן")]
        [Display(Name = "גוש דן")]
        GushDan = 3,
        [Description("ירושלים וסביבתה")]
        [Display(Name = "ירושלים וסביבתה")]
        Jerusalem = 4,
        [Description("שומרון")]
        [Display(Name = "שומרון")]
        Shomron = 5,
        [Description("שפלה")]
        [Display(Name = "שפלה")]
        Shfela = 6,
        [Description("דרום")]
        [Display(Name = "דרום")]
        South = 7,
        [Description("אילת והערבה")]
        [Display(Name = "אילת והערבה")]
        EilatAndArava = 8,
        [Description("אחר")]
        [Display(Name = "אחר")]
        Other = 9
    }

    public enum TTStatus
    {
        [Description("פעיל")]
        [Display(Name = "פעיל")]
        Active = 0,
        [Description("לא פעיל")]
        [Display(Name = "לא פעיל")]
        InActive = 1,
        [Description("טרם אושר")]
        [Display(Name = "טרם אושר")]
        UnApproved = 2
    }
    public enum SemesterType
    {
        [Description("חורף")]
        [Display(Name = "חורף")]
        Winter = 0,
        [Description("אביב")]
        [Display(Name = "אביב")]
        Spring = 1,
        [Description("קיץ")]
        [Display(Name = "קיץ")]
        Summer = 2
    }

    public enum HourStatisticsType
    {
        Sum = 0,
        Average = 1
    }

    public enum AlertStatus
    {
        [Description("חדשה")]
        [Display(Name = "חדשה")]
        New = 0,
        [Description("בטיפול")]
        [Display(Name = "בטיפול")]
        Ongoing = 1,
        [Description("סגורה")]
        [Display(Name = "סגורה")]
        Cloesd = 2
    }
}
