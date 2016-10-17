using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Generic.Interfaces;

namespace Generic.Models
{
    public class StatusModel : IStatusModel
    {
        #region Parameters
        public bool Success { get; set; }
        public string Message { get; set; }

        #endregion

        #region Constructors

        public StatusModel()
        {

        }

        public StatusModel(bool success, string message)
        {
            this.Success = success;
            this.Message = message;
        }


        #endregion

    }

    public class StatusModel<T> : StatusModel, IStatusModel<T>
    {
        public T Data { get; set; }

        public StatusModel(): base()
        {

        }

        public StatusModel(bool success,string message,T data)
            :base(success,message)
        {
            this.Data = data;
        }

        
    }
}
