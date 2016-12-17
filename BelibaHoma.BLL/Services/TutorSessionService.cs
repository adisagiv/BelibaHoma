﻿using BelibaHoma.BLL.Enums;
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

                    //add repositories for academic major and exstact by key according to form

                    //Retrieving Related Entities by using the repositories and GetById function (all but User which was not yet created)

                   var TutorReportRepository = unitOfWork.GetRepository<ITutorReportRepository>();
                   var TutorReport = TutorReportRepository.GetByKey(model.TutorReportId);

                    //Linking the Complexed entities to the retrieved ones
                    entity.TutorReport = TutorReport;
                    var TutorHoursTemp = entity.StartTime - entity.EndTime;
                    var Tht = TutorHoursTemp.TotalHours;
                    entity.TutorReport.TutorHours += Tht;

                    entity.TutorReport.TutorHoursBonding += entity.NumBondingHours;
                    
                    //entity.relevantmajor= מה ששמרתי מהרפוסיטורים
                    TutorSessionRepository.Add(entity);

                    unitOfWork.SaveChanges();

                    status.Success = true;
                    status.Message = String.Format("המפגש בתאריך {0} הוזן בהצלחה", model.MeetingDate);
                }
            }
            catch (Exception ex)
            {
                status.Message = String.Format("שגיאה במהלך הזנת המפגש");
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

                        var TutorHoursTemp = entity.StartTime - entity.EndTime;
                        var Tht = TutorHoursTemp.TotalHours;
                        entity.TutorReport.TutorHours += Tht - OldTht;

                        entity.TutorReport.TutorHoursBonding += entity.NumBondingHours - OldBondingHours;

                        unitOfWork.SaveChanges();

                        status.Success = true;
                        status.Message = String.Format("המפגש בתאריך {0} עודכנה בהצלחה", TutorSession.MeetingDate);
                    }
                }
            }
            catch (Exception ex)
            {
                status.Message = String.Format("שגיאה במהלך עדכון המפגש");
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
