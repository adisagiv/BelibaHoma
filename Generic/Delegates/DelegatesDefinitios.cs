using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generic.Delegates
{
    public delegate void GenericEventHandler<in TResult>(object source,TResult result);
}
