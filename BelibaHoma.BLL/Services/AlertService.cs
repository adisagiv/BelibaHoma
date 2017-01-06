using BelibaHoma.BLL.Enums;
using BelibaHoma.BLL.Interfaces;
using BelibaHoma.DAL;
using BelibaHoma.DAL.Interfaces;
using Catel.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using BelibaHoma.BLL.Models;
using Generic.Models;
using Services.Log;

namespace BelibaHoma.BLL.Services
{
    public class AlertService : IAlertService
    {
        public StatusModel AddTraineeGrade(int traineeId)
        {
            var status = new StatusModel(false, String.Empty);

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var traineeRepository = unitOfWork.GetRepository<ITraineeRepository>();
                    var trainee = traineeRepository.GetByKey(traineeId);
                    
                    var alertRepository = unitOfWork.GetRepository<IAlertRepository>();
                    var checkExistingAlert =
                        alertRepository.GetAll()
                            .ToList()
                            .Any(a => a.AlertType == (int) AlertType.TraineeGrade && a.LinkedTraineeId == traineeId && a.Status != (int) AlertStatus.Cloesd);
                    if (!checkExistingAlert)
                    {
                        if (trainee.User.Area != null)
                        {
                            var alert = new Alert
                            {
                                Status = (int) AlertStatus.New,
                                AlertType = (int) AlertType.TraineeGrade,
                                LinkedTraineeId = traineeId,
                                CreationTime = DateTime.Now,
                                UpdateTime = DateTime.Now,
                                Trainee = trainee,
                                Area = (int)trainee.User.Area
                            };

                            alertRepository.Add(alert);
                        }
                        unitOfWork.SaveChanges();
                    }
                    //If we got here - Yay! :)
                    status.Success = true;
                    status.Message = String.Format("ההתרעה עודכנה בהצלחה");
                    return status;
                }
            }
            catch (Exception ex)
            {
                if (status.Message == String.Empty)
                {
                    status.Message = String.Format("שגיאה במהלך הוספת ההתרעה עבור ציון חניך");
                }
                LogService.Logger.Error(status.Message, ex);
            }
            return status;
        }

        public StatusModel AddInervention(int tutorReportId)
        {
            var status = new StatusModel(false, String.Empty);

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var tutorReportRepository = unitOfWork.GetRepository<ITutorReportRepository>();
                    var tutorReport = tutorReportRepository.GetByKey(tutorReportId);

                    var alertRepository = unitOfWork.GetRepository<IAlertRepository>();
                    var checkExistingAlert =
                        alertRepository.GetAll()
                            .ToList()
                            .Any(a => a.AlertType == (int)AlertType.Intervention && a.LinkedReportId == tutorReportId && a.Status != (int)AlertStatus.Cloesd);
                    if (!checkExistingAlert)
                    {
                        var alert = new Alert
                        {
                            Status = (int)AlertStatus.New,
                            AlertType = (int)AlertType.Intervention,
                            LinkedReportId = tutorReportId,
                            CreationTime = DateTime.Now,
                            UpdateTime = DateTime.Now,
                            TutorReport = tutorReport,
                            Area = (int)tutorReport.TutorTrainee.Trainee.User.Area
                        };

                        alertRepository.Add(alert);
                        unitOfWork.SaveChanges();
                    }
                    //If we got here - Yay! :)
                    status.Success = true;
                    status.Message = String.Format("ההתרעה עודכנה בהצלחה");
                    return status;
                }
            }
            catch (Exception ex)
            {
                if (status.Message == String.Empty)
                {
                    status.Message = String.Format("שגיאה במהלך הוספת ההתרעה עבור דרושה התערבות");
                }
                LogService.Logger.Error(status.Message, ex);
            }
            return status;
        }

        public StatusModel AddTutorLate(int tutorId)
        {
            var status = new StatusModel(false, String.Empty);

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var tutorRepository = unitOfWork.GetRepository<ITutorRepository>();
                    var tutor = tutorRepository.GetByKey(tutorId);

                    var alertRepository = unitOfWork.GetRepository<IAlertRepository>();
                    var checkExistingAlert =
                        alertRepository.GetAll()
                            .ToList()
                            .Any(a => a.AlertType == (int)AlertType.LateTutor && a.LinkedTutorId == tutorId && a.Status != (int)AlertStatus.Cloesd);
                    if (!checkExistingAlert)
                    {
                        if (tutor.User.Area != null)
                        {
                            var alert = new Alert
                            {
                                Status = (int)AlertStatus.New,
                                AlertType = (int)AlertType.LateTutor,
                                LinkedTutorId = tutorId,
                                CreationTime = DateTime.Now,
                                UpdateTime = DateTime.Now,
                                Tutor = tutor,
                                Area = (int)tutor.User.Area
                            };

                            alertRepository.Add(alert);
                        }
                        unitOfWork.SaveChanges();
                    }
                    //If we got here - Yay! :)
                    status.Success = true;
                    status.Message = String.Format("ההתרעה עודכנה בהצלחה");
                    return status;
                }
            }
            catch (Exception ex)
            {
                if (status.Message == String.Empty)
                {
                    status.Message = String.Format("שגיאה במהלך הוספת ההתרעה עבור חונך מאחר בדיווח");
                }
                LogService.Logger.Error(status.Message, ex);
            }
            return status;
        }

        public StatusModel GenerateLateTutorsAlerts()
        {
            var status = new StatusModel(false, String.Empty);

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    int Thresh = -15;
                    var DateThresh = DateTime.Now.AddDays(Thresh);
                    var tutorRepository = unitOfWork.GetRepository<ITutorRepository>();
                    var tutorList = tutorRepository.GetAll().ToList().Where(t => t.User.IsActive == true && t.TutorTrainee.Any(tt => tt.Status == (int)TTStatus.Active)).ToList();

                    var tutorReportRepository = unitOfWork.GetRepository<ITutorReportRepository>();
                    foreach (var tutor in tutorList)
                    {
                        bool isNotLate =
                            tutorReportRepository.GetAll()
                                .ToList()
                                .Any(tr => tr.TutorTrainee.TutorId == tutor.UserId && tr.CreationTime > DateThresh);
                        if (isNotLate == false)
                        {
                            status = AddTutorLate(tutor.UserId);
                            if (!status.Success)
                            {
                                throw new Exception(status.Message);
                            }
                        }
                    }
                        //If we got here - Yay! :)
                        status.Success = true;
                        status.Message = String.Format("ההתרעות לאיחור בדיווח עודכנו בהצלחה");
                        return status;
                }
            }
            catch (Exception ex)
            {
                if (status.Message == String.Empty)
                {
                    status.Message = String.Format("שגיאה במהלך הוספת ההתרעה עבור חונך מאחר בדיווח");
                }
                LogService.Logger.Error(status.Message, ex);
            }
            return status;
        }

        public StatusModel ChangeStatus(int id)
        {
            var status = new StatusModel(false, String.Empty);

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var alertRepository = unitOfWork.GetRepository<IAlertRepository>();

                    var alert = alertRepository.GetByKey(id);
                    if (alert != null)
                    {
                        if (alert.Status == (int) AlertStatus.New)
                        {
                            alert.Status = (int) AlertStatus.Ongoing;
                        }
                        else if (alert.Status == (int)AlertStatus.Ongoing)
                        {
                            alert.Status = (int) AlertStatus.Cloesd;
                        }
                        
                        unitOfWork.SaveChanges();

                        status.Success = true;
                        status.Message = String.Format("סטטוס ההתרעה עודכן בהצלחה ");
                    }
                }
            }
            catch (Exception ex)
            {
                status.Message = String.Format("שגיאה במהלך שינוי סטטוס ההתרעה");
                LogService.Logger.Error(status.Message, ex);
            }
            return status;
        }

        public StatusModel<List<AlertModel>> GetReportAlerts(Area? area, bool archive)
        {
            var status = new StatusModel<List<AlertModel>>(false, String.Empty, new List<AlertModel>());
            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var alertRepository = unitOfWork.GetRepository<IAlertRepository>();

                   // status.Data = alertRepository.GetAll().ToList().Where(a => a.Status != (int)AlertStatus.Cloesd && a.AlertType == (int)AlertType.Intervention && (area == null || a.TutorReport.TutorTrainee.Tutor.User.Area == (int) area)).Select(a => new AlertModel(a)).OrderBy(a => a.UpdateTime).ToList();
                    if (archive == false)
                    {
                        var tutorRepository = unitOfWork.GetRepository<ITutorRepository>();
                        var tutors = tutorRepository.GetAll().Where(t => t.User.IsActive && (area == null || t.User.Area == (int?)area));
                        var alerts =
                            tutors.ToList().SelectMany(t => t.TutorTrainee.Where(tt => tt.Status == (int)TTStatus.Active))
                                .SelectMany(tt => tt.TutorReport)
                                .SelectMany(tr => tr.Alert)
                                .Where(
                                    a => a.Status != (int)AlertStatus.Cloesd && a.AlertType == (int)AlertType.Intervention)
                                .OrderBy(a => a.UpdateTime)
                                .ToList()
                                .Select(a => new AlertModel(a)).ToList();
                        status.Data = alerts;
                        //If we got here - Yay!!
                        status.Success = true;
                    }
                    else
                    {
                        var tutorRepository = unitOfWork.GetRepository<ITutorRepository>();
                        var tutors = tutorRepository.GetAll().Where(t => area == null || t.User.Area == (int?)area);
                        var alerts =
                            tutors.ToList().SelectMany(t => t.TutorTrainee)
                                .SelectMany(tt => tt.TutorReport)
                                .SelectMany(tr => tr.Alert)
                                .Where(
                                    a => a.Status == (int)AlertStatus.Cloesd && a.AlertType == (int)AlertType.Intervention)
                                .OrderBy(a => a.UpdateTime)
                                .ToList()
                                .Select(a => new AlertModel(a)).ToList();
                        status.Data = alerts;
                        //If we got here - Yay!!
                        status.Success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                status.Message = String.Format("שגיאה במהלך שליפת התרעות דורשות התערבות ממסד הנתונים");
                LogService.Logger.Error(status.Message, ex);
            }
            return status;
        }

        public StatusModel<List<AlertModel>> GetGradeAlerts(Area? area, bool archive)
        {
            var status = new StatusModel<List<AlertModel>>(false, String.Empty, new List<AlertModel>());
            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var alertRepository = unitOfWork.GetRepository<IAlertRepository>();
                    if (archive == false)
                    {
                        status.Data = alertRepository.GetAll().Where(a => a.Status != (int)AlertStatus.Cloesd && a.AlertType == (int)AlertType.TraineeGrade && (area == null || a.Area == (int?)area)).OrderBy(a => a.UpdateTime).ToList().Select(a => new AlertModel(a)).ToList();    
                    }
                    else
                    {
                        status.Data = alertRepository.GetAll().Where(a => a.Status == (int)AlertStatus.Cloesd && a.AlertType == (int)AlertType.TraineeGrade && (area == null || a.Area == (int?)area)).OrderBy(a => a.UpdateTime).ToList().Select(a => new AlertModel(a)).ToList();
                    }

                    //If we got here - Yay!!
                    status.Success = true;
                }
            }
            catch (Exception ex)
            {
                status.Message = String.Format("שגיאה במהלך שליפת התרעות עבור ציוני חניכים ממסד הנתונים");
                LogService.Logger.Error(status.Message, ex);
            }
            return status;
        }

        public StatusModel<List<AlertModel>> GetLateTutorAlerts(Area? area, bool archive)
        {
            var status = new StatusModel<List<AlertModel>>(false, String.Empty, new List<AlertModel>());
            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var alertRepository = unitOfWork.GetRepository<IAlertRepository>();

                    if (archive == false)
                    {
                        status.Data = alertRepository.GetAll().Where(a => a.Status != (int)AlertStatus.Cloesd && a.AlertType == (int)AlertType.LateTutor && (area == null || a.Area == (int?)area)).OrderBy(a => a.Status).ThenBy(a => a.UpdateTime).ToList().Select(a => new AlertModel(a)).ToList();

                        foreach (var alert in status.Data)
                        {
                            DateTime? lastReportTime = null;
                            if (alert.Tutor != null)
                            {
                                var tutorRepository = unitOfWork.GetRepository<ITutorRepository>();
                                var tutorId = alert.Tutor.UserId;
                                var tutor =
                                    tutorRepository.FirstOrDefault(t => t.UserId == tutorId);
                                var tutorTrainee = tutor.TutorTrainee;
                                var creationTimes = tutorTrainee.SelectMany(
                                                tt => tt.TutorReport.Select(tr => tr.CreationTime));
                                if (creationTimes.Count() > 0)
                                {
                                    lastReportTime = creationTimes.OrderByDescending(ct => ct).FirstOrDefault();
                                }
                            }
                            alert.LastReportTime = lastReportTime;
                        }
                    }
                    else
                    {
                        status.Data = alertRepository.GetAll().Where(a => a.Status == (int)AlertStatus.Cloesd && a.AlertType == (int)AlertType.LateTutor && (area == null || a.Area == (int?)area)).OrderBy(a => a.Status).ThenBy(a => a.UpdateTime).ToList().Select(a => new AlertModel(a)).ToList();
                    }
                    
                    //If we got here - Yay!!
                    status.Success = true;
                }
            }
            catch (Exception ex)
            {
                status.Message = String.Format("שגיאה במהלך שליפת התרעות עבור חונכים המאחרים בדיווח ממסד הנתונים");
                LogService.Logger.Error(status.Message, ex);
            }
            return status;
        }

        public StatusModel<int[]> GetAlertStatusCounts(Area? area)
        {
            var status = new StatusModel<int[]>(false, String.Empty, new int[2]);
            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var alertRepository = unitOfWork.GetRepository<IAlertRepository>();
                    int newCount = alertRepository.GetAll().Count(a => (area == null || a.Area == (int?)area) && a.Status == (int)AlertStatus.New);
                    int onGoingCount = alertRepository.GetAll().Count(a => (area == null || a.Area == (int?)area) && a.Status == (int)AlertStatus.Ongoing);

                    status.Data[0] = newCount;
                    status.Data[1] = onGoingCount;

                    //If we got here - Yay!!
                    status.Success = true;
                }
            }
            catch (Exception ex)
            {
                status.Message = String.Format("שגיאה במהלך שליפת מספרי ההתרעות ממסד הנתונים");
                LogService.Logger.Error(status.Message, ex);
            }
            return status;
        }

        public StatusModel<AlertModel> Get(int alertId)
        {
            var status = new StatusModel<AlertModel>(false, String.Empty, null);
            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var alertRepository = unitOfWork.GetRepository<IAlertRepository>();
                    var alert = alertRepository.GetByKey(alertId);

                    status.Data = new AlertModel(alert);

                    //If we got here - Yay!!
                    status.Success = true;
                }
            }
            catch (Exception ex)
            {
                status.Message = String.Format("שגיאה במהלך שליפת ההתרעה ממסד הנתונים");
                LogService.Logger.Error(status.Message, ex);
            }
            return status;
        }

        public StatusModel SaveAlertNotes(int alertId, string notes)
        {
            var status = new StatusModel(false, String.Empty);
            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var alertRepository = unitOfWork.GetRepository<IAlertRepository>();
                    var alert = alertRepository.GetByKey(alertId);

                    if (alert.Status != (int) AlertStatus.Cloesd)
                    {
                        alert.Notes = notes;
                        alert.Status = (int)AlertStatus.Ongoing;
                        alert.UpdateTime = DateTime.Now;

                        unitOfWork.SaveChanges();
                    }
                    //If we got here - Yay!!
                    status.Success = true;
                }
            }
            catch (Exception ex)
            {
                status.Message = String.Format("שגיאה במהלך שמירת ההתרעה במסד הנתונים");
                LogService.Logger.Error(status.Message, ex);
            }
            return status;
        }
    }
}
