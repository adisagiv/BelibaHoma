using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Extensions;
using Generic.GenericModel.Interfaces;
using Generic.GenericModel.ValueHandlers;

namespace Generic.GenericModel.Models
{
    public class GenericModel : IGenericModel
    {

        public GenericModel()
        {
            
        }
        public GenericModel(object source)
        {
            if (source == null) return;

            InvokeMapObject(source, this);
        }

        private static void InvokeMapObject(object source, object target)
        {
            var targetType = target.GetType();

            var method = typeof (GenericModel).GetMethod("MapObject", BindingFlags.NonPublic | BindingFlags.Static);
            var generic = method.MakeGenericMethod(targetType);
            generic.Invoke(target, new[] {source, target});
        }

        private static List<ResolveModel> ResolveSources(PropertyInfo targetPropertyInfo,
            List<PropertyInfo> sourcePropertiesInfo)
        {
            var sources =
                sourcePropertiesInfo.Where(s => s.Name == targetPropertyInfo.Name)
                    .Select(s => new ResolveModel(s))
                    .ToList();

            return sources;
        }

        private static void MapObject<TResult>(object source, TResult target)
        {
            var sourceType = source.GetType();
            var targetType = typeof (TResult);
            var targetProperties = targetType.GetProperties().ToList();
            var sourceProperties = sourceType.GetProperties().ToList();

            var resolver = ResolveProperties(targetProperties, sourceProperties);

            foreach (var typeEquality in resolver)
            {
                var value = typeEquality.GetValue(source);
                if (value != null)
                {
                    value = typeEquality.ValueHandler.ConvertValue(typeEquality, value);
                }
                
                typeEquality.TargetResolve.PropertyInfo.SetValue(target, value, null);
            }


        }

        private static List<TypeEquality> ResolveProperties(List<PropertyInfo> targetProperties,
            List<PropertyInfo> sourceProperties)
        {
            List<TypeEquality> resolvers = new List<TypeEquality>();
            foreach (var targetProperty in targetProperties)
            {
                var targetResolve = new ResolveModel(targetProperty);
                var sources = ResolveSources(targetProperty, sourceProperties);

                if (sources == null || sources.Count <= 0) continue;
                var currentresolvers = sources.Select(s => new TypeEquality(targetResolve, s)).ToList();

                foreach (var typeEquality in currentresolvers)
                {
                    if (typeEquality.IsTargetString)
                    {
                        typeEquality.UpdateValueHadler(new StringValueHandler());
                    }
                    else if (typeEquality.AreEnumerables)
                    {
                        typeEquality.UpdateValueHadler(new EnumerablesValueHandler());
                    }
                    else if (!typeEquality.AreEquel)
                    {
                        typeEquality.UpdateValueHadler(new DiffrentTypesValueHandler());
                    }
                    else if (typeEquality.AreEquel)
                    {
                        typeEquality.UpdateValueHadler(new SameTypeValueHandler());
                    }
                }

                resolvers.AddRange(currentresolvers);
            }

            return resolvers;
        }

        public TTarget MapTo<TTarget>()
        {
            var resultType = typeof (TTarget);

            // Create an instance of the result object
            var result = (TTarget) Activator.CreateInstance(resultType);

            MapObject(this, result);

            return result;
        }

        /// <summary>
        /// Map an Enumeration of <typeparamref name="TSource"/> to a List of <typeparamref name="TTarget"/>
        /// </summary>
        /// <param name="sources"><list type="TResult"></list></param>
        /// <returns></returns>
        public static List<TTarget> MapListOfModels<TSource, TTarget>(IEnumerable<TSource> sources)
            where TTarget : IGenericModel
        {
            var modelType = typeof (TTarget);

            var result = sources.Select(m => (TTarget) Activator.CreateInstance(modelType, m)).ToList();
            return result;
        }

        /// <summary>
        /// Map an Enumeration of <typeparamre name="IGenericModel"/> models to a List of <typeparamref name="TTarget"/>
        /// </summary>
        /// <param name="models">List of type <typeparamre name="IGenericModel"/> </param>
        /// <returns>List <typeparamref name="TTarget"/></returns>
        public static List<TTarget> MapListOfModels<TTarget>(IEnumerable<IGenericModel> models)
        {
            var result = models.Select(m => m.MapTo<TTarget>()).ToList();
            return result;
        }

        //public static Type CreateMapper<TTarget, TSource>(Func<TTarget,Type> selector) where TTarget : IGenericModel
        //{
            
        //}

        private static IEnumerable<TTarget> ConvertEnumerable<TTarget>(IEnumerable collection)
        {
            var targetType = typeof (TTarget);
            IEnumerable<TTarget> result;

            if (targetType.IsNullable())
            {
                var underlyingType = Nullable.GetUnderlyingType(targetType);

                if (underlyingType.IsEnum)
                {
                    result = from object obj in collection select (TTarget)Enum.ToObject(underlyingType, obj);
                }
                else
                {
                    result = from object obj in collection select (TTarget)Convert.ChangeType(obj, underlyingType);
                }
                 
            }
            else
            {
                result = from object obj in collection select (TTarget)obj;  
            }


            return result.ToList();
        }
    }

}
