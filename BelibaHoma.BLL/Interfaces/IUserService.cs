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
    }
}