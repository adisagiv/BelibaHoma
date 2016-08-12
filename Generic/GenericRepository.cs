using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Generic.Interfaces;

namespace Generic.Repository
{
    /// <summary>
    /// Generic repository
    /// </summary>
    /// <typeparam name="T">The entity</typeparam>
    /// <typeparam name="TKey">the table key type</typeparam>
    public class GenericRepository<T, TKey> : IGenericRepository<T, TKey> where T : class
    {
        private readonly DbSet<T> _dbSet;
        //private DbContext _context;

        /// <summary>
        /// so we can call stored procedures
        /// shiran 23.10.14
        /// </summary>
        private readonly DbContext _context;

        private readonly bool _isAutoCommit;

        protected DbSet<T> DBSet
        {
            get { return this._dbSet; }
        }

        protected DbContext Context
        {
            get { return this._context; }
        }

        protected GenericRepository(DbContext context, bool isAutoCommit = true)
        {
            _context = context;
            _dbSet = context.Set<T>();
            _isAutoCommit = isAutoCommit;
        }

        /// <summary>
        /// Insert to DB
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(T entity)
        {
            this._dbSet.Add(entity);
            if (_isAutoCommit)
            {
                Commit();
            }
        }

        /// <summary>
        /// Insert to DB
        /// </summary>
        /// <param name="entity"></param>
        public async Task<int> InsertAsync(T entity)
        {
            this._dbSet.Add(entity);
            if (_isAutoCommit)
            {
                return await CommitAsync();
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Insert to DB
        /// </summary>
        /// <param name="entity"></param>
        protected async Task<int> InsertRangeAsync(IEnumerable<T> entity)
        {
            this._dbSet.AddRange(entity);
            if (_isAutoCommit)
            {
                return await CommitAsync();
            }
            else
            {
                return -1;
            }
        }
                
        /// <summary>
        /// Delete from DB
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(T entity)
        {
            
            this._dbSet.Remove(entity);

            if (_isAutoCommit)
            {
                Commit();
            }
        }

        /// <summary>
        /// Delete from DB
        /// </summary>
        /// <param name="entity"></param>
        public async Task<int> DeleteAsync(T entity)
        {

            this._dbSet.Remove(entity);

            if (_isAutoCommit)
            {
                return await CommitAsync();
            }
            else
            {
                return -1;
            }
        }
              

        /// <summary>
        /// Get all records for <typeparamref name="T"/>
        /// </summary>
        public IQueryable<T> All
        {
            get { return this._dbSet; }
        }

        /// <summary>
        /// Get All including properties
        /// </summary>
        /// <param name="includeProperies">Including properties</param>
        /// <returns></returns>
        public IQueryable<T> AllIncluding(params System.Linq.Expressions.Expression<Func<T, object>>[] includeProperies)
        {
            IQueryable<T> query = this._dbSet;

            return includeProperies.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        /// <summary>
        /// Save changes
        /// </summary>
        /// <returns></returns>
        public int Commit()
        {
            return _context.SaveChanges();
        }

        /// <summary>
        /// Save changes Async
        /// </summary>
        /// <returns></returns>
        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        /// <summary>
        /// Get by table key
        /// </summary>
        /// <param name="key">key to search by</param>
        /// <returns></returns>
        public T GetByKey(TKey key)
        {
            return _dbSet.Find(key);
        }

        /// <summary>
        /// Get by table key Async
        /// </summary>
        /// <param name="key">key to search by</param>
        /// <returns></returns>
        public async Task<T> GetByKeyAsync(TKey key)
        {
            return await _dbSet.FindAsync(key);
        }

        /// <summary>
        /// Call store procedure 
        /// </summary>
        /// <typeparam name="TResult">Expeted result type</typeparam>
        /// <param name="storeProcedureName">Store procedure name</param>
        /// <param name="parameters">store procedure parameters</param>
        /// <returns></returns>
        public IEnumerable<TResult> Exec<TResult>(string storeProcedureName, params object[] parameters)
        {
            //IEnumerable<TResult> result = _context.Database.SqlQuery<TResult>(String.Format("EXEC {0}", storeProcedureName),parameters);
            var aa = _context.GetType().GetMethods().Where(m => m.Name == storeProcedureName).ToList();
            MethodInfo method = _context.GetType().GetMethods().FirstOrDefault(m => m.Name == storeProcedureName && m.GetParameters().Count() == parameters.Count());

            IEnumerable<TResult> result = (IEnumerable<TResult>)method.Invoke(_context, parameters);
            
            //return result;
            return result;
        }

        public void Exec(string storeProcedureName, params object[] parameters)
        {
            //IEnumerable<TResult> result = _context.Database.SqlQuery<TResult>(String.Format("EXEC {0}", storeProcedureName),parameters);
            var aa = _context.GetType().GetMethods().Where(m => m.Name == storeProcedureName).ToList();
            MethodInfo method = _context.GetType().GetMethods().FirstOrDefault(m => m.Name == storeProcedureName && m.GetParameters().Count() == parameters.Count());
            method.Invoke(_context, parameters);
        }

    }
}
