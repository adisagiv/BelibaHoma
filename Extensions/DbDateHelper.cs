using System.Data.SqlTypes;


namespace Extensions
{
    public static class DbDateHelper
    {
        /// <summary>
        /// Replaces any date before 01.01.1753 with a Nullable of 
        /// DateTime with a value of null.
        /// </summary>
        /// <param name="date">Date to check</param>
        /// <returns>Input date if valid in the DB, or Null if date is 
        /// too early to be DB compatible.</returns>
        public static System.DateTime? ToNullIfTooEarlyForDb(this System.DateTime date)
        {
            return (date >= (System.DateTime)SqlDateTime.MinValue) ? date : (System.DateTime?)null;
        }         
    }
}