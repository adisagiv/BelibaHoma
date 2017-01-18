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
using BelibaHoma.DAL.Repositories;
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

        public StatusModel<List<TutorReportModel>> GetById(int id)
        {
            var result = new StatusModel<List<TutorReportModel>>(false, String.Empty, new List<TutorReportModel>());

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var TutorReportRepository = unitOfWork.GetRepository<ITutorReportRepository>();

                    result.Data = TutorReportRepository.GetAll().ToList().Where(tr => tr.TutorTraineeId == id).Select(ai => new TutorReportModel(ai)).ToList();

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

        public StatusModel<List<TutorReportModel>> GetByTraineeId(int id)
        {
            var status = new StatusModel<List<TutorReportModel>>(false, String.Empty, new List<TutorReportModel>());
            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var traineeRepository = unitOfWork.GetRepository<ITraineeRepository>();
                    var reports =
                        traineeRepository.GetQuery(t => t.UserId == id)
                            .SelectMany(t => t.TutorTrainee)
                            .SelectMany(tt => tt.TutorReport)
                            .OrderBy(r => r.CreationTime).ToList()
                            .Select(r => new TutorReportModel(r))
                            .ToList();

                    status.Data = reports;
                    status.Success = true;
                    status.Message = String.Format("הדיווחים נשלפו בהצלחה");
                }
            }
            catch (Exception ex)
            {
                status.Message = String.Format("שגיאה במהלך שליפת דיווחים עבור חניך");
                LogService.Logger.Error(status.Message, ex);
            }

            return status;
        }


        public StatusModel<int> Add(TutorReportModel model, UserRole userRole)
        {
            var status = new StatusModel<int>(false, String.Empty,0);

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    model.CreationTime = DateTime.Now;
                    var tutorReportRepository = unitOfWork.GetRepository<ITutorReportRepository>();
                    var entity = model.MapTo<TutorReport>();

                    //Retrieving Related Entities by using the repositories and GetById function (all but User which was not yet created)
                    var tutorTraineeRepository = unitOfWork.GetRepository<ITutorTraineeRepository>();
                    var tutorTrainee = tutorTraineeRepository.GetByKey(model.TutorTraineeId);

                    //Server Side Validations
                    bool isNotLate =
                            tutorReportRepository.GetAll()
                                .ToList()
                                .Any(tr => tr.TutorTrainee.Status == (int)TTStatus.Active && tr.TutorTrainee.TutorId == tutorTrainee.Tutor.UserId && tr.CreationTime > DateTime.Now.AddDays(-21));
                    bool isReport = tutorReportRepository.GetAll()
                                .ToList()
                                .Any(tr => tr.TutorTrainee.Status == (int)TTStatus.Active && tr.TutorTrainee.TutorId == tutorTrainee.Tutor.UserId);

                    if (userRole == UserRole.Trainee && isNotLate == false && isReport)
                    {
                        status.Message = "לא הוזן דיווח כבר למעלה מ-3 שבועות.\nאנא פנה אל הרכז האזורי לעזרה.";
                        throw new System.ArgumentException(status.Message, "model");
                    }

                    //Linking the Complexed entities to the retrieved ones
                    entity.TutorTrainee = tutorTrainee;
                    
                    //entity.relevantmajor= מה ששמרתי מהרפוסיטורים
                    tutorReportRepository.Add(entity);

                    unitOfWork.SaveChanges();

                    status.Data = entity.Id;

                    status.Success = true;
                    status.Message = String.Format("הדיווח בתאריך {0} הוזן בהצלחה", model.CreationTime);
                }
            }
            catch (Exception ex)
            {
                if (status.Message == String.Empty)
                {
                    status.Message = String.Format("שגיאה במהלך הזנת הדיווח");   
                }
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
