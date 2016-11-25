﻿using System;
using System.ComponentModel.DataAnnotations;
using BelibaHoma.BLL.Enums;
using BelibaHoma.DAL;
using Generic.GenericModel.Models;
using Generic.Models;
using log4net;

namespace BelibaHoma.BLL.Models
{
    public class UserModel : GenericModel
    {
        public int Id { get; set; }

        [Display(Name = "תעודת זהות")]
        [Required(ErrorMessage = "נא להזין מספר תעודת זהות כולל ספרת ביקורת")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "נא להזין ת.ז. כולל ספרת ביקורת")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "נא להזין ספרות בלבד")]
        public string IdNumber { get; set; }
        
        [Display(Name = "שם פרטי")]
        [Required(ErrorMessage = "שם פרטי זהו שדה חובה")]
        public string FirstName { get; set; }
        
        [Display(Name = "שם משפחה")]
        [Required(ErrorMessage = "שם משפחה זהו שדה חובה")]
        public string LastName { get; set; }
        
        [Display(Name = "סיסמא")]
        [Required(ErrorMessage = "נא להזין סיסמא")]
        [DataType(DataType.Password)]
        [MinLength(6)]
        [MaxLength(20)]
        public string Password { get; set; }
        
        [Display(Name = "זמן יצירה")]
        public DateTime CreationTime { get; set; }
        
        [Display(Name = "סוג משתמש")]
        [Required(ErrorMessage = "נא לבחור את סוג המשתמש")]
        public UserRole UserRole { get; set; }
        
        [Display(Name = "זמן עדכון")]
        public DateTime UpdateTime { get; set; }
        
        [Display(Name = "זמן עדכון סיסמא")]
        public long? LastPasswordUpdate { get; set; }
        
        [Display(Name = "כתובת מייל")]
        [EmailAddress(ErrorMessage = "נא להזין כתובת מייל תקינה")]
        public string Email { get; set; }
        
        [Display(Name = "משתמש פעיל")]
        public bool IsActive { get; set; }
        
        [Display(Name = "אזור פעילות")]
        public Area? Area { get; set; }

        [Display(Name = "שם מלא")]
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
                    Area = (Area) Enum.Parse(typeof(Area), userDetails[3]);
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

        public UserModel()
        {
            // TODO: Complete member initialization
        }
    }
}

