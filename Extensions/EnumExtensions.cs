using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Extensions.Enums
{
    public static class EnumExtensions
    {
        public static string ToDescription(this Enum value)
        {
            var enumType = value.GetType();
            var field = enumType.GetField(value.ToString());
            var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length == 0 ? value.ToString() : ((DescriptionAttribute)attributes[0]).Description;
        }

        public static string ToDetails(this Enum value)
        {
            var enumType = value.GetType();
            var field = enumType.GetField(value.ToString());
            var attributes = field.GetCustomAttributes(typeof(DetailsAttribute), false);
            return attributes.Length == 0 ? value.ToString() : ((DetailsAttribute)attributes[0]).Details;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class DetailsAttribute : Attribute
    {
        private readonly string _details;

        public string Details 
        {
            get { return _details; }
        }

        public DetailsAttribute(string details)
        {
            this._details = details;
        }
    }
}
