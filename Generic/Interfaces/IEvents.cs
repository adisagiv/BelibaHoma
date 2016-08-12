using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Generic.Delegates;
using Generic.Models;

namespace Generic.Interfaces
{
    public interface IEvents
    {
        event GenericEventHandler<StatusModel<ProgressModel>> OnProgressEvent;
        
    }
}
