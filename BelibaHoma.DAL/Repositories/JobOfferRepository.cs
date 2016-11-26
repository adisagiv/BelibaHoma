using BelibaHoma.DAL.Interfaces;
using Catel.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BelibaHoma.DAL.Repositories
{
    public class JobOfferRepository : EntityRepositoryBase<JobOffer,int>, IJobOfferRepository
    {
        public JobOfferRepository(BelibaHomaDBEntities context)
            : base(context)
        {

        }
    }
}
