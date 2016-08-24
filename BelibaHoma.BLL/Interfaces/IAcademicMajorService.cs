using BelibaHoma.BLL.Enums;
using Generic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BelibaHoma.BLL.Models;

namespace BelibaHoma.BLL.Interfaces
{
    public interface IAcademicMajorService
    {
        //TODO:consider not sending Area
        /// <summary>
        /// Get all AcademicMajor from the db
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        List<AcademicMajorModel> Get(Area? area);

        /// <summary>
        /// Add new AcademicMajor to db
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        StatusModel Add(AcademicMajorModel model);

        /// <summary>
        /// Get AcademeicMajor by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        StatusModel<AcademicInstitutionModel> Get(int id);

        /// <summary>
        /// Update AcademicMajor in db
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedModel"></param>
        /// <returns></returns>
        StatusModel Update(int id, AcademicMajorModel updatedModel);
    }
}