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
    public interface ITutorService
    {
        /// <summary>
        /// Get list of all Tutors from db
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        StatusModel<List<TutorModel>> GetTutors(Area? area);

        /// <summary>
        /// Add new Tutor
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        StatusModel Add(TutorModel model);

            
        /// <summary>
        /// Get Tutor by the User Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        StatusModel<TutorModel> Get(int id);

        /// <summary>
        /// Update tutor in DB
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedModel"></param>
        /// <returns></returns>
        StatusModel Update(int id, TutorModel updatedModel);
    }

    }

