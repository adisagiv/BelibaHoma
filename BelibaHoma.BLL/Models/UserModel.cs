using System;
using BelibaHoma.BLL.Enums;
using BelibaHoma.DAL;
using Generic.Models;
using log4net;

namespace BelibaHoma.BLL.Models
{
    public class UserModel : GenericModel<UserModel>
    {

        public int Id { get; set; }
        public string IdNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public DateTime CreationTime { get; set; }
        public UserRole UserRole { get; set; }
        public DateTime UpdateTime { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public Area? Area { get; set; }

        public string FullName { get { return String.Format("{0}, {1}", LastName, FirstName); }  }

        public UserModel(User entity)
            : base(entity)
        {

        }

        public UserModel(System.Web.Security.FormsAuthenticationTicket ticket)
        {
            try
            {
                string[] userDetails = ticket.UserData.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                Id = ticket.Version;
                UserRole = (UserRole)Enum.Parse(typeof(UserRole), userDetails[0]);
                LastName = userDetails[1];
                FirstName = userDetails[2];

                if (userDetails.Length > 3)
                {
                    Area = (Area) Enum.Parse(typeof(Area), userDetails[1]);
                }
                else
                {
                    Area = null;
                }
                
                IdNumber = ticket.Name;

            }
            catch (Exception ex)
            {
                ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                _logger.Error(string.Format("error creating login details model from authentication ticket: {0}", ex));

                Id = -1;
            }
        }
    }
}

