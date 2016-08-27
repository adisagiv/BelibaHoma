using BelibaHoma.DAL.Interfaces;
using Catel.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BelibaHoma.DAL.Repositories
{
    public class UserRepository : EntityRepositoryBase<User,int>, IUserRepository
    {
        public UserRepository(BelibaHomaDBEntities context)
            : base(context)
        {

        }
    }
}
