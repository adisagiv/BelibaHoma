using System.ComponentModel.DataAnnotations;
using BelibaHoma.BLL.Enums;
using BelibaHoma.DAL;
using Generic.GenericModel.Models;
using Generic.Models;

namespace BelibaHoma.BLL.Models
{
    public class AcademicInstitutionModel : GenericModel
    {
        public int Id { get; set; }

        [Display(Name = "שם מוסד אקדמי")]
        [Required(ErrorMessage = "שם מוסד זה שדה חובה")]
        public string Name { get; set; }

        [Display(Name = "אזור פעילות")]
        [Required]
        public Area Area { get; set; }

        [Display(Name = "סוג מוסד אקדמי")]
        [Required]
        public InstitutionType InstitutionType { get; set; }


        public AcademicInstitutionModel(AcademicInstitution entity)
            :base(entity)
        {

        }

        public AcademicInstitutionModel()
        {

        }
    }
}
 