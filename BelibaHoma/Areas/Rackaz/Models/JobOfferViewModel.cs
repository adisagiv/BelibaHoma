using BelibaHoma.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BelibaHoma.Areas.Rackaz.Models
{
    public class JobOfferViewModel
    {
        public JobOfferModel JobOffer { get; set; }
        public List<AcademicMajorModel> AcademicMajors  { get; set; }

        public JobOfferViewModel()
        {
            JobOffer = new JobOfferModel();
            AcademicMajors = new List<AcademicMajorModel>();
        }
    }
}