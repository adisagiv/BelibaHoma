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
    public class GradeService : IGradeService
    {

        /// <summary>
        /// Get all job offers from the db
        /// </summary>
        /// <returns></returns>
        public StatusModel<List<GradeModel>> Get()
        {
            var result = new StatusModel<List<GradeModel>>(false, String.Empty, new List<GradeModel>());

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var GradeRepository = unitOfWork.GetRepository<IGradeRepository>();

                    result.Data = GradeRepository.GetAll().ToList().Select(ai => new GradeModel(ai)).ToList();

                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                result.Message = String.Format("Error getting job offers from DB");
                LogService.Logger.Error(result.Message, ex);
            }
            return result;
        }


        public StatusModel<string> Add(GradeModel model)
        {
            var status = new StatusModel<string>(false, String.Empty,String.Empty);

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    model.UpdateDate = DateTime.Now;
                    var GradeRepository = unitOfWork.GetRepository<IGradeRepository>();
                    var entity = model.MapTo<Grade>();

                    //add repositories for academic major and exstact by key according to form

                    //Retrieving Related Entities by using the repositories and GetById function (all but User which was not yet created)

                    var TraineeRepository = unitOfWork.GetRepository<ITraineeRepository>();
                    var Trainee = TraineeRepository.GetByKey(model.TraineeId);

                    if (GradeRepository.GetAll().Any(g => (g.SemesterNumber == model.SemesterNumber)&&(g.TraineeId==model.TraineeId))) //maybe id input?
                    {
                        status = new StatusModel<string>(false, "מספר סמסטר כבר קיים אנא הזן מספר אחר", "SemesterNumber");
                        return status;
                    }

                    //Linking the Complexed entities to the retrieved ones
                    entity.Trainee = Trainee;
                    
                    //entity.relevantmajor= מה ששמרתי מהרפוסיטורים
                    GradeRepository.Add(entity);

                    unitOfWork.SaveChanges();

                    status.Success = true;
                    status.Message = String.Format("הציון {0} הוזן בהצלחה", model.Grade1);
                }
            }
            catch (Exception ex)
            {
                status.Message = String.Format("שגיאה במהלך הזנת הציון");
                LogService.Logger.Error(status.Message, ex);
            }

            return status;
        }


        public StatusModel Update(int id, GradeModel updatedModel)
        {
            var status = new StatusModel(false, String.Empty);

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    updatedModel.UpdateDate = DateTime.Now;
                    var GradeRepository = unitOfWork.GetRepository<IGradeRepository>();
                    var entity = updatedModel.MapTo<Grade>();

                    //if (GradeRepository.GetAll().Any(g => (g.SemesterNumber == updatedModel.SemesterNumber) && (g.TraineeId == updatedModel.TraineeId))) //maybe id input?
                    //{
                    //    status = new StatusModel<string>(false, "מספר סמסטר כבר קיים אנא הזן מספר אחר", "SemesterNumber");
                    //    return status;
                    //}

                    var grade = GradeRepository.GetAll().FirstOrDefault(g => g.SemesterNumber == updatedModel.SemesterNumber && g.TraineeId == id);
                    if (grade != null)
                    {

                       
                        grade.Grade1 = (int)updatedModel.Grade1;
                        grade.SemesterType = (int)updatedModel.SemesterType;

                        //TODO: remove comment after year will be added to the data base
                        //grade.Year = (int) updatedModel.Year;


                        unitOfWork.SaveChanges();

                        status.Success = true;
                        status.Message = String.Format("הציון עודכן בהצלחה");
                    }
                }
            }
            catch (Exception ex)
            {
                status.Message = String.Format("שגיאה במהלך עדכון הציון");
                LogService.Logger.Error(status.Message, ex);
            }

            return status;
        }


        public StatusModel<GradeModel> Get(int id)  
        {
            var status = new StatusModel<GradeModel>();

            try
            {
                status.Message = String.Empty;
                status.Success = false;

                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var GradeRepository = unitOfWork.GetRepository<IGradeRepository>();

                    var Grade = GradeRepository.GetByKey(id);

                    status.Data = new GradeModel(Grade);

                    status.Success = true;
                }
            }
            catch (Exception ex)
            {
                status.Message = String.Format("שגיאה. לא נמצא הציון בסמסטר המבוקש.");
                LogService.Logger.Error(status.Message, ex);
            }

            return status;
        }

        public StatusModel<GradeModel> Get(int id, int semesterNumber)
        {

            var status = new StatusModel<GradeModel>();

            try
            {
                status.Message = String.Empty;
                status.Success = false;

                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var GradeRepository = unitOfWork.GetRepository<IGradeRepository>();

                    var grade = GradeRepository.GetAll().FirstOrDefault(g=> g.SemesterNumber == semesterNumber && g.TraineeId == id);  //semesterNumber);

                    status.Data = new GradeModel(grade);

                    status.Success = true;
                }
            }
            catch (Exception ex)
            {
                status.Message = String.Format("שגיאה. לא נמצא הציון בסמסטר המבוקש.");
                LogService.Logger.Error(status.Message, ex);
            }

            return status;
        
        }


        public StatusModel Delete(int id, int semesterNumber)
        {
            var status = new StatusModel(false, String.Empty);

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var GradeRepository = unitOfWork.GetRepository<IGradeRepository>();

                    var grade = GradeRepository.GetAll().FirstOrDefault(g => g.SemesterNumber == semesterNumber && g.TraineeId == id);  //semesterNumber);

                    if (grade != null)
                    {
                        GradeRepository.Delete(grade);
                        unitOfWork.SaveChanges();
                    }
                    // TODO : Add stsus if grade == null
                }
            }
            catch (Exception ex)
            {
                status.Message = String.Format("שגיאה במהלך הזנת הציון");
                LogService.Logger.Error(status.Message, ex);
            }

            return status;
            
        }


        public StatusModel<List<GradeModel>> GetById(int id)
        {
            var result = new StatusModel<List<GradeModel>>(false, String.Empty, new List<GradeModel>());

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var GradeRepository = unitOfWork.GetRepository<IGradeRepository>();

                    result.Data = GradeRepository.GetAll().Where(trId => trId.TraineeId == id).OrderBy(g=> g.SemesterNumber).ToList().Select(ai => new GradeModel(ai)).ToList();

                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                result.Message = String.Format("Error getting Grades from DB");
                LogService.Logger.Error(result.Message, ex);
            }
            return result;
        }

    }
}
