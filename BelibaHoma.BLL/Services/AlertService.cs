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
    class AlertService : IAlertService
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
                            .Any(a => a.LinkedTraineeId == traineeId && a.Status != (int) AlertStatus.Cloesd);
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
                            .Any(a => a.LinkedReportId == tutorReportId && a.Status != (int)AlertStatus.Cloesd);
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
                            .Any(a => a.LinkedTutorId == tutorId && a.Status != (int)AlertStatus.Cloesd);
                    if (!checkExistingAlert)
                    {
                        var alert = new Alert
                        {
                            Status = (int)AlertStatus.New,
                            AlertType = (int)AlertType.Intervention,
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
    }
}
