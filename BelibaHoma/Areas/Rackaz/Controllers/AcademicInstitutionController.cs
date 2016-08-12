﻿using BelibaHoma.BLL.Enums;
using BelibaHoma.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BelibaHoma.Areas.Rackaz.Controllers
{
    public class AcademicInstitutionController : Controller
    {
        private IAcademicInstitutionService _academicInstitutionService;

        public AcademicInstitutionController(IAcademicInstitutionService academicInstitutionService)
        {
            this._academicInstitutionService = academicInstitutionService;
        }

        // TODO: /remove area int? from action only for testing replace with nothing
        // GET: Rackaz/AcademicInstitution
        public ActionResult Index(int? area = null)
        {
            var result = _academicInstitutionService.Get((Area?)area);

            return View(result);
        }
    }
}