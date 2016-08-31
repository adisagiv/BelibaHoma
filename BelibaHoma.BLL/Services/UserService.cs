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
    public class UserService : IUserService
    {
        /// <summary>
        /// Get a list of all admins and Rackaz
        /// </summary>
        /// <returns></returns>
        public List<UserModel> GetAdminAndRackaz()
        {
            var result = new List<UserModel>();

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var userRepository = unitOfWork.GetRepository<IUserRepository>();
                    
                    result = userRepository.GetAll().Where(u => u.IsActive && u.UserRole < 2).OrderBy(u => u.Area).ToList().Select(u => new UserModel(u)).ToList();
                }
            }
            catch (Exception ex)
            {
                var message = String.Format("Error getting Admins and Rackazes from DB");
                LogService.Logger.Error(message, ex);
            }


            return result;
        }
    }
}