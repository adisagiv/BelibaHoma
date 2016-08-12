﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions.Numeric
{
    public static class NumericExtentions
    {

        public static string ToCommaSeparated(this int number)
        {
            return (number).ToString("N", new CultureInfo("en-US"));
        }

        public static string ToCommaSeparated(this long number)
        {
            return (number).ToString("N", new CultureInfo("en-US"));
        }

        public static string ToCommaSeparated(this double number)
        {
            string result = (number).ToString("N", new CultureInfo("en-US"));
            return result.Remove(result.Length - 3);
        }

        public static string ToCommaSeparated(this decimal number)
        {
            return (number).ToString("N", new CultureInfo("en-US"));
        }

        public static string ToCommaSeparated(this float number)
        {
            return (number).ToString("N", new CultureInfo("en-US"));
        }


    }
}
