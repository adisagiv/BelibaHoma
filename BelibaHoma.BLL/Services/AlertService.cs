using BelibaHoma.BLL.Enums;
using BelibaHoma.BLL.Interfaces;
using BelibaHoma.DAL;
using BelibaHoma.DAL.Interfaces;
using Catel.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                        var alert = new Alert
                        {
                            Status = (int) AlertStatus.New,
                            AlertType = (int) AlertType.TraineeGrade,
                            LinkedTraineeId = traineeId,
                            CreationTime = DateTime.Now,
                            UpdateTime = DateTime.Now,
                            Trainee = trainee
                        };

                        alertRepository.Add(alert);
                        unitOfWork.SaveChanges();

                        //If we got here - Yay! :)
                        status.Success = true;
                        status.Message = String.Format("ההתרעה עודכנה בהצלחה");
                        return status;
                    }
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
                            TutorReport = tutorReport
                        };

                        alertRepository.Add(alert);
                        unitOfWork.SaveChanges();

                        //If we got here - Yay! :)
                        status.Success = true;
                        status.Message = String.Format("ההתרעה עודכנה בהצלחה");
                        return status;
                    }
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
                        var alert = new Alert
                        {
                            Status = (int)AlertStatus.New,
                            AlertType = (int)AlertType.LateTutor,
                            LinkedTutorId = tutorId,
                            CreationTime = DateTime.Now,
                            UpdateTime = DateTime.Now,
                            Tutor = tutor
                        };

                        alertRepository.Add(alert);
                        unitOfWork.SaveChanges();

                        //If we got here - Yay! :)
                        status.Success = true;
                        status.Message = String.Format("ההתרעה עודכנה בהצלחה");
                        return status;
                    }
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
                    int Thresh = -20;
                    var DateThresh = DateTime.Now.AddDays(Thresh);
                    var tutorRepository = unitOfWork.GetRepository<ITutorRepository>();
                    var tutorList = tutorRepository.GetAll().ToList().Where(t => t.User.IsActive == true).ToList();

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
                        unitOfWork.SaveChanges();

                        status.Success = true;
                        status.Message = String.Format("מוסד הלימוד עודכן בהצלחה");
                    }
                }
            }
            catch (Exception ex)
            {
                status.Message = String.Format("שגיאה במהלך בסגירת התרעה");
                LogService.Logger.Error(status.Message, ex);
            }
            return status;
        }

        public StatusModel<List<AlertModel>> GetReportAlerts(Area? area)
        {
            var status = new StatusModel<List<AlertModel>>(false, String.Empty, new List<AlertModel>());
            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var alertRepository = unitOfWork.GetRepository<IAlertRepository>();

                   // status.Data = alertRepository.GetAll().ToList().Where(a => a.Status != (int)AlertStatus.Cloesd && a.AlertType == (int)AlertType.Intervention && (area == null || a.TutorReport.TutorTrainee.Tutor.User.Area == (int) area)).Select(a => new AlertModel(a)).OrderBy(a => a.UpdateTime).ToList();

                    var tutorRepository = unitOfWork.GetRepository<ITutorRepository>();
                    var tutors = tutorRepository.GetAll().Where(t => t.User.IsActive && (area == null || t.User.Area == (int?)area));
                    var alerts =
                        tutors.ToList().SelectMany(t => t.TutorTrainee)
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
            }
            catch (Exception ex)
            {
                status.Message = String.Format("שגיאה במהלך שליפת התרעות דורשות התערבות ממסד הנתונים");
                LogService.Logger.Error(status.Message, ex);
            }
            return status;
        }

        public StatusModel<List<AlertModel>> GetGradeAlerts(Area? area)
        {
            var status = new StatusModel<List<AlertModel>>(false, String.Empty, new List<AlertModel>());
            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var alertRepository = unitOfWork.GetRepository<IAlertRepository>();
                    status.Data = alertRepository.GetAll().Where(a => a.Status != (int)AlertStatus.Cloesd && a.AlertType == (int)AlertType.TraineeGrade && (area == null || a.Trainee.User.Area == (int?)area)).OrderBy(a => a.UpdateTime).ToList().Select(a => new AlertModel(a)).ToList();

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

        public StatusModel<List<AlertModel>> GetLateTutorAlerts(Area? area)
        {
            var status = new StatusModel<List<AlertModel>>(false, String.Empty, new List<AlertModel>());
            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var alertRepository = unitOfWork.GetRepository<IAlertRepository>();
                    status.Data = alertRepository.GetAll().Where(a => a.Status != (int)AlertStatus.Cloesd && a.AlertType == (int)AlertType.LateTutor && (area == null || a.Tutor.User.Area == (int?)area)).OrderBy(a => a.UpdateTime).ToList().Select(a => new AlertModel(a)).ToList();

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
    }
}
