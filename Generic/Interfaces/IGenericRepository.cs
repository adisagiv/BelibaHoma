using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Generic.Interfaces
{
    public interface IGenericRepository<T,TKey> : IDisposable
    {
        void Insert(T entity);
        void Delete(T entity);
        //void Update(T entity);
        T GetByKey(TKey id);
        IQueryable<T> All { get; }
        IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperies);
        int Commit();
        Task<int> CommitAsync();
        IEnumerable<TResult> Exec<TResult>(string storeProcedureName, params object[] parameters);
    }
}
