using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BelibaHoma.BLL.Enums;
using Generic.Models;
using BelibaHoma.BLL.Models;


namespace BelibaHoma.BLL.Interfaces
{
    public interface IJobOfferService
    {
        /// <summary>
        /// Get all Job Offers from the db
        /// </summary>
        /// <returns></returns>
        List<JobOfferModel> Get(JobArea? JobArea);

        /// <summary>
        /// Add new Job Offer to db
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        StatusModel Add(JobOfferModel model);

        /// <summary>
        /// Update Job Offer in db
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedModel"></param>
        /// <returns></returns>
        StatusModel Update(int id, JobOfferModel updatedModel);

        /// <summary>
        /// Get the JobOfferModel according to the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        StatusModel<JobOfferModel> Get(int id);
    }
}
