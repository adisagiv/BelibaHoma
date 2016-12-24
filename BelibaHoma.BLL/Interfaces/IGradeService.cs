using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BelibaHoma.BLL.Enums;
using Generic.Models;
using BelibaHoma.BLL.Models;


namespace BelibaHoma.BLL.Interfaces
{
    public interface IGradeService
    {
        /// <summary>
        /// Get all TutorReport from the db
        /// </summary>
        /// <returns></returns>
        StatusModel<List<GradeModel>> Get();

        /// <summary>
        /// Add new TutorReport to db
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        StatusModel<string> Add(GradeModel model); //cange to viewmodel?

        /// <summary>
        /// Update TutorReport in db
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedModel"></param>
        /// <returns></returns>
        StatusModel Update(int id, GradeModel updatedModel);

        /// <summary>
        /// Get the TutorReportModel according to the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        StatusModel<GradeModel> Get(int id);
        StatusModel<GradeModel> Get(int id, int semesterNumber);
        StatusModel Delete(int id, int semesterNumber);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        StatusModel<List<GradeModel>> GetById(int id);
    }
}