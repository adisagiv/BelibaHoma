using Ninject.Modules;
using Services.Excel;
using Services.Log;

namespace Services.Modules
{
    public class ExcelModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IExcelService>().To<ExcelService>();


        }
    }
}