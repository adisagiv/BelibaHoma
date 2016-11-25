﻿using BelibaHoma.BLL.Enums;
using BelibaHoma.BLL.Interfaces;
using BelibaHoma.DAL;
using BelibaHoma.DAL.Interfaces;
using Catel.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BelibaHoma.BLL.Models;
using Generic.Models;
using Services.Log;

namespace BelibaHoma.BLL.Services
{
    public class TutorTraineeService : ITutorTraineeService
    {
        /// <summary>
        /// Get all TutorTrainee from DB
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public StatusModel<List<TutorTraineeModel>> Get(Area? area)
        {
            var result = new StatusModel<List<TutorTraineeModel>>(false, String.Empty, new List<TutorTraineeModel>());
            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var tutorTraineeRepository = unitOfWork.GetRepository<ITutorTraineeRepository>();

                    result.Data = tutorTraineeRepository.GetAll().ToList().Where(t => !area.HasValue || t.Trainee.User.Area == (int)area.Value).OrderBy(t => t.Trainee.User.Area).ThenBy(t => t.Trainee.User.LastName).ThenBy(t => t.Trainee.User.FirstName).ToList().Select(t => new TutorTraineeModel(t)).ToList();
                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                result.Message = String.Format("שגיאה במהלך שליפת קשרי החונכות מהמסד");
                LogService.Logger.Error(result.Message, ex);
            }

            return result;
        }

        /// <summary>
        /// Get TutorTrainee by the Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public StatusModel<TutorTraineeModel> Get(int id)
        {
            var status = new StatusModel<TutorTraineeModel>();

            try
            {
                status.Message = String.Empty;
                status.Success = false;

                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var tutortraineeRepository = unitOfWork.GetRepository<ITutorTraineeRepository>();

                    var tutortrainee = tutortraineeRepository.GetByKey(id);

                    status.Data = new TutorTraineeModel(tutortrainee);

                    status.Success = true;
                }
            }
            catch (Exception ex)
            {
                status.Message = String.Format("התרחשה שגיאה במהלך גישה לפרטי קשר החונכות");
                LogService.Logger.Error(status.Message, ex);
            }

            return status;
        }

        /// <summary>
        /// Change tutortrainee Status and IsException in DB
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedModel"></param>
        /// <returns></returns>
        public StatusModel Update(int id, TutorTraineeModel updatedModel)
        {
            var status = new StatusModel(false, String.Empty);

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var tutortraineeRepository = unitOfWork.GetRepository<ITutorTraineeRepository>();
                    //var tutorRepository = unitOfWork.GetRepository<ITutorRepository>();
                    //var traineeRepository = unitOfWork.GetRepository<ITraineeRepository>();

                    var tutortrainee = tutortraineeRepository.GetByKey(id);
                    //var tutor = tutortraineeRepository.GetByKey(tutortrainee.TutorId);
                    //var trainee = tutortraineeRepository.GetByKey(tutortrainee.TraineeId);

                        //Updating the entity from the model received by the form
                        tutortrainee.Status = (int)updatedModel.Status;
                        tutortrainee.IsException = updatedModel.IsException;
                        unitOfWork.SaveChanges();

                        status.Success = true;
                        status.Message = String.Format("מצב הקשר עודכן בהצלחה");
                    }
                }
            catch (Exception ex)
            {
                status.Message = String.Format("שגיאה במהלך עדכון מצב הקשר");
                LogService.Logger.Error(status.Message, ex);
            }

            return status;
        }

        /// <summary>
        /// Add new TutorTrainee
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public StatusModel AddManual(TutorTraineeModel model)
        {
            var status = new StatusModel(false, String.Empty);

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    //Updating the status in the model
                    model.Status = TTStatus.Active;
                    model.IsException = false;

                    //Retrieving Related Entities by using the repositories and GetById function (all but User which was not yet created)
                    var traineeRepository = unitOfWork.GetRepository<ITraineeRepository>();
                    var trainee = traineeRepository.GetByKey(model.TraineeId);

                    var tutorRepository = unitOfWork.GetRepository<ITutorRepository>();
                    var tutor = tutorRepository.GetByKey(model.TutorId);

                    //TODO: server side validations - check if there is an exstiting reliation,  if active - reject, if inactive  - reactive! 
                    

                    //Cast into entity
                    var entity = model.MapTo<TutorTrainee>();

                    //Linking the Complexed entities to the retrieved ones
                    entity.Trainee = trainee;
                    entity.Tutor = tutor;

                    //Finally Adding the entity to DB
                    var tutorTraineeRepository = unitOfWork.GetRepository<ITutorTraineeRepository>();
                    tutorTraineeRepository.Add(entity);
                    unitOfWork.SaveChanges();

                    //If we got here - Yay! :)
                    status.Success = true;
                    status.Message = String.Format("הציוות הוזן בהצלחה");
                    
                }
            }
            catch (Exception ex)
            {
                status.Message = String.Format("שגיאה במהלך הוספת הציוות");
                LogService.Logger.Error(status.Message, ex);
            }

            return status;
        }
       
    }
}