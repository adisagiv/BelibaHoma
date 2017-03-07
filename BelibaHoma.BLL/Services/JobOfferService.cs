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
        public StatusModel<List<JobOfferModel>> Get()
        {
            var result = new StatusModel<List<JobOfferModel>>(false, String.Empty, new List<JobOfferModel>());

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var jobOfferRepository = unitOfWork.GetRepository<IJobOfferRepository>();

                    result.Data = jobOfferRepository.GetAll().ToList().Select(ai => new JobOfferModel(ai)).ToList();

                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                result.Message = String.Format("Error getting job offers from DB");
                LogService.Logger.Error(result.Message, ex);
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
                    model.CreationTime = DateTime.Now;
                    var JobOfferRepository = unitOfWork.GetRepository<IJobOfferRepository>();
                    var entity = model.MapTo<JobOffer>();
                    
                    //add repositories for academic major and exstact by key according to form
                   
                    //Retrieving Related Entities by using the repositories and GetById function (all but User which was not yet created)

                    var academicMajorRepository = unitOfWork.GetRepository<IAcademicMajorRepository>();
                    var academicMajor = academicMajorRepository.GetByKey(model.RelevantMajorId1);
                    if (model.RelevantMajorId2 != null)
                    {
                        int Id2 = (int) model.RelevantMajorId2;
                        entity.AcademicMajor1 = academicMajorRepository.GetByKey(Id2);
                    }
                    else
                    {
                        entity.AcademicMajor1 = null;
                    }
                    if (model.RelevantMajorId3 != null)
                    {
                        int Id3 = (int) model.RelevantMajorId3;
                        entity.AcademicMajor2 = academicMajorRepository.GetByKey(Id3);
                    }
                    else
                    {
                        entity.AcademicMajor2 = null;
                    }
                    
                    //Linking the Complexed entities to the retrieved ones
                    entity.AcademicMajor = academicMajor;

                    //entity.relevantmajor= מה ששמרתי מהרפוסיטורים
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
                    updatedModel.UpdateTime = DateTime.Now;
                    var JobOfferRepository = unitOfWork.GetRepository<IJobOfferRepository>();
                    var entity = updatedModel.MapTo<JobOffer>();

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
