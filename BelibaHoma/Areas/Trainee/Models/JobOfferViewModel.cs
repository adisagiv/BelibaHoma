using BelibaHoma.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BelibaHoma.Areas.Trainee.Models
{
    public class JobOfferViewModel
    {
        public JobOfferModel JobOffer { get; set; }
        public List<AcademicMajorModel> AcademicMajors  { get; set; }
        public bool IsTrainee { get; set; }

        public JobOfferViewModel()
        {
            JobOffer = new JobOfferModel();
            IsTrainee = true;
            AcademicMajors = new List<AcademicMajorModel>();
        }
    }
}