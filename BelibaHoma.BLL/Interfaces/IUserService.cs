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
        StatusModel<List<UserModel>> GetAdminAndRackaz();

        /// <summary>
        /// Add new User to db
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        StatusModel<int> Add(UserModel model);
        
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

        /// <summary>
        /// Change current password
        /// </summary>
        /// <param name="id"></param>
        /// <param name="currentPassword"></param>
        /// <param name="newPassword"></param>
        /// <param name="reTypePassword"></param>
        /// <returns></returns>
        StatusModel<long> ChangePassword(int id,string currentPassword, string newPassword, string reTypePassword);

        /// <summary>
        /// Zero passwordd for user 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        StatusModel ZeroPassword(int id);
    }
}