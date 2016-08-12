using BelibaHoma.BLL.Enums;
using BelibaHoma.DAL;
using Generic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BelibaHoma.BLL.Interfaces
{
    public class AcademicInstitutionModel : GenericModel<AcademicInstitutionModel>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Area Area { get; set; }
        public InstitutionType InstitutionType { get; set; }


        public AcademicInstitutionModel(AcademicInstitution entity)
            :base(entity)
        {

        }
    }
}
 