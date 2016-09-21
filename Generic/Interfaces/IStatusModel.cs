using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Generic.Interfaces
{
    public interface IStatusModel
    {
        bool Success { get; set; }
        string Message { get; set; }


    }

    public interface IStatusModel<T> : IStatusModel
    {
        T Data { get; set; }
    }
}
