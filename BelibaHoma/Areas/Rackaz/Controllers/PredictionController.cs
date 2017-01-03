using BelibaHoma.BLL.Enums;
using BelibaHoma.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BelibaHoma.BLL.Models;
using BelibaHoma.Controllers;
using Generic.Models;

namespace BelibaHoma.Areas.Rackaz.Controllers
{
    [CustomAuthorization(UserRoles = new UserRole[] { UserRole.Admin, UserRole.Rackaz })]
    public class PredictionController : BaseController
    {
        private readonly IPredictionTrainingService _predictionTrainingService;

        public PredictionController(IPredictionTrainingService predictionTrainingService)
        {
            this._predictionTrainingService = predictionTrainingService;
        }

        public ActionResult Predict()
        {
            Area? area = CurrentUser.Area;
            var status = _predictionTrainingService.GeneratePrediction(area);
            if (status.Success)
            {
                return View(status.Data);
            }
            return Error(status);
        }
    }
}