using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions.DateTime
{
    public static class DateTimeExtensions
    {
        public static long Utc(this System.DateTime date)
        {
            return (long)(date.ToUniversalTime() - new System.DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
        }

        public static System.DateTime ToDateUnixTime(this long utc)
        {
            //return new DateTime(1970, 1, 1, 0, 0, 0).AddMinutes((DateTime.Now - DateTime.UtcNow).TotalMinutes).AddMilliseconds(utc);
            var epoch = new System.DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(utc);
        }

        public static System.DateTime ToDateFromUtc(this long utc)
        {
            //return new DateTime(1970, 1, 1, 0, 0, 0).AddMinutes((DateTime.Now - DateTime.UtcNow).TotalMinutes).AddMilliseconds(utc);
            System.DateTime dtDateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddMilliseconds(utc).ToLocalTime();
            return dtDateTime;
        }

        public static System.DateTime ToDateFromUtc(this string utcStr)
        {
            try
            {
                var utc = long.Parse(utcStr);
                return utc.ToDateFromUtc();
            }
            catch (System.Exception ex)
            {

                throw new InvalidCastException(String.Format("The value {0}, is not a valid Utc value", utcStr));
            }
        }

        public static System.DateTime ToDateFromUtc(this double utc)
        {
            //return new DateTime(1970, 1, 1, 0, 0, 0).AddMinutes((DateTime.Now - DateTime.UtcNow).TotalMinutes).AddMilliseconds(utc);
            System.DateTime dtDateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddMilliseconds(utc).ToLocalTime();
            return dtDateTime;
        }




        static GregorianCalendar _gc = new GregorianCalendar();
        public static int GetWeekOfMonth(this System.DateTime time)
        {
            System.DateTime first = new System.DateTime(time.Year, time.Month, 1);
            var weekNumber = time.GetWeekOfYear() - first.GetWeekOfYear() + 1;
            return weekNumber;
        }

        static int GetWeekOfYear(this System.DateTime time)
        {
            return _gc.GetWeekOfYear(time, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
        }

        //public static int WeekNumber(this System.DateTime TargetDate)
        //{
        //    var weekNumber =  (TargetDate.Day - 1) / 7 + 1;
        //    return weekNumber;
        //}


    }
}
