using Ninject.Modules;
using Services.Log;

namespace Services.Modules
{
    public class LogModule : NinjectModule
    {
        public override void Load()
        {
            //Bind<ILogService>().To<LogService>().InSingletonScope();

            
        }
    }
}
