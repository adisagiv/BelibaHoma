using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BelibaHoma.Areas.Admin.Models;
using BelibaHoma.Areas.Rackaz.Models;
using BelibaHoma.BLL.Enums;
using BelibaHoma.BLL.Interfaces;
using BelibaHoma.Controllers;
using Generic.Models;

namespace BelibaHoma.Areas.Admin.Controllers
{
    [CustomAuthorization(UserRoles = new UserRole[] { UserRole.Admin })]
    public class NewYearController : BaseController
    {
        private readonly ITutorService _tutorService;
        private readonly ITraineeService _traineeService;
        private readonly ITutorTraineeService _tutorTraineeService;

        public NewYearController(ITutorService tutorService, ITraineeService traineeService, ITutorTraineeService tutorTraineeService)
        {
            _tutorService = tutorService;
            _traineeService = traineeService;
            _tutorTraineeService = tutorTraineeService;
        }

        // GET: Admin/NewYear
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Area area)
        {
            var status = new StatusModel();
            var tutors = _tutorService.GetTutors(area);
            if (tutors.Success)
            {
                var trainees = _traineeService.GetTrainees(area);

                if (trainees.Success)
                {
                    var tutorTrainnee = new TutorTraineeNewYearViewModel
                    {
                        Tutors = tutors.Data.Where(t=> t.User.IsActive).ToList(),
                        Trainees = trainees.Data.Where(t => t.User.IsActive).ToList(),
                        Area = area

                    };

                    return View("NewYearAreaSelected", tutorTrainnee);

                }
                else
                {
                    status = trainees;
                }
            }
            else
            {
                status = tutors;
            }



            return Error(status);
        }

        [HttpPost]
        public ActionResult NewYearAreaSelected(Area area, List<int> chooseTutor, List<int> chooseTrainee)
        {
            var status = _tutorService.MoveToNextYear(area, chooseTutor);

            if (status.Success)
            {
                status = _traineeService.MoveToNextYear(area, chooseTrainee);

                if (status.Success)
                {
                    var result = _tutorTraineeService.Get(area,  TTStatus.Active);

                    if (result.Success)
                    {
                        var tutorTrainnee = new TutorTraineeNewYearViewModel
                        {
                            TutorTrainee = result.Data,
                            Area = area

                        };
                        return View("SelecetMatches", tutorTrainnee);
                    }
                    else
                    {
                        status = result;
                    }

                }
            }

            return Error(status);
        }
        [HttpPost]
        public ActionResult SelecetMatches(Area area, List<int> chooseTutorTrainee)
        {
            var status = _tutorTraineeService.MoveToNextYear(area, chooseTutorTrainee);

            if (status.Success)
            {
                return View("NewYearFinished");
            }

            return Error(status);

        }


    }
}