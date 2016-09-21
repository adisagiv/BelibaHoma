using BelibaHoma.BLL.Enums;
using Generic.GenericModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BelibaHoma.BLL.Models;
using Generic.Models;

namespace BelibaHoma.BLL.Interfaces
{
    public interface IAcademicInstitutionService
    {
        /// <summary>
        /// Get all AcademicInstitution from the db
        /// </summary>
        /// <returns></returns>
        List<AcademicInstitutionModel> Get(Area? area);

        /// <summary>
        /// Add new Academic Institution to db
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        StatusModel Add(AcademicInstitutionModel model);

        /// <summary>
        /// Update Academic Institution in db
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedModel"></param>
        /// <returns></returns>
        StatusModel Update(int id, AcademicInstitutionModel updatedModel);

        /// <summary>
        /// Get the AcademicInstitutionModel according to the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        StatusModel<AcademicInstitutionModel> Get(int id);
    }
}
