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
    public class TutorSessionService : ITutorSessionService
    {

        /// <summary>
        /// Get all job offers from the db
        /// </summary>
        /// <returns></returns>
        public StatusModel<List<TutorSessionModel>> Get()
        {
            var result = new StatusModel<List<TutorSessionModel>>(false, String.Empty, new List<TutorSessionModel>());

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var TutorSessionRepository = unitOfWork.GetRepository<ITutorSessionRepository>();

                    result.Data = TutorSessionRepository.GetAll().ToList().Select(ai => new TutorSessionModel(ai)).ToList();

                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                result.Message = String.Format("Error getting Tutor Sessions from DB");
                LogService.Logger.Error(result.Message, ex);
            }
            return result;
        }


        public StatusModel Add(TutorSessionModel model)
        {
            var status = new StatusModel(false, String.Empty);

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                   
                    var TutorSessionRepository = unitOfWork.GetRepository<ITutorSessionRepository>();
                    var entity = model.MapTo<TutorSession>();

                    //Retrieving Related Entities by using the repositories and GetById function (all but User which was not yet created)

                   var TutorReportRepository = unitOfWork.GetRepository<ITutorReportRepository>();
                   var TutorReport = TutorReportRepository.GetByKey(model.TutorReportId);

                    //Server side validations
                   if (entity.StartTime > entity.EndTime)
                   {
                       status.Message = String.Format("זמן תחילת המפגש חייב להיות לפני זמן סיום המפגש");
                       throw new System.ArgumentException(status.Message, "model");

                   }
                   var meetingDuration = entity.EndTime - entity.StartTime;
                   double meetingDurationDouble = (meetingDuration.Hours + meetingDuration.Minutes / 100.0 + meetingDuration.Seconds / 10000.0) * (meetingDuration > TimeSpan.Zero ? 1 : -1);
                   if (entity.NumBondingHours > meetingDurationDouble)
                   {
                       status.Message = String.Format("מספר שעות חברותא לא יכול להיות גדול מזמן המפגש");
                       throw new System.ArgumentException(status.Message, "model");

                   }
                   if (TutorReport.CreationTime > model.MeetingDate.AddDays(21))
                    {
                        status.Message = "לא ניתן להזין מפגש שהתרחש יותר מ-3 שבועות לפני תאריך הדיווח.\nאנא פנה אל הרכז האזורי לעזרה.";
                        throw new System.ArgumentException(status.Message, "model");
                    }



                    //Linking the Complexed entities to the retrieved ones
                    entity.TutorReport = TutorReport;
                    var TutorHoursTemp = entity.EndTime - entity.StartTime;
                    var Tht = TutorHoursTemp.TotalHours;
                    entity.TutorReport.TutorHours += Tht; ///adding the total tutoring hours to tutorReport 
                    entity.TutorReport.TutorTrainee.Trainee.TutorHours += Tht; ///adding the total tutoring hours to trainee
                    entity.TutorReport.TutorTrainee.Tutor.TutorHours += Tht; ///adding the total tutoring hours to tutor


                    entity.TutorReport.TutorHoursBonding += entity.NumBondingHours;//adding the total bonding hour to tutorReport
                    entity.TutorReport.TutorTrainee.Trainee.TutorHoursBonding += entity.NumBondingHours;//adding the total bonding hour to trainee
                    entity.TutorReport.TutorTrainee.Tutor.TutorHoursBonding += entity.NumBondingHours;//adding the total bonding hour to tutor


                    
                    //entity.relevantmajor= מה ששמרתי מהרפוסיטורים
                    TutorSessionRepository.Add(entity);

                    unitOfWork.SaveChanges();

                    status.Success = true;
                    status.Message = String.Format("המפגש בתאריך {0} הוזן בהצלחה", model.MeetingDate);
                }
            }
            catch (Exception ex)
            {
                if (status.Message == String.Empty)
                {
                status.Message = String.Format("שגיאה במהלך הזנת המפגש");
                }
                LogService.Logger.Error(status.Message, ex);
            }

            return status;
        }


        public StatusModel Update(int id, TutorSessionModel updatedModel)
        {
            var status = new StatusModel(false, String.Empty);

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var TutorSessionRepository = unitOfWork.GetRepository<ITutorSessionRepository>();
                    var entity = updatedModel.MapTo<TutorSession>();

                    var TutorSession = TutorSessionRepository.GetByKey(id);
                    if (TutorSession != null)
                    {
                        var OldTutorHoursTemp = TutorSession.StartTime - TutorSession.EndTime;
                        var OldTht = OldTutorHoursTemp.TotalHours;
                        var OldBondingHours = TutorSession.NumBondingHours;

                        TutorSession.MeetingDate = updatedModel.MeetingDate;
                        TutorSession.StartTime = updatedModel.StartTime;
                        TutorSession.EndTime = updatedModel.EndTime;
                        TutorSession.NumBondingHours = updatedModel.NumBondingHours;
                        TutorSession.TutorReportId = updatedModel.TutorReportId; //problem with FK
                        TutorSession.MeetingPlace = updatedModel.MeetingPlace; //problem with FK

                        var TutorReportRepository = unitOfWork.GetRepository<ITutorReportRepository>();
                        var TutorReport = TutorReportRepository.GetByKey(updatedModel.TutorReportId);

                        entity.TutorReport = TutorReport;

                        //Server side validations
                        if (entity.StartTime > entity.EndTime)
                        {
                            status.Message = String.Format("זמן תחילת המפגש חייב להיות לפני זמן סיום המפגש");
                            throw new System.ArgumentException(status.Message, "model");

                        }
                        var meetingDuration = entity.EndTime - entity.StartTime;
                        double meetingDurationDouble = (meetingDuration.Hours + meetingDuration.Minutes / 100.0 + meetingDuration.Seconds / 10000.0) * (meetingDuration > TimeSpan.Zero ? 1 : -1);

                        if (entity.NumBondingHours > meetingDurationDouble)
                        {
                            status.Message = String.Format("מספר שעות חברותא לא יכול להיות גדול מזמן המפגש");
                            throw new System.ArgumentException(status.Message, "model");

                        }
                        if (TutorReport.CreationTime > updatedModel.MeetingDate.AddDays(21))
                        {
                            status.Message = "לא ניתן להזין מפגש שהתרחש יותר מ-3 שבועות לפני תאריך יצירת הדיווח.\nאנא פנה אל הרכז האזורי לעזרה.";
                            throw new System.ArgumentException(status.Message, "updatedModel");
                        }

                        var TutorHoursTemp = entity.StartTime - entity.EndTime;
                        var Tht = TutorHoursTemp.TotalHours;
                        //updating Report bonding and tutor hours
                        entity.TutorReport.TutorHours += Tht - OldTht;
                        entity.TutorReport.TutorHoursBonding += entity.NumBondingHours - OldBondingHours;

                        //updating Tutor bonding and tutor hours
                        entity.TutorReport.TutorTrainee.Tutor.TutorHours += Tht - OldTht;
                        entity.TutorReport.TutorTrainee.Tutor.TutorHoursBonding = entity.NumBondingHours - OldBondingHours;

                        //updating Trainee bonding and tutor hours
                        entity.TutorReport.TutorTrainee.Trainee.TutorHours += Tht - OldTht;
                        entity.TutorReport.TutorTrainee.Trainee.TutorHoursBonding = entity.NumBondingHours - OldBondingHours;

                        unitOfWork.SaveChanges();

                        status.Success = true;
                        status.Message = String.Format("המפגש בתאריך {0} עודכנה בהצלחה", TutorSession.MeetingDate);
                    }
                }
            }
            catch (Exception ex)
            {
                if (status.Message == String.Empty)
                {
                status.Message = String.Format("שגיאה במהלך עדכון המפגש");
                }
                LogService.Logger.Error(status.Message, ex);
            }

            return status;
        }


        public StatusModel<List<TutorSessionModel>> GetById(int id)
        {
            var result = new StatusModel<List<TutorSessionModel>>(false, String.Empty, new List<TutorSessionModel>());

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var TutorSessionRepository = unitOfWork.GetRepository<ITutorSessionRepository>();

                    result.Data = TutorSessionRepository.GetAll().ToList().Where(TR => TR.TutorReportId == id).Select(ai => new TutorSessionModel(ai)).ToList();

                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                result.Message = String.Format("Error getting Tutor Sessions from DB");
                LogService.Logger.Error(result.Message, ex);
            }
            return result;
        }


        public StatusModel<TutorSessionModel> Get(int id)
        {
            var status = new StatusModel<TutorSessionModel>();

            try
            {
                status.Message = String.Empty;
                status.Success = false;

                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var TutorSessionRepository = unitOfWork.GetRepository<ITutorSessionRepository>();

                    var TutorSession = TutorSessionRepository.GetByKey(id);

                    status.Data = new TutorSessionModel(TutorSession);

                    status.Success = true;
                }
            }
            catch (Exception ex)
            {
                status.Message = String.Format("שגיאה. לא נמצא המפגש המבוקש.");
                LogService.Logger.Error(status.Message, ex);
            }

            return status;
        }
    }
}
