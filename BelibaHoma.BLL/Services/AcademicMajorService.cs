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
    public class AcademicMajorService : IAcademicMajorService
    {
        /// <summary>
        /// Get all AcademicMajor from the db
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public List<AcademicMajorModel> Get()
        {
            var result = new List<AcademicMajorModel>();

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var academicMajorRepository = unitOfWork.GetRepository<IAcademicMajorRepository>();
                    result = academicMajorRepository.GetAll().Where(am => true).ToList().Select(am => new AcademicMajorModel(am)).ToList();
                }
            }
            catch (Exception ex)
            {
                var message = String.Format("Error getting Academic Majors from DB");
                LogService.Logger.Error(message, ex);
            }

            return result;
        }

       /// <summary>
       /// Add new Academic Major
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
        public StatusModel Add(AcademicMajorModel model)
        {
            var status = new StatusModel(false, String.Empty);

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var academicMajorRepository = unitOfWork.GetRepository<IAcademicMajorRepository>();
                    var entity = model.MapTo<AcademicMajor>();
                    academicMajorRepository.Add(entity);

                    unitOfWork.SaveChanges();

                    status.Success = true;
                    status.Message = String.Format("מסלול הלימוד {0} הוזן בהצלחה", model.Name);
                }
            }
            catch (Exception ex)
            {
                status.Message = String.Format("שגיאה במהלך הזנת מסלול לימוד");
                LogService.Logger.Error(status.Message, ex);
            }

            return status;
        }

        /// <summary>
        /// Get Major by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public StatusModel<AcademicMajorModel> Get(int id)
        {
            var status = new StatusModel<AcademicMajorModel>();

            try
            {
                status.Message = String.Empty;
                status.Success = false;

                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var academicMajorRepository = unitOfWork.GetRepository<IAcademicMajorRepository>();

                    var academicMajor = academicMajorRepository.GetByKey(id);

                    status.Data = new AcademicMajorModel(academicMajor);

                    status.Success = true;
                }
            }
            catch (Exception ex)
            {
                status.Message = String.Format("שגיאה. לא נמצא מסלול הלימוד המבוקש.");
                LogService.Logger.Error(status.Message, ex);
            }

            return status;
        }

        /// <summary>
        /// Update AcademicMajor in db
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedModel"></param>
        /// <returns></returns>
        public StatusModel Update(int id, AcademicMajorModel updatedModel)
        {
            var status = new StatusModel(false, String.Empty);

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var academicMajorRepository = unitOfWork.GetRepository<IAcademicMajorRepository>();

                    var academicMajor = academicMajorRepository.GetByKey(id);
                    if (academicMajor != null)
                    {
                        academicMajor.Name = updatedModel.Name;
                        academicMajor.AcademicCluster = (int)updatedModel.AcademicCluster;

                        unitOfWork.SaveChanges();

                        status.Success = true;
                        status.Message = String.Format("מסלול הלימוד {0} עודכן בהצלחה", academicMajor.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                status.Message = String.Format("שגיאה במהלך עדכון מסלול הלימוד");
                LogService.Logger.Error(status.Message, ex);
            }

            return status;
        }
    }
}