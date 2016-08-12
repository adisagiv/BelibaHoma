using System;
using System.Collections.Generic;
using Ninject.Modules;
using Ninject.Extensions.Conventions;
using BelibaHoma.DAL.Modules;

namespace BelibaHoma.BLL.Modules
{
    public class BelibaHomaBLLModule : NinjectModule
    {
        public override void Load()
        {

            Kernel.Bind(x =>
            x.FromThisAssembly()
                .SelectAllClasses()
                .BindAllInterfaces());

            
            var modules = new List<INinjectModule>
            {
                new BelibaHomaDALModule(),
            };

            Kernel.Load(modules);
        }
    }
}
