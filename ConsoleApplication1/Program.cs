using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using BelibaHoma.BLL.Enums;
using BelibaHoma.BLL.Interfaces;
using BelibaHoma.BLL.Models;
using BelibaHoma.BLL.Services;
using Generic.GenericModel.Models;
using Generic.Models;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var ais = new AcademicInstitutionService();

            var model = new AcademicInstitutionModel();

            model.Area = Area.Jerusalem;
            model.Name = "העברית";
            model.InstitutionType = InstitutionType.University;
            //model.Id = 1;
            //model.Id = 5;
            StatusModel statusModel = ais.Add(model);

            Console.WriteLine(statusModel.Success);
            Console.WriteLine(statusModel.Message);

        }
    }
}
