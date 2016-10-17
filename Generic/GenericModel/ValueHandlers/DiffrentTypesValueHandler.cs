using System;
using Generic.GenericModel.Interfaces;
using Generic.GenericModel.Models;

namespace Generic.GenericModel.ValueHandlers
{
    internal class DiffrentTypesValueHandler : IValueHandler
    {

        public object ConvertValue(TypeEquality typeEquality, object value)
        {
            object result = null;
            var targetResolve = typeEquality.TargetResolve;
            var sourceResolve = typeEquality.SourceResolve;
            if (targetResolve.IsGenericModel)
            {
                result = Activator.CreateInstance(targetResolve.PropertyType, value);
            }
            else if (sourceResolve.IsGenericModel)
            {
                var method = sourceResolve.MapToMethod;

                var methodType = method.MakeGenericMethod(targetResolve.PropertyType);

                result = methodType.Invoke(value, null);
            }
            else if (targetResolve.IsEnum || sourceResolve.IsEnum)
            {
                return value;
            }
            else if (targetResolve.IsNullable || sourceResolve.IsNullable)
            {
                if (targetResolve.UnderlyingType.IsEnum)
                {
                    result = Enum.ToObject(targetResolve.UnderlyingType, value);
                }
                else if (sourceResolve.UnderlyingType.IsEnum)
                {
                    result = Convert.ChangeType(value, targetResolve.UnderlyingType);
                }
            }
            else
            {
                var message =
                    string.Format(
                        "At least source or target need to be of type IGenericModel, source propertey is of type {0}, target propertey is of type {1}",
                        sourceResolve.PropertyType, targetResolve.PropertyType);
                throw new Exception(message);
            }
            return result;
        }
    }
}