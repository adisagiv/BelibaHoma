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
                    //TODO: server side validations - 
                    var tutorTraineeRepository = unitOfWork.GetRepository<ITutorTraineeRepository>();
                    var checkExistingRelation =
                        tutorTraineeRepository.GetAll()
                            .SingleOrDefault(t => t.TutorId == model.TutorId && t.TraineeId == model.TraineeId);
                    if (checkExistingRelation != null)
                    {
                        if (checkExistingRelation.Status == (int)TTStatus.Active)
                        {
                            //relation exists in Active status
                            status.Message = "קיים ציוות פעיל בין החניך והחונך במערכת";
                            throw new System.ArgumentException(status.Message, "model");
                        }
                        else
                        {
                            //exist in another status => change to Active
                            checkExistingRelation.Status = (int) TTStatus.Active;
                            unitOfWork.SaveChanges();

                            //If we got here - Yay! :)
                            status.Success = true;
                            status.Message = String.Format("הציוות עודכן בהצלחה");
                            return status;
                        }
                    }

                    //Updating the status in the model
                    model.Status = TTStatus.Active;
                    model.IsException = false;

                    //Retrieving Related Entities by using the repositories and GetById function (all but User which was not yet created)
                    var traineeRepository = unitOfWork.GetRepository<ITraineeRepository>();
                    var trainee = traineeRepository.GetByKey(model.TraineeId);

                    var tutorRepository = unitOfWork.GetRepository<ITutorRepository>();
                    var tutor = tutorRepository.GetByKey(model.TutorId);

                    //Cast into entity
                    var entity = model.MapTo<TutorTrainee>();

                    //Linking the Complexed entities to the retrieved ones
                    entity.Trainee = trainee;
                    entity.Tutor = tutor;

                    //Finally Adding the entity to DB
                    tutorTraineeRepository.Add(entity);
                    unitOfWork.SaveChanges();

                    //If we got here - Yay! :)
                    status.Success = true;
                    status.Message = String.Format("הציוות הוזן בהצלחה");
                    
                }
            }
            catch (Exception ex)
            {
                if (status.Message == String.Empty)
                {
                    status.Message = String.Format("שגיאה במהלך הוספת הציוות");   
                }
                LogService.Logger.Error(status.Message, ex);
            }

            return status;
        }

        public StatusModel<List<TutorTraineeUnApprovedViewModel>> GetUnApprovedMatches(Area area)
        {
            var result = new StatusModel<List<TutorTraineeUnApprovedViewModel>>(false, String.Empty, new List<TutorTraineeUnApprovedViewModel>());
            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var tutorTraineeRepository = unitOfWork.GetRepository<ITutorTraineeRepository>();

                    result.Data = tutorTraineeRepository.GetAll().Where(t => t.Status == (int)TTStatus.UnApproved && t.Trainee.User.Area == (int)area && t.Tutor.User.Area == (int)area).ToList().Select(t => new TutorTraineeUnApprovedViewModel(t, t.Trainee.TutorTrainee.Count(tt => t.Status == (int)TTStatus.Active), t.Tutor.TutorTrainee.Count(tt => t.Status == (int)TTStatus.Active))).ToList();
                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                result.Message = String.Format("שגיאה במהלך שליפת המלצות לקשרי חונכות ממסד הנתתונים");
                LogService.Logger.Error(result.Message, ex);
            }

            return result;
        }


        public StatusModel<List<TutorTraineeModel>> GetById(int id)
        {
            var result = new StatusModel<List<TutorTraineeModel>>(false, String.Empty, new List<TutorTraineeModel>());

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var TutorTraineeRepository = unitOfWork.GetRepository<ITutorTraineeRepository>();

                    result.Data = TutorTraineeRepository.GetAll().ToList().Where(tid => tid.TutorId == id).Select(ai => new TutorTraineeModel(ai)).ToList();

                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                result.Message = String.Format("Error getting TutorTrainee from DB");
                LogService.Logger.Error(result.Message, ex);
            }
            return result;
        }


        public StatusModel Remove(int id)
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
                    tutortraineeRepository.Delete(tutortrainee);

                    unitOfWork.SaveChanges();
                    status.Success = true;
                }
            }
            catch (Exception ex)
            {
                status.Message = String.Format("התרחשה שגיאה במהלך מחיקת ההמלצה לחונכות");
                LogService.Logger.Error(status.Message, ex);
            }

            return status;
        }

        public StatusModel<List<TutorTraineeModel>> GetRecommended(Area area)
        {
            var result = new StatusModel<List<TutorTraineeModel>>(false, String.Empty, new List<TutorTraineeModel>());
            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var tutorTraineeRepository = unitOfWork.GetRepository<ITutorTraineeRepository>();

                    result.Data = tutorTraineeRepository.GetAll().ToList().Where(t => t.Trainee.User.Area == (int)area && t.Status == (int)TTStatus.UnApproved).OrderBy(t => t.Trainee.User.LastName).ThenBy(t => t.Trainee.User.FirstName).ToList().Select(t => new TutorTraineeModel(t)).ToList();
                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                result.Message = String.Format("שגיאה במהלך שליפת ההמלצות לחונכות מהמסד");
                LogService.Logger.Error(result.Message, ex);
            }

            return result;
        }

        public StatusModel RunAlgorithm(AlgorithmModel model)
        {
            throw new NotImplementedException();
            var status = new StatusModel(false, String.Empty);
            int numTrainees = model.TraineeList.Count;
            int numTutors = model.TutorList.Count;

            int[,] utilityMat = new int[numTrainees,numTutors];
            int maxUtil = 0;

            for (int traineeIdx = 0; traineeIdx < numTrainees; traineeIdx++)
            {
                for (int tutorIdx = 0; tutorIdx < numTutors; tutorIdx++)
                {
                    TraineeModel trainee = model.TraineeList[traineeIdx];
                    TutorModel tutor = model.TutorList[tutorIdx];
                    int libaWeight = 5;
                    int majorWeight = 1000;
                    int minorWeight = 800;
                    int clusterWeight = 200;
                    int institutionWeight = 100; 

                    bool isMechina = trainee.AcademicInstitution.InstitutionType == InstitutionType.Mechina;
                    if (isMechina)
                    {
                        majorWeight = majorWeight/5;
                        institutionWeight = institutionWeight/4;
                        minorWeight = minorWeight/5;
                        clusterWeight = clusterWeight/4;

                    }

                    //Gender must match
                    if (trainee.Gender != tutor.Gender)
                    {
                        utilityMat[traineeIdx, tutorIdx] = -1;
                        break;
                    }

                    //TODO: Check with or if required AcademicYear instead of SemesterNumber
                    //tutor has senority over trainee
                    if (trainee.SemesterNumber > tutor.SemesterNumber)
                    {
                        utilityMat[traineeIdx, tutorIdx] = -1;
                        break;
                    }

                    //Liba matching
                    if ((int)trainee.MathLevel > (int)tutor.MathLevel || (int)trainee.EnglishLevel > (int)tutor.EnglishLevel || (int)trainee.PhysicsLevel > (int)tutor.PhysicsLevel)
                    {
                        utilityMat[traineeIdx, tutorIdx] = -1;
                        break;
                    }
                    else 
                    {
                        utilityMat[traineeIdx, tutorIdx] += libaWeight;
                    }

                    //Trainee Major match
                    int calc1 = 0;
                    if (trainee.AcademicMajor == tutor.AcademicMajor)
                    {
                        calc1 += majorWeight;
                    }
                    else if (tutor.AcademicMajor1 != null && trainee.AcademicMajor == tutor.AcademicMajor1)
                    {
                        calc1 += majorWeight - (int)(0.1*majorWeight);
                    }

                    //Trainee minor match
                    int calc2 = 0;
                    if (trainee.AcademicMajor1 != null)
                    {
                        if (tutor.AcademicMajor1 != null && trainee.AcademicMajor1 == tutor.AcademicMajor1)
                        {
                            calc2 += minorWeight;
                        }
                        else if (trainee.AcademicMajor1 == tutor.AcademicMajor)
                        {
                            calc2 += minorWeight - (int)(0.1 * minorWeight);
                        }            
                    }
                    else
                    {
                        calc1 = (int)(1.5*calc1);
                    }

                    //Trainee needed help exsists
                    if (trainee.AcademicMajor2 != null)
                    {
                        if (trainee.AcademicMajor2 == tutor.AcademicMajor)
                        {
                            calc1 += (int) (majorWeight*0.5);
                        }
                        else if (tutor.AcademicMajor1 != null && trainee.AcademicMajor2 == tutor.AcademicMajor1)
                        {
                            calc1 += (int) (majorWeight*0.5);
                        }
                    }
                    else
                    {
                        if (calc1 > 0) calc1 += (int) (majorWeight*0.5);
                        if (calc2 > 0) calc2 += (int)(minorWeight*0.5);
                    }
                    
                    //adding calc to matrix
                    utilityMat[traineeIdx, tutorIdx] += (calc1 + calc2);


                    //Academic Clusters
                    //Trainee major cluster
                    calc1 = 0;
                    if (trainee.AcademicMajor.AcademicCluster == tutor.AcademicMajor.AcademicCluster)
                    {
                        calc1 += clusterWeight;
                    }
                    else if (tutor.AcademicMajor1 != null && trainee.AcademicMajor.AcademicCluster == tutor.AcademicMajor1.AcademicCluster)
                    {
                        calc1 += clusterWeight - (int)(0.1 * clusterWeight);
                    }

                    //Trainee minor cluster
                    calc2 = 0;
                    if (trainee.AcademicMajor1 != null)
                    {
                        if (tutor.AcademicMajor1 != null && trainee.AcademicMajor1.AcademicCluster == tutor.AcademicMajor1.AcademicCluster)
                        {
                            calc2 += clusterWeight;
                        }
                        else if (trainee.AcademicMajor1.AcademicCluster == tutor.AcademicMajor.AcademicCluster)
                        {
                            calc2 += clusterWeight - (int)(0.1 * clusterWeight);
                        }
                    }
                    else
                    {
                        calc1 = (int)(1.5 * calc1);
                    }

                    //Trainee needed help exsists - consider cluster
                    if (trainee.AcademicMajor2 != null)
                    {
                        if (trainee.AcademicMajor2.AcademicCluster == tutor.AcademicMajor.AcademicCluster)
                        {
                            calc1 += (int)(clusterWeight * 0.5);
                        }
                        else if (tutor.AcademicMajor1 != null && trainee.AcademicMajor2.AcademicCluster == tutor.AcademicMajor1.AcademicCluster)
                        {
                            calc1 += (int)(clusterWeight * 0.35);
                        }
                    }
                    else
                    {
                        if (calc1 > 0) calc1 += (int)(clusterWeight * 0.5);
                        if (calc2 > 0) calc2 += (int)(clusterWeight * 0.5);
                    }

                    //adding calc to matrix
                    utilityMat[traineeIdx, tutorIdx] += (calc1 + calc2);

                    //AcademicInstitution needs to be alike.. or similar
                    //Academic institution does not disqualify matches
                    if (trainee.AcademicInstitution.InstitutionType == tutor.AcademicInstitution.InstitutionType)
                    {
                        utilityMat[traineeIdx, tutorIdx] += institutionWeight;
                    }
                    else if (trainee.AcademicInstitution.InstitutionType < tutor.AcademicInstitution.InstitutionType)
                    {
                        utilityMat[traineeIdx, tutorIdx] += institutionWeight/2;
                    }

                    //If they have the same Academic Institution
                    if (trainee.AcademicInstitution.Name == tutor.AcademicInstitution.Name)
                    {
                        utilityMat[traineeIdx, tutorIdx] += institutionWeight;
                    }
                }

            }

            //Generate cost matrix

            //Create a balanced matrix for the algorithm NXN
            int matSize = 0;
            matSize = numTrainees >= numTutors ? numTrainees : numTutors;
            int[,] costMatrix = new int[matSize,matSize];
            int bigM= 100000;
            maxUtil = 5000;

            for (int traineeIdx = 0; traineeIdx < numTrainees; traineeIdx++)
            {
                for (int tutorIdx = 0; tutorIdx < numTutors; tutorIdx++)
                {
                    //replace -1 (forbidden) with M
                    if (utilityMat[traineeIdx, tutorIdx] == -1)
                    {
                        costMatrix[traineeIdx, tutorIdx] = bigM;
                    }
                    //Convert utility to cost
                    else
                    {
                        costMatrix[traineeIdx, tutorIdx] = maxUtil - utilityMat[traineeIdx, tutorIdx];
                    }
                }
            }

            // Send Matrix to Algorithm
            MatchingAlgorithm alg = new MatchingAlgorithm(costMatrix);
            int[,] finalmatch = alg.Run();

            //Translate Matrix to actual matches



            //TODO: make sure its fine to not go over the whole matrix - and that eliminates the need to go over imaginary rows/cols
            for (int traineeIdx = 0; traineeIdx < numTrainees; traineeIdx++)
            {
                for (int tutorIdx = 0; tutorIdx < numTutors; tutorIdx++)
                {
                    if (finalmatch[traineeIdx, tutorIdx] == 5)
                    {
                        if (costMatrix[traineeIdx, traineeIdx] != bigM)
                        {
                            //Create Match
                            TutorTraineeAdd(model.TutorList[tutorIdx], model.TraineeList[traineeIdx]);
                        }
                    }
                }
            }

            return status;
        }

        public StatusModel TutorTraineeAdd(TutorModel tutor,TraineeModel trainee )
        {
            throw new NotImplementedException();
            //var status = new StatusModel(false, String.Empty);

            //try
            //{
            //    using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
            //    {

            //        //Retrieving Related Entities by using the repositories and GetById function (all but User which was not yet created)
            //        var traineeRepository = unitOfWork.GetRepository<ITraineeRepository>();
            //        var _trainee  = traineeRepository.GetByKey(trainee.UserId);

            //        var tutorRepository = unitOfWork.GetRepository<ITutorRepository>();
            //        var _tutor = tutorRepository.GetByKey(tutor.UserId);

            //        TutorTraineeModel newModel = new TutorTraineeModel();
            //        newModel.Status = TTStatus.UnApproved;    

            //        //Mapping the model into TutorTrainee Entity
            //        var tutortraineeRepository = unitOfWork.GetRepository<ITutorTraineeRepository>();
            //        var entity = newModel.MapTo<TutorTrainee>();

            //        //Linking the Complexed entities to the retrieved ones
            //        entity.Tutor = _tutor;
            //        entity.TutorId = tutor.UserId;
            //        entity.Trainee = _trainee;
            //        entity.TraineeId = _trainee.UserId;
            //        entity.IsException = false;

            //        //Finally Adding the entity to DB
            //        ITutorTraineeRepository.Add(entity);
            //        unitOfWork.SaveChanges();

            //            //If we got here - Yay! :)
            //            status.Success = true;
            //            status.Message = String.Format("חונך {0} הוזן בהצלחה", tutor1.User.FullName);
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    status.Message = String.Format("שגיאה במהלך הוספת החונך");
            //    LogService.Logger.Error(status.Message, ex);
            //}

            //return status;
        }
    }
}