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
    public interface ITutorSessionService
    {
        /// <summary>
        /// Get all Tutor Sessions from the db
        /// </summary>
        /// <returns></returns>
        StatusModel<List<TutorSessionModel>> Get();

        /// <summary>
        /// Add new Tutor Session to db
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userRole"></param>
        /// <returns></returns>
        StatusModel Add(TutorSessionModel model, UserRole userRole);

        /// <summary>
        /// Update Tutor Session in db
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedModel"></param>
        /// <param name="userRole"></param>
        /// <returns></returns>
        StatusModel Update(int id, TutorSessionModel updatedModel, UserRole userRole);

        /// <summary>
        /// Get the TutorSessionModel according to the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        StatusModel<TutorSessionModel> Get(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        StatusModel<List<TutorSessionModel>> GetById(int id);
    }
}