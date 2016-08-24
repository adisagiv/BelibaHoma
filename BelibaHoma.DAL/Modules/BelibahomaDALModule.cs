using System;
using System.Collections.Generic;
using Ninject.Modules;
using Catel.IoC;
using Ninject.Extensions.Conventions;
using BelibaHoma.DAL.Interfaces;
using BelibaHoma.DAL.Repositories;

namespace BelibaHoma.DAL.Modules
{
    public class BelibaHomaDALModule : NinjectModule
    {
        public override void Load()
        {

            Kernel.Bind(x =>
            x.FromThisAssembly()
                .SelectAllClasses()
                .BindAllInterfaces());

            var serviceLocator = Kernel.GetServiceLocator();
            serviceLocator.RegisterType<IAcademicInstitutionRepository, AcademicInstitutionRepository>();
            serviceLocator.RegisterType<IAcademicMajorRepository, AcademicMajorRepository>();

        }
    }
}
