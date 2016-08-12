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

namespace BelibaHoma.BLL.Services
{
    public class AcademicInstitutionService : IAcademicInstitutionService
    {
        /// <summary>
        /// Get all AcademicInstitution from the db
        /// </summary>
        /// <returns></returns>
        public List<AcademicInstitutionModel> Get(Area? area)
        {
            var result = new List<AcademicInstitutionModel>();

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var academicInstitutionRepository = unitOfWork.GetRepository<IAcademicInstitutionRepository>();

                    var academicInstitutions = academicInstitutionRepository.GetAll();

                    result = academicInstitutions.Where(ai => !area.HasValue || ai.Area == (int)area.Value).ToList()
                        .Select(ai => new AcademicInstitutionModel(ai)).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            

            return result;
        }
    }
}
