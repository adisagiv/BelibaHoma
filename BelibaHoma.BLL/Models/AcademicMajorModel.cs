using System.ComponentModel.DataAnnotations;
using BelibaHoma.BLL.Enums;
using BelibaHoma.DAL;
using Generic.Models;

namespace BelibaHoma.BLL.Models
{
    public class AcademicMajorModel : GenericModel<AcademicMajorModel>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "שם מסלול הלימודים זהו שדה חובה")]
        public string Name { get; set; }

        [Required]
        public AcademicCluster Cluster { get; set; }

        public AcademicMajorModel(AcademicMajor entity)
            :base(entity)
        {

        }

        public AcademicMajorModel()
        {

        }
    }
}