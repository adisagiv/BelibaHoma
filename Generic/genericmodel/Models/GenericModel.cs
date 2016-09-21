using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Generic.GenericModel.Interfaces;
using Generic.GenericModel.Models;
using Generic.Interfaces;

namespace Generic.Models
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

            var method = typeof(GenericModel).GetMethod("MapObject", BindingFlags.NonPublic | BindingFlags.Static);
            var generic = method.MakeGenericMethod(targetType);
            generic.Invoke(target, new[] { source, target });
        }

        private static ResolveModel ResolveTarget(List<PropertyInfo> targetPropertyInfo, PropertyInfo sourcePropertyInfo)
        {
            var target = targetPropertyInfo.FirstOrDefault(d => d.Name == sourcePropertyInfo.Name);

            return new ResolveModel(target);
        }

        private static TypeEquality CheckTypes(ResolveModel source, ResolveModel target)
        {
            var typeEquality = new TypeEquality(source, target);
            return typeEquality;
        }

        private static void MapObject<TResult>(object source, TResult target)
        {
            var sourceType = source.GetType();
            var targetType = typeof(TResult);
            var targetProperties = targetType.GetProperties().ToList();
            var sourceProperties = sourceType.GetProperties().ToList();
            
            foreach (var sourceProperty in sourceProperties)
            {
                var targetResolve = ResolveTarget(targetProperties, sourceProperty);
                var sourceResolve = new ResolveModel(sourceProperty);

                if (null == targetResolve.PropertyInfo || !targetResolve.CanWrite) continue;
                var value = sourceResolve.PropertyInfo.GetValue(source);


                var typeEquality = CheckTypes(sourceResolve, targetResolve);



                if (typeEquality.IsTargetString)
                {
                    value = value.ToString();
                }
                else if (typeEquality.AreEnumerables)
                {
                    value = HandleEnumerables(sourceResolve, targetResolve, typeEquality, value);
                }
                else if (!typeEquality.AreEquel)
                {
                    value = HandleDiffrentTypes(sourceResolve, targetResolve, value);
                }
                
                targetResolve.PropertyInfo.SetValue(target, value, null);
            }
        }

        private static object HandleEnumerables(ResolveModel sourceResolve, ResolveModel targetResolve, TypeEquality typeEquality,object value)
        {
            var result = value;
            MethodInfo method = null;
            if (typeEquality.AreEnumerablesTypesEquel)
            {
                if (sourceResolve.IsEnumerableTypeGenericModel)
                {
                    method = sourceResolve.PropertyType.GetMethod("MapListOfModels",
                        new[] {targetResolve.PropertyType});
                    method = method.MakeGenericMethod(targetResolve.EnumerableTypes.First());

                }
                else if (targetResolve.IsEnumerableTypeGenericModel)
                {
                    method =
                        typeof (GenericModel).GetMethods()
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

                result = method.Invoke(null, new[] {value});
            }
            else 
            {
                if (targetResolve.IsEnumerableTypeEnum && sourceResolve.IsEnumerableTypeNumeric ||
                    targetResolve.IsEnumerableTypeNumeric && sourceResolve.IsEnumerableTypeEnum)
                {
                    var targetType = targetResolve.EnumerableTypes.First();

                    method = typeof(GenericModel).GetMethod("ConvertEnumerable", BindingFlags.NonPublic | BindingFlags.Static);
                    var generic = method.MakeGenericMethod(targetType);
                    result = generic.Invoke(null, new[] { value });


                    return result;
                }
            }
            

            return result;
        }

        private static object HandleGenericModel(ResolveModel targetResolve, object modelValue)
        {
            return Activator.CreateInstance(targetResolve.PropertyType, modelValue);
        }

        private static IEnumerable<TTarget> ConvertEnumerable<TTarget>(IEnumerable collection)
        {
                      
            var result = from object obj in collection select (TTarget)obj;

            return result.ToList();
        }

        private static object HandleDiffrentTypes(ResolveModel sourceResolve, ResolveModel targetResolve, object value)
        {
            object result;
            if (targetResolve.IsGenericModel)
            {
                result = HandleGenericModel(targetResolve, value);
            }
            else if (sourceResolve.IsGenericModel)
            {
                var method = sourceResolve.MapToMethod;

                var methodType = method.MakeGenericMethod(targetResolve.PropertyType);

                result = methodType.Invoke(null, new[] {value});
            }
            else if (targetResolve.IsEnum || sourceResolve.IsEnum)
            {
                return value;
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

        public TTarget MapTo<TTarget>()
        {
            var resultType = typeof(TTarget);

            // Create an instance of the result object
            var result = (TTarget)Activator.CreateInstance(resultType);

            MapObject(this, result);

            return result;
        }

        /// <summary>
        /// Map an Enumeration of <typeparamref name="TSource"/> to a List of <typeparamref name="TTarget"/>
        /// </summary>
        /// <param name="sources"><list type="TResult"></list></param>
        /// <returns></returns>
        public static List<TTarget> MapListOfModels<TSource, TTarget>(IEnumerable<TSource> sources) where TTarget : IGenericModel
        {
            var modelType = typeof(TTarget);

            var result = sources.Select(m => (TTarget)Activator.CreateInstance(modelType, m)).ToList();
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
        
    }

    //public static class EnumerableExtension
    //{
    //    /// <summary>
    //    /// Map an Enumerable collection to the requierd target
    //    /// </summary>
    //    /// <typeparam name="TResult">The Enumerable collection type</typeparam>
    //    /// <param name="source">The Source Enumerable collection</param>
    //    /// <returns></returns>
    //    public static IEnumerable<TResult> MapTo<TResult>(this IEnumerable<object> source)
    //    {
    //        var sourceType = source.GetType();

    //        var target = MapObject<TResult>(sourceType); 

    //        return target;
    //    }

    //    private static IEnumerable<TResult> MapObject<TResult>(Type sourceType)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    public static class ObjectExtension
    {
        public static TResult MapTo<TResult>(this object source)
        {
            var target = MapObject<TResult>(source); 

            return target;
        }

        private static ResolveModel ResolveTarget(List<PropertyInfo> targetPropertyInfo, PropertyInfo sourcePropertyInfo)
        {
            var target = targetPropertyInfo.FirstOrDefault(d => d.Name == sourcePropertyInfo.Name);

            return new ResolveModel(target);
        }

        private static TResult MapObject<TResult>(object source)
        {
            // Create an instance of the result object
            var target = Activator.CreateInstance<TResult>();

            var sourceType = source.GetType();
            var targetType = typeof(TResult);
            var targetProperties = targetType.GetProperties().ToList();
            var sourceProperties = sourceType.GetProperties().ToList();

            foreach (var sourceProperty in sourceProperties)
            {
                var targetResolve = ResolveTarget(targetProperties, sourceProperty);
                var sourceResolve = new ResolveModel(sourceProperty);
                 if (null != targetResolve.PropertyInfo && targetResolve.CanWrite)
                {
                    var value = sourceResolve.PropertyInfo.GetValue(source);

                    if (sourceResolve.PropertyType != targetResolve.PropertyType)
                    {
                        value = HandleDiffrentTypes(sourceResolve, targetResolve, value);
                    }
                    else
                    {
                    //    if (targetResolve.IsEnumerable)
                    //    {
                    //        value = HandleEnumerable(targetResolve,);
                    //    }
                        
                    //}
                    //if ((sourceModelProp.PropertyType.Name.Contains("Collection") || sourceModelProp.PropertyType.Name.Contains("List")) && modelValue != null)
                    //{
                    //    var targetListType = targetModelProp.PropertyType.GetGenericArguments()[0];
                    //    var sourceListType = sourceModelProp.PropertyType.GetGenericArguments()[0];

                    //    MethodInfo method;
                    //    if (sourceListType.GetInterfaces().Contains(typeof(IGenericModel)))
                    //    {
                    //        method = sourceListType.GetMethodWithParent("GetListFromModels", BindingFlags.Public | BindingFlags.Static);
                    //        method = method.MakeGenericMethod(targetListType);

                    //    }
                    //    else if (targetListType.GetInterfaces().Contains(typeof(IGenericModel)))
                    //    {
                    //        method = targetListType.GetMethodWithParent("GetListOfModels",
                    //            BindingFlags.Public | BindingFlags.Static);
                    //        method = method.MakeGenericMethod(sourceListType);

                    //    }
                    //    else
                    //    {

                    //        // targetModelProp.SetValue(targetModel, result, null);
                    //        // throw new Exception("source or target need to be of type GenericModel");
                    //    }

                    //    //var methodType = method.MakeGenericMethod(targetListType);
                    //    value = method.Invoke(null, new object[] { modelValue });

                    }

                    targetResolve.PropertyInfo.SetValue(target, value, null);
                }
            }

            return target;
        }

        private static object HandleDiffrentTypes(ResolveModel sourceResolve, ResolveModel targetResolve, object value)
        {
            throw new NotImplementedException();
        }

       
    }
   
    /// <summary>
    /// Generic model type
    /// </summary>
    /// <typeparam name="TModel">The type of the model</typeparam>
    //public class GenericModel<TModel> : IGenericModel<TModel> where TModel : IGenericModel<TModel>
    //{
    //    //private readonly Type _entityType;
    //    private readonly Type _modelType;
    //    /// <summary>
    //    /// Default constructor
    //    /// </summary>
    //    public GenericModel()
    //    {
    //        // get E and M types
    //       // _entityType = typeof(TEntity);
    //        _modelType = typeof(TModel);
    //    }

    //    /// <summary>
    //    /// Constructor that build the model from the given model
    //    /// </summary>
    //    /// <param name="model">Source model to map from</param>
    //    /// <param name="getInnerModel">Models to map the inharite from the main model</param>
    //    public GenericModel(object model, Func<object, object> getInnerModel = null)
    //        : this()
    //    {
    //        if (model != null)
    //        {
    //            Type modelType = model.GetType();

    //            var modelProps = modelType.GetProperties().ToList();

    //            MapObject(this,modelProps, modelType, model);

    //            if (getInnerModel != null)
    //            {
    //                var innerModel = getInnerModel(model);

    //                if (innerModel != null)
    //                {
    //                    var innetModelType = innerModel.GetType();

    //                    var innerModelProps = innetModelType.GetProperties().ToList();

    //                    MapObject(this,innerModelProps, modelType, innerModel);
    //                }
    //            }
    //        }
    //    }

    //    private ResolveModel ResolveTarget(List<PropertyInfo> targetPropertyInfo, PropertyInfo sourcePropertyInfo)
    //    {
    //        var target = targetPropertyInfo.FirstOrDefault(d => d.Name == sourcePropertyInfo.Name);

    //        return new ResolveModel(target);
    //    }

    //    private void MapObject(object targetModel, List<PropertyInfo> sourceModelProps, Type sourceModelType, object sourceModel)
    //    {
    //        foreach (PropertyInfo sourceModelProp in sourceModelProps)
    //        {
    //            PropertyInfo destinationModelProp = targetModel.GetType().GetProperty(sourceModelProp.Name, BindingFlags.Public | BindingFlags.Instance);

    //            if (null != destinationModelProp && destinationModelProp.CanWrite)
    //            {
    //                var modelValue = sourceModelProp.GetValue(sourceModel);
    //                object result = modelValue;
    //                if (destinationModelProp.PropertyType.BaseType != null
    //                    && destinationModelProp.PropertyType.GetInterfaces().Contains(typeof(IGenericModel))
    //                    && sourceModelType != null
    //                    && modelValue != null)
    //                {
    //                    result = Activator.CreateInstance(destinationModelProp.PropertyType, new object[] { modelValue });
    //                }
    //                else if (modelValue != null
    //                         && sourceModelProp.PropertyType.BaseType != null
    //                         && sourceModelProp.PropertyType.GetInterfaces().Contains(typeof(IGenericModel))
    //                         && sourceModelType != null)
    //                {
    //                    var method = sourceModelProp.PropertyType.GetMethods().FirstOrDefault(m => m.Name.Contains("MapTo"));

    //                    var methodType = method.MakeGenericMethod(destinationModelProp.PropertyType);

    //                    result = methodType.Invoke(null, new object[] { modelValue });
    //                }
    //                else if ((sourceModelProp.PropertyType.Name.Contains("Collection") || sourceModelProp.PropertyType.Name.Contains("List")) && modelValue != null)
    //                {
    //                    try
    //                    {
    //                        var destinationListType = destinationModelProp.PropertyType.GetGenericArguments()[0];
    //                        var sourceListType = sourceModelProp.PropertyType.GetGenericArguments()[0];

    //                        MethodInfo method;
    //                        if (sourceListType.GetInterfaces().Contains(typeof(IGenericModel)))
    //                        {
    //                            method = sourceListType.GetMethodWithParent("GetListFromModels", BindingFlags.Public | BindingFlags.Static);
    //                            method = method.MakeGenericMethod(destinationListType);

    //                        }
    //                        else if (destinationListType.GetInterfaces().Contains(typeof(IGenericModel)))
    //                        {
    //                            method = destinationListType.GetMethodWithParent("GetListOfModels",
    //                                BindingFlags.Public | BindingFlags.Static);
    //                            method = method.MakeGenericMethod(sourceListType);

    //                        }
    //                        else
    //                        {
    //                            throw new Exception("source or destination need to be of type GenericModel");
    //                        }

    //                        //var methodType = method.MakeGenericMethod(destinationListType);
    //                        result = method.Invoke(null, new object[] { modelValue });
    //                    }
    //                    catch (Exception exception)
    //                    {


    //                    }

    //                }

    //                destinationModelProp.SetValue(destinationModel, result, null);
    //            }
    //        }
    //    }

        
        
    //    /// <summary>
    //    /// Get a list of modeles from the model
    //    /// </summary>
    //    /// <param name="models"></param>
    //    /// <returns></returns>
    //    public static List<TModel> GetListOfModels<TSource>(ICollection<TSource> models)
    //    {
    //        var result = new List<TModel>();
    //        Type modelType = typeof(TModel);

    //        result = models.Select(m => (TModel)Activator.CreateInstance(modelType, new object[] { m })).ToList();
    //        return result;
    //    }

    //    /// <summary>
    //    /// Get a list of modeles from the model
    //    /// </summary>
    //    /// <param name="models"></param>
    //    /// <returns></returns>
    //    public static List<TResult> GetListFromModels<TResult>(List<TModel> models)
    //    {
    //        var result = new List<TResult>();

    //        result = models.Select(m => m.MapTo<TResult>()).ToList();
    //        return result;
    //    }

    //    /// <summary>
    //    /// Reflect from this model to Result model
    //    /// </summary>
    //    /// <typeparam name="TResult">type of Result model</typeparam>
    //    /// <param name="getInnerModel">Models to map the inharite from the main model</param>
    //    /// <returns></returns>
    //    public virtual TResult MapTo<TResult>(Func<TResult, object> getInnerModel = null)
    //    {
    //        // get E and M types
    //        Type resultType = typeof(TResult);

    //        // create instance of E
    //        TResult result = (TResult)Activator.CreateInstance(typeof(TResult));

    //        var modelProps = this.GetType().GetProperties().ToList();

    //        MapObject(result, modelProps, _modelType, this);

    //        if (getInnerModel != null)
    //        {
    //            var innerModel = getInnerModel(result);

    //            if (innerModel != null)
    //            {
    //              //  foreach (var innerModel in innerModels)
    //               // {
    //                    var innetModelType = innerModel.GetType();

    //                    var innerModelProps = innetModelType.GetProperties().ToList();

    //                    MapObject(innerModel, innerModelProps, _modelType, this);
    //            //    }
    //            }
    //        }

    //        return result;
    //    }

    //    public TResult MapTo<TResult>()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //public static class TypeExtenstions
    //{
    //    public static MethodInfo GetMethodWithParent(this Type type, string methodName,BindingFlags bindingFlags)
    //    {
    //        var currentType = type;
    //        MethodInfo method;
    //        do
    //        {
    //            method = currentType.GetMethod(methodName, bindingFlags);
    //            currentType = currentType.BaseType;

    //        } while (currentType != null && method == null);

    //        return method;
    //    }
    //}
}
