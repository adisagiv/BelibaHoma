using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Generic.GenericModel.Interfaces;
using Generic.Interfaces;

namespace Generic.GenericModel.Models
{
    internal class ResolveModel
    {
        private readonly PropertyInfo _property;
        public PropertyInfo PropertyInfo {
            get { return _property; }
        }
        public Type PropertyType 
        { 
            get 
            {
                if (_property != null)
                {
                    return _property.PropertyType;
                }
                return null;
        }  }

        public bool IsGenericModel { get { return _property != null && PropertyType.IsGenericModel(); } }

        public bool IsEnumerable
        {
            get { return _property != null && typeof(IEnumerable).IsAssignableFrom(PropertyType); }
        }

        public bool IsEnum { get { return _property != null && PropertyType.IsEnum; } }

        public bool IsString { get { return _property != null && typeof (string) == PropertyType; } }

        public bool IsNullable {
            get { return _property != null && PropertyType.IsGenericType && PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>); }
        }

        public bool CanWrite { get { return _property != null && PropertyInfo.CanWrite; } }

        public bool IsSimple
        {
            get { return _property != null && PropertyType.IsSimpleType(); }
        }

        public bool IsNumeric
        {
            get { return _property != null && PropertyType.IsNumericType(); }
        }

        

        public IEnumerable<Type> EnumerableTypes
        {
            get
            {
                var interfaces = PropertyType.GetInterfaces();
                return from i in interfaces where i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>) select i.GetGenericArguments()[0];
            }
        }

        public MethodInfo MapToMethod
        {
            get { return PropertyType.GetMethods().First(m => m.Name.Contains("MapTo")); }
        }

        public bool IsEnumerableTypeGenericModel
        {
            get { return EnumerableTypes.All(e => e.IsGenericModel()); }
        }

        public bool IsEnumerableTypeNumeric
        {
            get { return EnumerableTypes.All(e => e.IsNumericType()); }
        }

        public bool IsEnumerableTypeEnum
        {
            get { return EnumerableTypes.All(e => e.IsEnum); }
        }

        public ResolveModel(PropertyInfo property)
        {
            _property = property;
        }
    }

    public static class TypeExtensions
    {
        public static bool IsSimpleType(this Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // nullable type, check if the nested type is simple.
                return IsSimpleType(type.GetGenericArguments()[0]);
            }
            return type.IsPrimitive
                   || type.IsEnum
                   || type == typeof(string)
                   || type == typeof(decimal);
        }

        public static bool IsGenericModel(this Type type)
        {
            return type != null && typeof(IGenericModel).IsAssignableFrom(type);
            
        }

        public static bool IsString(this Type type)
        {
            return type != null && typeof (string) == type;
            
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
