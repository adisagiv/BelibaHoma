﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BelibaHoma.BLL.Enums;
using BelibaHoma.BLL.Models;
using BelibaHoma.DAL;
using Generic.Models;
using log4net;
using Ninject.Planning.Bindings;

namespace BelibaHoma.Areas.Rackaz.Models
{
    public class TraineeViewModel
    {
        public TraineeModel Trainee;
        public List<AcademicInstitutionModel> AcademicInstitutionList;
        public List<AcademicMajorModel> AcademicMajorList;

        public TraineeViewModel()
        {

        }
    }
}