using System.ComponentModel.DataAnnotations;
using BelibaHoma.BLL.Enums;
using BelibaHoma.DAL;
using Generic.GenericModel.Models;

namespace BelibaHoma.BLL.Models
{
    public class AcademicMajorModel : GenericModel<AcademicMajorModel>
    {
        public int Id { get; set; }

        [Display(Name = "שם המסלול")]
        [Required(ErrorMessage = "שם מסלול הלימודים זהו שדה חובה")]
        public string Name { get; set; }

        [Display(Name = "אשכול לימוד")]
        [Required]
        public AcademicCluster AcademicCluster { get; set; }

        public AcademicMajorModel(AcademicMajor entity)
            :base(entity)
        {

        }

        public AcademicMajorModel()
        {

        }
    }
}