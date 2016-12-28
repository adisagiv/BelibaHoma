﻿using BelibaHoma.BLL.Enums;
using Generic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BelibaHoma.BLL.Models;

namespace BelibaHoma.BLL.Interfaces
{
    interface IAlertService
    {
        /// <summary>
        /// Add Alert to DB due to Trainee low grade
        /// </summary>
        /// <param name="traineeId"></param>
        /// <returns></returns>
        StatusModel AddTraineeGrade(int traineeId);

        /// <summary>
        /// Add alert to DB due to RequiredIntervention mark
        /// </summary>
        /// <param name="tutorReportId"></param>
        /// <returns></returns>
        StatusModel AddInervention(int tutorReportId);

        /// <summary>
        /// Add Tutor Late in filing reports alert to DB
        /// </summary>
        /// <param name="tutorId"></param>
        /// <returns></returns>
        StatusModel AddTutorLate(int tutorId);

        /// <summary>
        /// Add new alerts for Late tutors to DB (in all areas)
        /// </summary>
        /// <returns></returns>
        StatusModel GenerateLateTutorsAlerts();
    }
}
