using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Extensions;
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

        public bool IsGenericModel { get { return _property != null && PropertyType.IsAssignableFrom<IGenericModel>(); } }

        public bool IsEnumerable
        {
            get { return _property != null && typeof(IEnumerable).IsAssignableFrom(PropertyType); }
        }

        public bool IsEnum { get { return _property != null && PropertyType.IsEnum; } }

        public bool IsString { get { return _property != null && typeof (string) == PropertyType; } }

        public bool IsNullable {
            get { return _property != null && PropertyType.IsNullable(); }
        }

        public Type UnderlyingType { get { return Nullable.GetUnderlyingType(PropertyType); } }

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
            get { return EnumerableTypes.All(e => e.IsAssignableFrom<IGenericModel>()); }
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
}
