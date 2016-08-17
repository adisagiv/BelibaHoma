using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BelibaHoma.BLL.Enums;
using BelibaHoma.BLL.Models;

namespace BelibaHoma.Areas.Rackaz.Models
{
    public class AcademicInstitutionVM
    {
        public Area Area { get; set; }
        public AcademicInstitutionModel AcademicInstitution { get; set; }
    }
}