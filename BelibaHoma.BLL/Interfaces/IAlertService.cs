using BelibaHoma.BLL.Enums;
using Generic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BelibaHoma.BLL.Models;

namespace BelibaHoma.BLL.Interfaces
{
    public interface IAlertService
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

        /// <summary>
        /// Change Alert's status 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        StatusModel ChangeStatus(int id);

        /// <summary>
        /// Get active required intervention alerts from DB (all / by area)
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        StatusModel<List<AlertModel>> GetReportAlerts(Area? area);

        /// <summary>
        /// Get active grade alerts from DB (all / by area) 
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        StatusModel<List<AlertModel>> GetGradeAlerts(Area? area);

        /// <summary>
        /// Get active alerts 
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        StatusModel<List<AlertModel>> GetLateTutorAlerts(Area? area);
    }
}
