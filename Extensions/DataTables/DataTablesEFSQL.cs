// Usage: (ContactPerson entity as example)
//
//  add: using DataTables;
//
//  Old way - same implementation - paging on asp server
//  ----------------------------------------------------
//  IQueryable<ContactPerson> q = dc.ContactPersons;
//  DataTableParser<ContactPerson> parser=new DataTableParser<ContactPerson>(Request, q.ToList());
//  return Json(parser.parse(), JsonRequestBehavior.AllowGet);
//
//
//  New syntax - paging on sql server
//  ---------------------------------
//  FormatedList q = dc.ContactPersons.ToDataTables();
//  return Json(q, JsonRequestBehavior.AllowGet);
//
//  *Note: ToDataTables() takes true/false to use new implementation (default: true).
//            
//
//  Custom Columns
//  --------------
//  Use dtSelect() to define what columns to return:
//
//  FormatedList dt = dc.ContactPersons
//      .dtSelect   
//      (
//          x => x.Id,
//          x => x.LastName + " " + x.FirstName,
//          x => x.DateOfBirth,
//          x => x.ContactPersonType
//      )                 
//      .ToDataTables();
//  
//  *Note: Sorting and search will apply to these expressions.  
//  *Note2: Use only SQL-safe expressions. 
//
//  
//  Using a Model
//  -------------
//  Use dtToList() to actualize params.
//  Use regular Select() to project to your Model.
//  Use ToDataTables() to convert to formatted list.  
//
//  FormatedList dt = dc.ContactPersons
//      .dtSelect                   //  sorts/searches on SQL-Safe specified fields/expression
//      (
//          x => x.Id,
//          x => x.LastName + x.FirstName,
//          x => x.DateOfBirth,
//          x => x.ContactPersonType
//      )                                   
//      .dtToList()                 // query is executed against sql server.
//      .Select(                    // executed on paged results in memory.
//          e=>new ContactPersonModel
//          {
//              Id=e.Id, ...   
//          }
//      )    
//      .ToDataTables();            // Formatted list according to model
//
//  * Note: projection to model happens after search/pageing.
//  *       doesn't have to be SQL-safe.
//
//
//  Custom search
//  -------------
//  Use dbWhere() to define which columns/expressions to search:
//
//  FormatedList dt = dc.ContactPersons
//      .dtWhere
//      (
//         x => x.LastName + x.FirstName
//      )
//      .dtSelect   
//      (
//          x => x.Id,
//          x => x.LastName + " " + x.FirstName,
//          x => x.DateOfBirth,
//          x => x.ContactPersonType
//      )                 
//      .ToDataTables();
//
//  * Only the given expressions will be searched.
//  * If you wish to disable search, use: x => false.
//  * If you need individual column searches, provide dbWhere() with expressions in the same order of the columns.
//  * You can use x = > false for columns you do not wish to search.
//  
//
//  Custom sorts
//  ------------
//  Use dtOrderBy() to define which columns/expressions to sort for each column:
//
//  FormatedList dt = dc.ContactPersons
//      .dtWhere
//      (
//         x => x.LastName + x.FirstName
//      ) 
//      .dtOrderBy                          
//      (
//          x => x.Id,
//          x => x.LastName + x.FirstName,
//          x => x.DateOfBirth,
//          x => x.ContactPersonType
//      )
//      .ToDataTables();
//
//  * Must be in the same order of columns.
//  * Must provide to all columns. 
//  * Use x => false to disable sorting on column.
//  * dtOrderBy() substitutes for dtSelect() if it is ommited.
//
//
//  Alt. to model
//  -------------
//  ToDataTables() can accept expressions instead of using model.
//
//  var dt = dc.ContactPersons
//      .dtWhere
//      (
//          x => x.LastName + x.FirstName,
//          x => x.DateOfBirth,
//          x => false
//      ) 
//      .dtOrderBy                          
//      (
//          x => x.LastName + x.FirstName,
//          x => x.DateOfBirth,
//          x => x.ContactPersonType
//      )
//      .ToDataTables(
//          x => (x.FirstName ?? "") + " " + (x.LastName ?? ""),
//          x => (x.DateOfBirth.HasValue) ? x.DateOfBirth.Value.ToString("dd/MM/yyyy") : "N/A",
//          x => (x.ContactPersonType == 1) ? "Business" : "Other"
//      );
//
//  return Json(dt, JsonRequestBehavior.AllowGet);
//
//
//  Limitations:
//  Because search is done on sql server there are limitations to search:
//  Strings - uses contains (LIKE '%search%').
//  DateTime - equal by date, year or month.
//  Others - equal by value.
// 
//  Extras:
//  Simple sliding cache (off by default):
//      DataTablesLinqExtensions.UseCache = false;
//      DataTablesLinqExtensions.SlidingCacheMins = 10;
//  Search by day of week on dates (sunday, monday...)


using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.SqlServer;


namespace DataTables
{
    public static class DataTablesConst
    {
        public const string INDIVIDUAL_SEARCH_KEY_PREFIX = "sSearch_";
        public const string INDIVIDUAL_SORT_KEY_PREFIX = "iSortCol_";
        public const string INDIVIDUAL_SORT_DIRECTION_KEY_PREFIX = "sSortDir_";
        public const string DISPLAY_START = "iDisplayStart";
        public const string DISPLAY_LENGTH = "iDisplayLength";
        public const string ECHO = "sEcho";
        public const string ASCENDING_SORT = "asc";
    }

    public class FormatedList
    {
        public FormatedList()
        {
        }

        public int sEcho { get; set; }
        public int iTotalRecords { get; set; }
        public int iTotalDisplayRecords { get; set; }
        public List<List<string>> aaData { get; set; }
        public string sColumns { get; set; }

        public void Import(string[] properties)
        {
            sColumns = string.Join(",", properties);
        }
    }

    public static class DataTablesLinqExtensions
    {
        public static int SlidingCacheMins = 10;
        public static bool UseCache = false;

        public static IQueryable<T> dtWhere<T>(this IQueryable<T> source,NameValueCollection request, params Expression<Func<T, object>>[] searches)
        {
            string key = RequestKey<T>(request);
            RequestDTWheres.Add(key, searches);
            return source;
        }

        public static IQueryable<T> dtOrderBy<T>(this IQueryable<T> source, NameValueCollection request, params Expression<Func<T, object>>[] sorts)
        {
            string key = RequestKey<T>(request);
            RequestDTOrderBys.Add(key, sorts);
            return source;
        }

        public static IQueryable<T> dtSelect<T>(this IQueryable<T> source, NameValueCollection request, params Expression<Func<T, object>>[] selects)
        {
       
            string key = RequestKey<T>(request);
            RequestDTSelects.Add(key, selects);
            return source;
        }

        public static IQueryable<T> dtSelect<T>(this IQueryable<T> source,NameValueCollection request, params string[] selects)
        {
            string key = RequestKey<T>(request);
            var paramExpression = Expression.Parameter(typeof(T), "val");
            PropertyInfo[] _properties = selects.Select(s=>typeof(T).GetProperty(s)).ToArray();
            Expression<Func<T, object>>[] searches = _properties.Select(p => Expression.Lambda<Func<T, object>>(Expression.Convert(Expression.Property(paramExpression, p), typeof(object)), paramExpression)).ToArray();
            RequestDTSelects.Add(key, searches);
            return source;
        }

        // Applies all params and formats list (does everything)
        public static FormatedList ToDataTables<T>(this IQueryable<T> source, NameValueCollection request,int count = 0)
        {
            NameValueCollection req = GetRequest(request); if (req == null) new FormatedList();

            string key = RequestKey<T>(req);

            // if query was already actualized
            if (RequestActualized.Keys.Any(k=>k==key))
            {
                RequestActualized.Remove(key);
                return source.ToFormatedList(req,count);
            }

            var result = source.ActualizeDataTablesParams(req, count);
            RequestActualized.Remove(key);
            return result.ToFormatedList(req,count);
        }
        
        public static FormatedList ToDataTables<T>(this IQueryable<T> source, System.Collections.Specialized.NameValueCollection dtParams,  params Expression<Func<T, object>>[] selectsLocal)
        {
            NameValueCollection request = GetRequest(dtParams); if (request == null) return new FormatedList();
            string key = RequestKey<T>(request);
            RequestDTLocalSelects.Add(key, selectsLocal);

            return ToDataTables(source, request);
        }

        // Actualizes params.
        public static IQueryable<T> dtToList<T>(this IQueryable<T> source, NameValueCollection request=null)
        {
            request = GetRequest(request); if (request == null) return source;

            string key=RequestKey<T>(request);

            IQueryable<T> temp = source.ActualizeDataTablesParams(request);

            RequestDTWheres.Remove(key);
            RequestDTOrderBys.Remove(key);
            RequestDTSelects.Remove(key);
            return temp;
        }

        // Members
        private static Dictionary<string, int> RequestCount = new Dictionary<string, int>();
        private static Dictionary<string, int> RequestDisplayCount = new Dictionary<string, int>();
        private static Dictionary<string, bool> RequestActualized = new Dictionary<string, bool>();
        private static Dictionary<string, Expression[]> RequestDTSelects = new Dictionary<string, Expression[]>();
        private static Dictionary<string, Expression[]> RequestDTLocalSelects = new Dictionary<string, Expression[]>();
        private static Dictionary<string, Expression[]> RequestDTWheres = new Dictionary<string, Expression[]>();
        private static Dictionary<string, Expression[]> RequestDTOrderBys = new Dictionary<string, Expression[]>();

        // Flow
        private static IQueryable<T> ActualizeDataTablesParams<T>(this IQueryable<T> source, NameValueCollection request,int count = 0)
        {
            request = GetRequest(request); if (request == null) return source;

            string key = RequestKey<T>(request);
            string cacheKey = CacheKey<T>(request);

            IQueryable<T> temp = null;

            if (source is ObjectQuery)
                try { (source as ObjectQuery).Context.Connection.Open(); }
                catch { }

            var result = source.ApplyDataTablesParams(request, count);
                
            temp = result.AsQueryable();

            if (source is ObjectQuery)
                try { (source as ObjectQuery).Context.Connection.Close(); }
                catch { }

            RequestActualized.Add(key, true);
            return temp;
        }

        private static IQueryable<T> ApplyDataTablesParams<T>(this IQueryable<T> source, NameValueCollection request,int count = 0)
        {
            request = GetRequest(request); if (request == null) return source;

            var search = source.ApplyDataTablesSearch(request, count);
            var sort = search.ApplyDataTablesSort(request, count);
            var paging = sort.ApplyDataTablesPaging(request, count);

            return paging;
        }

        private static IQueryable<T> ApplyDataTablesSort<T>(this IQueryable<T> source, NameValueCollection request,int? count = 0)
        {
            request = GetRequest(request); if (request == null) return source;

            var sort = source.ApplyDataTablesSortImp(request,count);

            return sort;
        }

        private static IQueryable<T> ApplyDataTablesSearch<T>(this IQueryable<T> source, NameValueCollection request,int count = 0)
        {
            request = GetRequest(request); if (request == null) return source;
            string key=RequestKey<T>(request);
            if (RequestCount.Keys.All(k => k != key))
            {
                if (count == 0)
                {
                    count = source.Count();
                }
                RequestCount.Add(key, count);
            }
            
            IQueryable<T> search=source.Where(GetSearchExpression<T>(source, request));
            return search;
        }

        private static IQueryable<T> ApplyDataTablesPaging<T>(this IQueryable<T> source, NameValueCollection request,int count = 0)
        {
            try
            {
                request = GetRequest(request); if (request == null) return source;

                string key = RequestKey<T>(request);
                if (RequestCount.Keys.All(k => k != key))
                {
                    if (count == 0)
                    {
                        count = source.Count();
                    }
                    RequestCount.Add(key, count);
                }
                if (RequestDisplayCount.Keys.All(k => k != key))
                {
                    if (count == 0)
                    {
                        count = source.Count();
                    }
                    RequestDisplayCount.Add(key, count);
                }

                IQueryable<T> paging = source.ApplyDataTablesPagingImp(request);
                return paging;
            }
            catch { return null; }
        }

        private static FormatedList ToFormatedList<T>(this IQueryable<T> source, NameValueCollection request,int count = 0)
        {
            request = GetRequest(request); if (request == null) return null;
            return source.ToFormatedListImp(request,count);
        }

        // Implementation
        private static Expression<Func<T, bool>> GetSearchExpression<T>(this IQueryable<T> source, NameValueCollection request)
        {
            request = GetRequest(request); if (request == null) return x => true;

            string search = request["sSearch"];
            var individualSearches = request.AllKeys.Where(x => x.StartsWith(DataTablesConst.INDIVIDUAL_SEARCH_KEY_PREFIX));
            
            // Check if there's any search to be made
                        
            // No search to be made, return true
            if (string.IsNullOrEmpty(request["sSearch"]) && individualSearches.Count(i => !string.IsNullOrEmpty(request[i])) == 0)
                return x => true;

            string key = RequestKey<T>(request);
            Expression[] searches;
            Expression compoundExpression; 
            if(string.IsNullOrEmpty(request["sSearch"]))
                compoundExpression= Expression.Constant(true);
            else
                compoundExpression= Expression.Constant(false);
            Expression compoundExpressionIndividual = Expression.Constant(true);
            var paramExpression = Expression.Parameter(typeof(T), "val");

            // if dtWhere was used
            Expression<Func<T, object>>[] wheres=null;
            try
            {
                wheres = (Expression<Func<T, object>>[])RequestDTWheres[key];
            }
            catch { }

            if (wheres != null)
            {
                searches = wheres.Select(w => w.Body).ToArray();
            }
            else
            {
                try
                {
                    wheres = (Expression<Func<T, object>>[])RequestDTSelects[key];
                }
                catch { }
                if (wheres != null)
                {
                    searches = wheres.Select(w => w.Body).ToArray();
                }
                else
                {
                    PropertyInfo[] _properties = typeof(T).GetProperties().Where(p => p.Name != "EntityKey" && p.Name != "EntityState").ToArray();
                    searches = _properties.Select(p => Expression.Property(paramExpression, p)).ToArray();
                }
            }

            if (!string.IsNullOrEmpty(search))
                foreach(Expression x in searches)
                    compoundExpression = Expression.Or(compoundExpression, GetSearchExpressionForProp(paramExpression, x, search));

            // individual searches
            foreach (string iSearchKey in individualSearches)
            {
                try
                {
                    string iSearch = request[iSearchKey];
                    if (!string.IsNullOrEmpty(iSearch))
                    {
                        int index = int.Parse(iSearchKey.Replace(DataTablesConst.INDIVIDUAL_SEARCH_KEY_PREFIX, string.Empty));
                        compoundExpressionIndividual = Expression.And(compoundExpressionIndividual, GetSearchExpressionForProp(paramExpression, searches[index], iSearch));
                    }
                }
                catch { continue; }
            }

            compoundExpression = Expression.And(compoundExpression, compoundExpressionIndividual);
           
            // compile the expression into a lambda 
            return Expression.Lambda<Func<T, bool>>(compoundExpression, paramExpression);
        }

        private static Expression GetSearchExpressionForProp(ParameterExpression param, Expression x, string search)
        {
            ReplaceParameter visitor = new ReplaceParameter();
            var pExpression = (x.NodeType == ExpressionType.Convert) ? visitor.Modify((x as UnaryExpression).Operand, param) : visitor.Modify(x, param);
            Type propType = pExpression.Type;

            if (propType.BaseType == typeof(Enum))
            {
                var searchExpression = Expression.Constant(search.Trim().ToLower());
                var toStringExpression = Expression.Call(pExpression, typeof(Enum).GetMethods().First(m => m.Name == "ToString"));
                var toLowerExpression = Expression.Call(toStringExpression, typeof(string).GetMethods().First(m => m.Name == "ToLower"));
                var pNotNullExpression = Expression.Constant(true);//Expression.NotEqual(pExpression, Expression.Constant(null));
                var pContainsExpression = Expression.Call(toLowerExpression, typeof(string).GetMethod("Contains"), searchExpression);
                return Expression.AndAlso(pContainsExpression, pNotNullExpression);
            }
    
            if (propType == typeof(DateTime) || propType == typeof(DateTime?))
                return BuildDateTimeExpression(x, param, search);

            if (propType == typeof(string))
            {
                var searchExpression = Expression.Constant(search.Trim().ToLower());
                var toLowerExpression = Expression.Call(pExpression, typeof(string).GetMethods().First(m => m.Name == "ToLower"));
                var pNotNullExpression = Expression.NotEqual(pExpression, Expression.Constant(null));
                var pContainsExpression = Expression.Call(toLowerExpression, typeof(string).GetMethod("Contains"), searchExpression);
                return Expression.IsTrue(pContainsExpression);
            }
            else
            {
                try
                {
                    object converted = null;
                    if (Nullable.GetUnderlyingType(propType) != null)
                    {

                        converted = Convert.ChangeType(search.ToLower(), Nullable.GetUnderlyingType(propType));
                        return Expression.AndAlso(Expression.NotEqual(pExpression, Expression.Constant(null)), Expression.Equal(Expression.Property(pExpression, "Value"), Expression.Constant(converted)));
                    }
                    else
                    {
                        converted = Convert.ChangeType(search.ToLower(), propType);
                        return Expression.Equal(pExpression, Expression.Constant(converted));
                    }

                }
                catch { return Expression.Constant(false); }
            }
        }

        private static Expression BuildDateTimeExpression(Expression x, ParameterExpression param, string search)
        {
            ReplaceParameter visitor = new ReplaceParameter();
            var pExpression = (x.NodeType == ExpressionType.Convert) ? visitor.Modify((x as UnaryExpression).Operand, param) : visitor.Modify(x, param);
            Type propType = pExpression.Type;

            try
            {
                DateTime converted;
                bool bDateTime = DateTime.TryParse(search, out converted);
                if (bDateTime)
                {
                    if (propType == typeof(DateTime?))
                    {
                        Expression notNull = Expression.NotEqual(pExpression, Expression.Constant(null));
                        Expression equal = Expression.Equal(Expression.Property(pExpression, "Value"), Expression.Constant(converted));
                        MethodInfo miTruncateTime = typeof(DbFunctions).GetMethods().First(m => m.Name == "TruncateTime" && m.GetParameters().First().ParameterType == typeof(DateTime?));
                        Expression dateEqual = Expression.Equal(Expression.Call(null, miTruncateTime, pExpression), Expression.Convert(Expression.Constant(converted.Date), typeof(DateTime?)));

                        Expression exp;
                        exp = Expression.AndAlso(notNull, equal);
                        exp = Expression.Or(exp, Expression.AndAlso(notNull, dateEqual));
                        return exp;
                    }
                    else
                    {
                        Expression equal = Expression.Equal(pExpression, Expression.Constant(converted));
                        MethodInfo miTruncateTime = typeof(DbFunctions).GetMethods().First(m => m.Name == "TruncateTime" && m.GetParameters().First().ParameterType == typeof(DateTime?));
                        Expression dateEqual = Expression.Equal(Expression.Call(null, miTruncateTime, Expression.Convert(pExpression, typeof(DateTime?))), Expression.Constant(converted.Date));

                        Expression exp;
                        exp = equal;
                        exp = Expression.Or(exp, dateEqual);
                        return exp;
                    }
                }
                int datePart;
                bool bDatePart = int.TryParse(search, out datePart);
                if (bDatePart)
                {
                    if (propType == typeof(DateTime?))
                    {
                        Expression notNull = Expression.NotEqual(pExpression, Expression.Constant(null));
                        Expression yearEqual = Expression.Equal(Expression.Property(Expression.Property(pExpression, "Value"), "Year"), Expression.Constant(datePart));
                        Expression monthEqual = Expression.Equal(Expression.Property(Expression.Property(pExpression, "Value"), "Month"), Expression.Constant(datePart));
                        Expression exp;
                        exp = Expression.AndAlso(notNull, yearEqual);
                        exp = Expression.Or(exp, Expression.AndAlso(notNull, monthEqual));
                        return exp;
                    }
                    else
                    {
                        Expression yearEqual = Expression.Equal(Expression.Property(pExpression, "Year"), Expression.Constant(datePart));
                        Expression monthEqual = Expression.Equal(Expression.Property(pExpression, "Month"), Expression.Constant(datePart));

                        Expression exp;
                        exp = yearEqual;
                        exp = Expression.Or(exp, monthEqual);
                        return exp;
                    }
                }
                int day = -1;
                string[] days = Enum.GetNames(typeof(DayOfWeek));
                for (int i = 0; i < days.Length; i++)
                {
                    if (days[i].ToLower() == search.Trim().ToLower())
                        day = i + 1;
                }
                if (day > -1)
                {
                    if (propType == typeof(DateTime?))
                    {
                        Expression notNull = Expression.NotEqual(pExpression, Expression.Constant(null));
                        MethodInfo miSqlDatePart = typeof(SqlFunctions).GetMethods().First(m => m.Name == "DatePart" && m.GetParameters()[1].ParameterType == typeof(DateTime?));
                        Expression dayEqual = Expression.Equal(Expression.Property(Expression.Call(null, miSqlDatePart, Expression.Constant("weekday"), pExpression), "Value"), Expression.Constant(day));
                        return Expression.AndAlso(notNull, dayEqual);
                    }
                    else
                    {
                        MethodInfo miSqlDatePart = typeof(SqlFunctions).GetMethods().First(m => m.Name == "DatePart" && m.GetParameters()[1].ParameterType == typeof(DateTime?));
                        Expression dayEqual = Expression.Equal(Expression.Property(Expression.Call(null, miSqlDatePart, Expression.Constant("weekday"), Expression.Convert(pExpression, typeof(DateTime?))), "Value"), Expression.Constant(day));
                        return dayEqual;
                    }
                }
                return Expression.Constant(false);
            }
            catch { return Expression.Constant(false); }
        }

        private static IQueryable<T> ApplyDataTablesSortImp<T>(this IQueryable<T> source, NameValueCollection request,int? count = 0)
        {
            request = GetRequest(request); if (request == null) return source;

            string key = RequestKey<T>(request);
            Expression<Func<T, object>>[] orderbys = null;
            try
            {
                orderbys = (Expression<Func<T, object>>[])RequestDTOrderBys[key];
            }
            catch {
                try
                {
                    orderbys = (Expression<Func<T, object>>[])RequestDTSelects[key];
                }
                catch { }
            }

            PropertyInfo[] _properties = typeof(T).GetProperties();
            // enumerate the keys for any sortations
            foreach (string iSortKey in request.AllKeys.Where(x => x.StartsWith(DataTablesConst.INDIVIDUAL_SORT_KEY_PREFIX)))
            {
                // column number to sort (same as the array)
                int sortcolumn = int.Parse(request[iSortKey]);

                // ignore malformatted values
                if (sortcolumn < 0 || sortcolumn >= _properties.Length)
                    continue;

                // get the direction of the sort
                string sortdir = request[DataTablesConst.INDIVIDUAL_SORT_DIRECTION_KEY_PREFIX + iSortKey.Replace(DataTablesConst.INDIVIDUAL_SORT_KEY_PREFIX, string.Empty)];

                var param = Expression.Parameter(typeof(T), "val");
                Expression pExpression;
                if (orderbys!=null && orderbys.Count() > sortcolumn)
                {
                    Expression x = orderbys[sortcolumn].Body;
                    ReplaceParameter visitor = new ReplaceParameter();
                    pExpression = (x.NodeType == ExpressionType.Convert) ? visitor.Modify((x as UnaryExpression).Operand, param) : visitor.Modify(x, param);
                }
                else
                {
                    pExpression = Expression.MakeMemberAccess(param, _properties[sortcolumn]);
                }

                Type propType = pExpression.Type;

                var orderByExp = Expression.Lambda(pExpression, param);
                MethodCallExpression resultExp;

                if (string.IsNullOrEmpty(sortdir) || sortdir.Equals(DataTablesConst.ASCENDING_SORT, StringComparison.OrdinalIgnoreCase))
                    resultExp = Expression.Call(typeof(Queryable), "OrderBy", new Type[] { typeof(T), propType }, source.Expression, Expression.Quote(orderByExp));
                else
                    resultExp = Expression.Call(typeof(Queryable), "OrderByDescending", new Type[] { typeof(T), propType }, source.Expression, Expression.Quote(orderByExp));

                source = source.Provider.CreateQuery<T>(resultExp);
            }

            return source;
        }

        private static IQueryable<T> ApplyDataTablesPagingImp<T>(this IQueryable<T> source, NameValueCollection request = null,int count = 0)
        {
            request = GetRequest(request); if (request == null) return source;

            int skip = 0, take = 10;
            int.TryParse(request[DataTablesConst.DISPLAY_START], out skip);
            int.TryParse(request[DataTablesConst.DISPLAY_LENGTH], out take);

            /// incase All is selected
            if (take == -1)
            {
                if (count == 0)
                {
                    count = source.Count();
                }
                take = count;
            }

            return source.Skip(skip).Take(take);
        }

        private static FormatedList ToFormatedListImp<T>(this IQueryable<T> source, NameValueCollection request = null, int count = 0, int displayCount = 0)
        {
            request = GetRequest(request); if (request == null) return null;
            PropertyInfo[] _properties = typeof(T).GetProperties();
            var list = new FormatedList();

            string key = RequestKey<T>(request);
            Expression<Func<T, object>>[] selects = null;
            try
            {
                selects = (Expression<Func<T, object>>[])RequestDTLocalSelects[key];
            }
            catch
            {
                try
                {
                    selects = (Expression<Func<T, object>>[])RequestDTSelects[key];
                }
                catch{
                    try
                    {
                        selects = (Expression<Func<T, object>>[])RequestDTOrderBys[key];
                    }
                    catch { }
                }
            }
            finally
            {
                RequestDTSelects.Remove(key);
                RequestDTOrderBys.Remove(key);
                RequestDTWheres.Remove(key);
                RequestDTLocalSelects.Remove(key);
            }

            if (selects!=null && selects.Any())
            {
                string[] propNames = new string[selects.Count()];
                for (int i = 0; i < selects.Count(); i++)
                {
                    propNames[i] = "Property" + i.ToString();
                }
                list.Import(propNames);
            }
            else
                list.Import(_properties.Select(x => x.Name).ToArray());

            // parse the echo property (must be returned as int to prevent XSS-attack)
            list.sEcho = request.AllKeys.Contains(DataTablesConst.ECHO) ? int.Parse(request[DataTablesConst.ECHO]) : 0;
            int pos = key.IndexOf('&');
            string cacheKey = pos >= 0 ? key.Remove(key.LastIndexOf('&')).Remove(0, pos + 1) : key;
            int? cacheCount = null;
            int? cacheDisplayCount = null;
            
            bool keyfound = RequestCount.Keys.Any(k => k == key);
            list.iTotalRecords = (count == 0) ? (keyfound ? RequestCount[key] : source.Count()) : count;
            if (keyfound) RequestCount.Remove(key);

            keyfound = RequestDisplayCount.Keys.Any(k => k == key);
            list.iTotalDisplayRecords = (count == 0) ? (keyfound ? RequestDisplayCount[key] : source.Count()) : count;
            if (keyfound) RequestDisplayCount.Remove(key);

            list.aaData = source.ToListOfStrings(selects);

            return list;
        }

        public static List<List<string>> ToListOfStrings<T>(this IQueryable<T> source, params Expression<Func<T, object>>[] props)
        {
            Expression<Func<T, List<string>>> proj;
            if (props==null || !props.Any())
            {
                PropertyInfo[] _properties = typeof(T).GetProperties();

                proj = value => _properties.Select
                (
                    prop => (prop.GetValue(value, new object[0]) ?? string.Empty).ToString()
                ).ToList();
            }
            else
            {
                proj = val => props.Select
                (
                    ex => (ex.Compile().Invoke(val) ?? string.Empty).ToString()
                ).ToList();

            }
            var result = source.Select(proj);
            return result.ToList();//source.ToList().AsQueryable().Select(proj).ToList();
        }

        // Utils
        private static NameValueCollection GetRequest(NameValueCollection request)
        {
            return request;
        }

        private static string RequestKey<T>(NameValueCollection req)
        {
            return GetRequest(req).ToString();
        }

        private static string CacheKey<T>(NameValueCollection request)
        {
            string key = GetRequest(request).ToString();
            int i = key.IndexOf('&');
            if (i >= 0)
            {
                key = key.Remove(key.LastIndexOf('&')).Remove(0, i + 1);
            }
            key = typeof(T).FullName + "," + key;
            return key;
        }

        class ReplaceParameter : ExpressionVisitor
        {
            Expression paramExpression = null;
            Expression sourceExpression = null;

            public Expression Modify(Expression expression, Expression param)
            {
                paramExpression = param;
                sourceExpression = expression;
                return Visit(expression);
            }

            protected override Expression VisitMember(MemberExpression node)
            {
                if (node.Member.MemberType != System.Reflection.MemberTypes.Property)
                    return node;

                return Expression.MakeMemberAccess(paramExpression, paramExpression.Type.GetProperty(node.Member.Name));
            }

        }

    }
    
}