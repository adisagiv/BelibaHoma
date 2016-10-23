using BelibaHoma.BLL.Enums;
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
    public class JobOfferService : IJobOfferService
    {

        /// <summary>
        /// Get all job offers from the db
        /// </summary>
        /// <returns></returns>
        public List<JobOfferModel> Get(JobArea? jobarea)
        {
            var result = new List<JobOfferModel>();

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var JobOfferRepository = unitOfWork.GetRepository<IJobOfferRepository>();

                    var Joboffer = JobOfferRepository.GetAll();

                    result = Joboffer.Where(ai => !jobarea.HasValue || ai.JobArea == (int)jobarea.Value).ToList()
                        .Select(ai => new JobOfferModel(ai)).ToList();
                }
            }
            catch (Exception ex)
            {
                var message = String.Format("Error getting job offers from DB");
                LogService.Logger.Error(message, ex);
            }


            return result;
        }


        public StatusModel Add(JobOfferModel model)
        {
            var status = new StatusModel(false, String.Empty);

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var JobOfferRepository = unitOfWork.GetRepository<IJobOfferRepository>();
                    var entity = model.MapTo<JobOffer>();
                    JobOfferRepository.Add(entity);

                    unitOfWork.SaveChanges();

                    status.Success = true;
                    status.Message = String.Format("המשרה {0} הוזנה בהצלחה", model.JobTitle);
                }
            }
            catch (Exception ex)
            {
                status.Message = String.Format("שגיאה במהלך הזנת המשרה");
                LogService.Logger.Error(status.Message, ex);
            }

            return status;
        }


        public StatusModel Update(int id, JobOfferModel updatedModel)
        {
            var status = new StatusModel(false, String.Empty);

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var JobOfferRepository = unitOfWork.GetRepository<IJobOfferRepository>();

                    var joboffer = JobOfferRepository.GetByKey(id);
                    if (joboffer != null)
                    {
                        joboffer.JobArea = (int)updatedModel.JobArea;
                        joboffer.JobTitle = updatedModel.JobTitle;
                        joboffer.Requirements = updatedModel.Requirements;
                        joboffer.Description = updatedModel.Description;
                        joboffer.RelevantMajorId1 = updatedModel.RelevantMajorId1; //problem with FK
                        joboffer.RelevantMajorId2 = updatedModel.RelevantMajorId2; //problem with FK
                        joboffer.RelevantMajorId3 = updatedModel.RelevantMajorId3; //problem with FK
                        joboffer.Organization = updatedModel.Organization;
                        joboffer.JobType = (int)updatedModel.JobType;
                        joboffer.Address = updatedModel.Address;
                        joboffer.NumEmployees = (int)updatedModel.NumEmployees;
                        joboffer.ContactName = updatedModel.ContactName;
                        joboffer.ContactMail = updatedModel.ContactMail;
                        joboffer.ContactPhone = updatedModel.ContactPhone;
                        joboffer.ContactJobPosition = updatedModel.ContactJobPosition;
                        joboffer.JobStatus = (int)updatedModel.JobStatus;
                        

                        unitOfWork.SaveChanges();

                        status.Success = true;
                        status.Message = String.Format("המשרה {0} עודכנה בהצלחה", joboffer.JobTitle);
                    }
                }
            }
            catch (Exception ex)
            {
                status.Message = String.Format("שגיאה במהלך עדכון המשרה");
                LogService.Logger.Error(status.Message, ex);
            }

            return status;
        }


        public StatusModel<JobOfferModel> Get(int id)
        {
            var status = new StatusModel<JobOfferModel>();

            try
            {
                status.Message = String.Empty;
                status.Success = false;

                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var JobOfferRepository = unitOfWork.GetRepository<IJobOfferRepository>();

                    var joboffer = JobOfferRepository.GetByKey(id);

                    status.Data = new JobOfferModel(joboffer);

                    status.Success = true;
                }
            }
            catch (Exception ex)
            {
                status.Message = String.Format("שגיאה. לא נמצאה המשרה המבוקשת.");
                LogService.Logger.Error(status.Message, ex);
            }
            
            return status;
        }
    }
}
