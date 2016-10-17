using System;

namespace Extensions
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Is type of simple type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsSimpleType(this Type type)
        {
            if (type.IsNullable())
            {
                // nullable type, check if the nested type is simple.
                return IsSimpleType(type.GetGenericArguments()[0]);
            }
            return type.IsPrimitive
                   || type.IsEnum
                   || type == typeof(string)
                   || type == typeof(decimal);
        }
        /// <summary>
        /// Is type Assignable From type <paramre name="TSource"/>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsAssignableFrom<TSource>(this Type type)
        {
            return type != null && typeof(TSource).IsAssignableFrom(type);
            
        }
        /// <summary>
        /// Is type is of type string
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsString(this Type type)
        {
            return type != null && typeof (string) == type;
            
        }

        /// <summary>
        /// Is type is of type nullable
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNullable(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

        }

        /// <summary>
        /// Determines if a type is numeric.  Nullable numeric types are considered numeric.
        /// </summary>
        /// <remarks>
        /// Boolean is not considered numeric.
        /// </remarks>
        public static bool IsNumericType(this Type type)
        {
            if (type == null)
            {
                return false;
            }

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
                case TypeCode.Object:
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        return IsNumericType(Nullable.GetUnderlyingType(type));
                    }
                    return false;
            }
            return false;
        }
    }
}