using BelibaHoma.BLL.Enums;
using BelibaHoma.DAL;
using Generic.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace BelibaHoma.BLL.Interfaces
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
 