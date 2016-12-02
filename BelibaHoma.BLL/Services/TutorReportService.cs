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
    public class TutorReportService : ITutorReportService
    {

        /// <summary>
        /// Get all job offers from the db
        /// </summary>
        /// <returns></returns>
        public StatusModel<List<TutorReportModel>> Get()
        {
            var result = new StatusModel<List<TutorReportModel>>(false, String.Empty, new List<TutorReportModel>());

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var TutorReportRepository = unitOfWork.GetRepository<ITutorReportRepository>();

                    result.Data = TutorReportRepository.GetAll().ToList().Select(ai => new TutorReportModel(ai)).ToList();

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


        public StatusModel Add(TutorReportModel model)
        {
            var status = new StatusModel(false, String.Empty);

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    model.CreationTime = DateTime.Now;
                    var TutorReportRepository = unitOfWork.GetRepository<ITutorReportRepository>();
                    var entity = model.MapTo<TutorReport>();

                    //add repositories for academic major and exstact by key according to form

                    //Retrieving Related Entities by using the repositories and GetById function (all but User which was not yet created)

                    var TutorTraineeRepository = unitOfWork.GetRepository<ITutorTraineeRepository>();
                    var TutorTrainee = TutorTraineeRepository.GetByKey(model.TutorTraineeId);
                    //Linking the Complexed entities to the retrieved ones
                    entity.TutorTrainee = TutorTrainee;
                    //entity.relevantmajor= מה ששמרתי מהרפוסיטורים
                    TutorReportRepository.Add(entity);

                    unitOfWork.SaveChanges();

                    status.Success = true;
                    status.Message = String.Format("הדיווח בתאריך {0} הוזן בהצלחה", model.CreationTime);
                }
            }
            catch (Exception ex)
            {
                status.Message = String.Format("שגיאה במהלך הזנת הדיווח");
                LogService.Logger.Error(status.Message, ex);
            }

            return status;
        }


        public StatusModel Update(int id, TutorReportModel updatedModel)
        {
            var status = new StatusModel(false, String.Empty);

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var TutorReportRepository = unitOfWork.GetRepository<ITutorReportRepository>();

                    var TutorReport = TutorReportRepository.GetByKey(id);
                    if (TutorReport != null)
                    {
                        TutorReport.MeetingsDescription = updatedModel.MeetingsDescription;
                        TutorReport.IsProblem = updatedModel.IsProblem; 

                        unitOfWork.SaveChanges();

                        status.Success = true;
                        status.Message = String.Format("הדיווח בתאריך {0} עודכנה בהצלחה", TutorReport.CreationTime);
                    }
                }
            }
            catch (Exception ex)
            {
                status.Message = String.Format("שגיאה במהלך עדכון הדיווח");
                LogService.Logger.Error(status.Message, ex);
            }

            return status;
        }


        public StatusModel<TutorReportModel> Get(int id)
        {
            var status = new StatusModel<TutorReportModel>();

            try
            {
                status.Message = String.Empty;
                status.Success = false;

                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var TutorReportRepository = unitOfWork.GetRepository<ITutorReportRepository>();

                    var TutorReport = TutorReportRepository.GetByKey(id);

                    if (TutorReport != null)
                    {
                        status.Data = new TutorReportModel(TutorReport);

                        status.Success = true;
                    }
                    else
                    {
                        status.Message = String.Format("שגיאה. לא נמצא הדיווח המבוקש.");
                    }
                    
                }
            }
            catch (Exception ex)
            {
                status.Message = String.Format("שגיאה. לא נמצא הדיווח המבוקש.");
                LogService.Logger.Error(status.Message, ex);
            }

            return status;
        }
    }
}
