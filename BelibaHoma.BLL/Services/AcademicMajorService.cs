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

namespace BelibaHoma.BLL.Services
{
    public class AcademicMajorService
    {
        /// <summary>
        /// Get all AcademicMajor from the db
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public List<AcademicMajorModel> Get(Area? area)
        {
            var result = new List<AcademicMajorModel>();

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var academicMajorRepository = unitOfWork.GetRepository<IAcademicMajorRepository>();

                    result = academicMajorRepository.GetAll().Select(am => new AcademicMajorModel(am)).ToList();
                }
            }
            catch (Exception ex)
            {

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
            }

            return status;
        }

        /// <summary>
        /// Get Major by id.Same as AcademicInstitution-Update but it made more sense to place before the func that calls it.  
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public StatusModel<AcademicMajorModel> Get(int id)
        {
            var status = new StatusModel<AcademicMajorModel>();
            using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
            {
                var academicMajorRepository = unitOfWork.GetRepository<IAcademicMajorRepository>();

                var academicMajor = academicMajorRepository.GetByKey(id);

                status.Data = new AcademicMajorModel(academicMajor);
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
                        //academicMajor.AcademicCluster = (int)updatedModel.Area;
                        academicMajor.Name = updatedModel.Name;
                        //double check the following line - "AcademicCluster to Cluster"
                        academicMajor.AcademicCluster = (int)updatedModel.Cluster;

                        unitOfWork.SaveChanges();

                        status.Success = true;
                        status.Message = String.Format("מסלול הלימוד {0} עודכן בהצלחה", academicMajor.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                status.Message = String.Format("שגיאה במהלך עדכון מסלול הלימוד");
            }

            return status;
        }
    }
}