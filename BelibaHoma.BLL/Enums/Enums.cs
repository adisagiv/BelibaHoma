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
        [Description("אוניברסיטה")]
        [Display(Name = "אוניברסיטה")]
        University = 1,
        [Description("מכללה")]
        [Display(Name = "מכללה")]
        Collage = 2
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
}
