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
    public class AcademicInstitutionService : IAcademicInstitutionService
    {

        /// <summary>
        /// Get all AcademicInstitution from the db
        /// </summary>
        /// <returns></returns>
        public StatusModel<List<AcademicInstitutionModel>> Get(Area? area)
        {
            var result = new StatusModel<List<AcademicInstitutionModel>>(false,String.Empty,new List<AcademicInstitutionModel>());

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var academicInstitutionRepository = unitOfWork.GetRepository<IAcademicInstitutionRepository>();

                    var academicInstitutions = academicInstitutionRepository.GetAll();

                    result.Data = academicInstitutions.Where(ai => !area.HasValue || ai.Area == (int)area.Value).ToList()
                        .Select(ai => new AcademicInstitutionModel(ai)).ToList();

                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                result.Message = String.Format("שגיאה בשליפת מוסדות הלימוד ממסד הנתונים");
                LogService.Logger.Error(result.Message, ex);
            }


            return result;
        }


        public StatusModel Add(AcademicInstitutionModel model)
        {
            var status = new StatusModel(false, String.Empty);

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var academicInstitutionRepository = unitOfWork.GetRepository<IAcademicInstitutionRepository>();
                    var entity = model.MapTo<AcademicInstitution>();
                    academicInstitutionRepository.Add(entity);

                    unitOfWork.SaveChanges();

                    status.Success = true;
                    status.Message = String.Format("מוסד הלימוד {0} הוזן בהצלחה", model.Name);
                }
            }
            catch (Exception ex)
            {
                status.Message = String.Format("שגיאה במהלך הזנת מוסד הלימוד");
                LogService.Logger.Error(status.Message, ex);
            }

            return status;
        }


        public StatusModel Update(int id, AcademicInstitutionModel updatedModel)
        {
            var status = new StatusModel(false, String.Empty);

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var academicInstitutionRepository = unitOfWork.GetRepository<IAcademicInstitutionRepository>();

                    var academicInstitution = academicInstitutionRepository.GetByKey(id);
                    if (academicInstitution != null)
                    {
                        academicInstitution.Area = (int)updatedModel.Area;
                        academicInstitution.Name = updatedModel.Name;
                        academicInstitution.InstitutionType = (int)updatedModel.InstitutionType;

                        unitOfWork.SaveChanges();

                        status.Success = true;
                        status.Message = String.Format("מוסד הלימוד {0} עודכן בהצלחה", academicInstitution.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                status.Message = String.Format("שגיאה במהלך עדכון מוסד הלימוד");
                LogService.Logger.Error(status.Message, ex);
            }

            return status;
        }


        public StatusModel<AcademicInstitutionModel> Get(int id)
        {
            var status = new StatusModel<AcademicInstitutionModel>();

            try
            {
                status.Message = String.Empty;
                status.Success = false;

                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var academicInstitutionRepository = unitOfWork.GetRepository<IAcademicInstitutionRepository>();

                    var academicInstitution = academicInstitutionRepository.GetByKey(id);

                    status.Data = new AcademicInstitutionModel(academicInstitution);

                    status.Success = true;
                }
            }
            catch (Exception ex)
            {
                status.Message = String.Format("שגיאה. לא נמצא מוסד הלימוד המבוקש.");
                LogService.Logger.Error(status.Message, ex);
            }
            
            return status;
        }
    }
}
