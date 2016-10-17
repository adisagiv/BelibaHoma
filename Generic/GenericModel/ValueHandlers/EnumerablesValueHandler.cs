using System;
using System.Linq;
using System.Reflection;
using Generic.GenericModel.Interfaces;
using Generic.GenericModel.Models;

namespace Generic.GenericModel.ValueHandlers
{
    internal class EnumerablesValueHandler : IValueHandler
    {
        public object ConvertValue(TypeEquality typeEquality, object value)
        {
            var result = value;
            MethodInfo method = null;
            var sourceResolve = typeEquality.SourceResolve;
            var targetResolve = typeEquality.TargetResolve;
            if (typeEquality.AreEnumerablesTypesEquel)
            {
                if (sourceResolve.IsEnumerableTypeGenericModel)
                {
                    method = sourceResolve.PropertyType.GetMethod("MapListOfModels",
                        new[] { targetResolve.PropertyType });
                    method = method.MakeGenericMethod(targetResolve.EnumerableTypes.First());

                }
                else if (targetResolve.IsEnumerableTypeGenericModel)
                {
                    method =
                        typeof(Models.GenericModel).GetMethods()
                            .Where(m => m.Name == "MapListOfModels")
                            .First(m => m.GetGenericArguments().Count() == 2);

                    method = method.MakeGenericMethod(sourceResolve.EnumerableTypes.First(),
                        targetResolve.EnumerableTypes.First());

                }
                else if (typeEquality.AreEnumerablesTypesSimple)
                {
                    return result;

                }
                else
                {
                    var message =
                        string.Format(
                            "At least source or target need to be of type IGenericModel, source propertey is of type {0}, target propertey is of type {1}",
                            sourceResolve.PropertyType, targetResolve.PropertyType);
                    throw new Exception(message);
                }

                result = method.Invoke(null, new[] { value });
            }
            else
            {
                if (sourceResolve.IsEnumerableTypeNumeric ||
                    targetResolve.IsEnumerableTypeNumeric)
                {
                    var targetType = targetResolve.EnumerableTypes.First();

                    method = typeof(Models.GenericModel).GetMethod("ConvertEnumerable", BindingFlags.NonPublic | BindingFlags.Static);
                    var generic = method.MakeGenericMethod(targetType);
                    result = generic.Invoke(null, new[] { value });


                    return result;
                }
            }


            return result;
        }



        
    }
}