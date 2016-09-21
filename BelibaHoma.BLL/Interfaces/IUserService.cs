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
    public interface IUserService
    {
        /// <summary>
        /// Get a list of all admins and Rackaz
        /// </summary>
        /// <returns></returns>
        List<UserModel> GetAdminAndRackaz();

        /// <summary>
        /// Add new User to db
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        StatusModel Add(UserModel model);
        
        /// <summary>
        /// Update a User in db
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedModel"></param>
        /// <returns></returns>
        StatusModel Update(int id, UserModel updatedModel);

        /// <summary>
        /// Get User by ID (index/key) from db
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        StatusModel<UserModel> Get(int id);
    }
}