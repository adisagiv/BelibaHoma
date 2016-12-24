﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BelibaHoma.BLL.Enums;
using Generic.Models;
using BelibaHoma.BLL.Models;


namespace BelibaHoma.BLL.Interfaces
{
    public interface ITutorReportService
    {
        /// <summary>
        /// Get all TutorReport from the db
        /// </summary>
        /// <returns></returns>
        StatusModel<List<TutorReportModel>> Get();

        /// <summary>
        /// Add new TutorReport to db
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        StatusModel Add(TutorReportModel model);

        /// <summary>
        /// Update TutorReport in db
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedModel"></param>
        /// <returns></returns>
        StatusModel Update(int id, TutorReportModel updatedModel);

        /// <summary>
        /// Get the TutorReportModel according to the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        StatusModel<TutorReportModel> Get(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        StatusModel<List<TutorReportModel>> GetById(int id);
    }
}