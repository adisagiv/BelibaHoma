using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BelibaHoma.DAL.Interfaces;
using Catel.Data.Repositories;

namespace BelibaHoma.DAL.Repositories
{
    public class TutorRepository : EntityRepositoryBase<Tutor, int>, ITutorRepository
    {
        public TutorRepository(BelibaHomaDBEntities context)
            : base(context)
        {

        }
    }
}
