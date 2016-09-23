using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BelibaHoma.DAL.Interfaces;
using Catel.Data.Repositories;

namespace BelibaHoma.DAL.Repositories
{
    public class TraineeRepository : EntityRepositoryBase<Trainee, int>, ITraineeRepository
    {
        public TraineeRepository(BelibaHomaDBEntities context)
            : base(context)
        {

        }
    }
}
