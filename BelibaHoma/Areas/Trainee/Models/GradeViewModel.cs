using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BelibaHoma.BLL.Models;

namespace BelibaHoma.Areas.Trainee.Models
{
    public class GradeViewModel
    {
        public List<GradeModel> Grades { get; set; }
        public TraineeModel Trainee { get; set; }
    }
}