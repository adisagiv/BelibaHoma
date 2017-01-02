using BelibaHoma.BLL.Enums;
using BelibaHoma.BLL.Interfaces;
using BelibaHoma.DAL;
using BelibaHoma.DAL.Interfaces;
using Catel.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using BelibaHoma.BLL.Models;
using Generic.Models;
using Services.Log;

namespace BelibaHoma.BLL.Services
{
    public class PredictionTrainingService : IPredictionTrainingService
    {
        public StatusModel AddFromGrade(int traineeId, int semesterNumber)
        {
            var status = new StatusModel(false, String.Empty);

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var gradeRepository = unitOfWork.GetRepository<IGradeRepository>();
                    var currentGrade = gradeRepository.GetQuery(g => g.SemesterNumber == semesterNumber && g.TraineeId == traineeId).First();
                    var lastGrade = gradeRepository.GetAll().FirstOrDefault(g => g.TraineeId == currentGrade.TraineeId && g.SemesterNumber == semesterNumber - 1);

                    var predictionTrainingRepository = unitOfWork.GetRepository<IPredictionTrainingRepository>();
                    var entity = new PredictionTraining();

                    entity.Gender = currentGrade.Trainee.Gender;
                    entity.AcademicInstitution = currentGrade.Trainee.AcademicInstitutionId;
                    entity.AcademicMajor = currentGrade.Trainee.AcademicMajorId;
                    entity.AcademicMinor = (currentGrade.Trainee.AcademicMajor1 != null ? (double) currentGrade.Trainee.AcademicMinorId : -1.0);
                    entity.SemesterNumber = currentGrade.SemesterNumber;
                    entity.LastSemesterGrade = (lastGrade != null ? lastGrade.Grade1 : -1.0);
                    entity.TraineeId = traineeId;

                    var alertRepository = unitOfWork.GetRepository<IAlertRepository>();
                    entity.AlertCount = alertRepository.GetAll().Count(a => a.AlertType == (int) AlertType.Intervention && a.TutorReport.TutorTrainee.TraineeId == currentGrade.TraineeId);
                    entity.IsDroppedOut = 0.0;

                    predictionTrainingRepository.Add(entity);
                    unitOfWork.SaveChanges();
                    
                    //If we got here - Yay! :)
                    status.Success = true;
                    status.Message = String.Format("רשומת האימון נוספה בהצלחה");
                    return status;
                }
            }
            catch (Exception ex)
            {
                if (status.Message == String.Empty)
                {
                    status.Message = String.Format("שגיאה במהלך הוספת רשומת אימון עקב הוספת ציון לחניך");
                }
                LogService.Logger.Error(status.Message, ex);
            }
            return status;
        }

        public StatusModel AddFromDropping(int traineeId)
        {
            var status = new StatusModel(false, String.Empty);

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var traineeRepository = unitOfWork.GetRepository<ITraineeRepository>();
                    var trainee = traineeRepository.GetByKey(traineeId);

                    var predictionTrainingRepository = unitOfWork.GetRepository<IPredictionTrainingRepository>();
                    var entity = new PredictionTraining();

                    entity.Gender = trainee.Gender;
                    entity.AcademicInstitution = trainee.AcademicInstitutionId;
                    entity.AcademicMajor = trainee.AcademicMajorId;
                    entity.AcademicMinor = (trainee.AcademicMajor1 != null ? (double)trainee.AcademicMinorId : -1.0);
                    entity.SemesterNumber = trainee.SemesterNumber;
                    entity.TraineeId = traineeId;

                    var lastGrade = trainee.Grade.FirstOrDefault(g => g.SemesterNumber == trainee.SemesterNumber - 1);
                    entity.LastSemesterGrade = (lastGrade != null ? lastGrade.Grade1 : -1.0);

                    var alertRepository = unitOfWork.GetRepository<IAlertRepository>();
                    entity.AlertCount = alertRepository.GetAll().Count(a => a.AlertType == (int)AlertType.Intervention && a.TutorReport.TutorTrainee.TraineeId == traineeId);
                    entity.IsDroppedOut = 1.0;

                    predictionTrainingRepository.Add(entity);
                    unitOfWork.SaveChanges();

                    //If we got here - Yay! :)
                    status.Success = true;
                    status.Message = String.Format("רשומת האימון נוספה בהצלחה");
                    return status;
                }
            }
            catch (Exception ex)
            {
                if (status.Message == String.Empty)
                {
                    status.Message = String.Format("שגיאה במהלך הוספת רשומת אימון עקב נשירת חניך");
                }
                LogService.Logger.Error(status.Message, ex);
            }
            return status;
        }

        public StatusModel<List<TraineeModel>> GeneratePrediction(Area? area)
        {
            var status = new StatusModel<List<TraineeModel>>(false, String.Empty, new List<TraineeModel>());
            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var traineeRepository = unitOfWork.GetRepository<ITraineeRepository>();
                    var predictionTrainingRepository = unitOfWork.GetRepository<IPredictionTrainingRepository>();
                    var alertRepository = unitOfWork.GetRepository<IAlertRepository>();

                    var trainingRawData = predictionTrainingRepository.GetAll().ToList();
                    int NPoints = trainingRawData.Count();
                    const int NVars = 7;
                    const int NClasses = 2;
                    const int NTrees = 50;
                    const double R = 0.6;
                    int info = 0;
                    var forest = new alglib.dforest.decisionforest();
                    var report = new alglib.dforest.dfreport();

                    var trainingSet = new double[NPoints, 8];
                    for(int i = 0; i < NPoints; i++)
                    {
                        //Gender0,AcademicInstitution1,AcademicMajor2,AcademicMinor3,SemesterNumber4,SemesterGrade5,AlertCount6,IsDroppedOut7
                        trainingSet[i, 0] = trainingRawData[i].Gender;
                        trainingSet[i, 1] = trainingRawData[i].AcademicInstitution;
                        trainingSet[i, 2] = trainingRawData[i].AcademicMajor;
                        trainingSet[i, 3] = trainingRawData[i].AcademicMinor;
                        trainingSet[i, 4] = trainingRawData[i].SemesterNumber;
                        trainingSet[i, 5] = trainingRawData[i].LastSemesterGrade;
                        trainingSet[i, 6] = trainingRawData[i].AlertCount;
                        trainingSet[i, 7] = trainingRawData[i].IsDroppedOut;
                    }

                    alglib.dforest.dfbuildrandomdecisionforest(trainingSet, NPoints, NVars, NClasses, NTrees, R, ref info, forest, report);

                    if (info == 1)
                    {
                        var testRawData =
                            traineeRepository.GetQuery(
                                t => t.User.IsActive && (area == null || t.User.Area == (int?) area)).ToList();
                        int traineeCount = testRawData.Count();
                        var endangeredTrainees = new List<TraineeModel>();

                        for (int i = 0; i < traineeCount; i++)
                        {
                            //Gender0,AcademicInstitution1,AcademicMajor2,AcademicMinor3,SemesterNumber4,SemesterGrade5,AlertCount6
                            var testParameters = new double[7];
                            testParameters[0] = testRawData[i].Gender;
                            testParameters[1] = testRawData[i].AcademicInstitutionId;
                            testParameters[2] = testRawData[i].AcademicMajorId;
                            testParameters[3] = (double) (testRawData[i].AcademicMajor1 != null ? testRawData[i].AcademicMinorId : -1);
                            testParameters[4] = testRawData[i].SemesterNumber;
                            testParameters[5] = testRawData[i].Grade.OrderByDescending(g => g.SemesterNumber).Select(g => g.Grade1).FirstOrDefault();
                            testParameters[6] = alertRepository.GetQuery(a => a.AlertType == (int)AlertType.Intervention && a.TutorReport.TutorTrainee.TraineeId == testRawData[i].UserId).Count();
                            var prediction = new double[NClasses];
                            alglib.dforest.dfprocess(forest, testParameters, ref prediction);
                            if (prediction[1] > 0.65)
                            {
                                endangeredTrainees.Add(new TraineeModel(testRawData[i]));
                            }
                        }

                        //If we got here - Yay! :)
                        status.Success = true;
                        status.Message = String.Format("חיזוי בוצע בהצלחה");
                        return status;
                    }
                }
            }
            catch (Exception ex)
            {
                if (status.Message == String.Empty)
                {
                    status.Message = String.Format("שגיאה במהלך חיזוי חניכים בסיכון לנשירה");
                }
                LogService.Logger.Error(status.Message, ex);
            }
            return status;
        }
    }
}
