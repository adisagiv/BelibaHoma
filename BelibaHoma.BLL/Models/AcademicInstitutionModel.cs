using System.ComponentModel.DataAnnotations;
using BelibaHoma.BLL.Enums;
using BelibaHoma.DAL;
using Generic.Models;

namespace BelibaHoma.BLL.Models
{
    public class AcademicInstitutionModel : GenericModel<AcademicInstitutionModel>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "שם מוסד זה שדה חובה")]
        public string Name { get; set; }

        [Required]
        public Area Area { get; set; }

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
 