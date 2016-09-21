using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Generic.Interfaces;

namespace Generic.Models
{
   
    /// <summary>
    /// Generic model type
    /// </summary>
    /// <typeparam name="TModel">The type of the model</typeparam>
    public class GenericModel<TModel> : IGenericModel<TModel> where TModel : IGenericModel<TModel>
    {
        //private readonly Type _entityType;
        private readonly Type _modelType;
        /// <summary>
        /// Default constructor
        /// </summary>
        public GenericModel()
        {
            /// get E and M types
           // _entityType = typeof(TEntity);
            _modelType = typeof(TModel);
        }

        /// <summary>
        /// Constructor that build the model from the given model
        /// </summary>
        /// <param name="model">Source model to map from</param>
        /// <param name="getInnerModel">Models to map the inharite from the main model</param>
        public GenericModel(object model, Func<object, object> getInnerModel = null)
            : this()
        {
            if (model != null)
            {
                Type _modelType = model.GetType();

                var modelProps = _modelType.GetProperties().ToList();

                MapObject(this,modelProps, _modelType, model);

                if (getInnerModel != null)
                {
                    var innerModel = getInnerModel(model);

                    if (innerModel != null)
                    {
                        //foreach (var innerModel in innerModels)
                       // {
                            var innetModelType = innerModel.GetType();

                            var innerModelProps = innetModelType.GetProperties().ToList();

                            MapObject(this,innerModelProps, _modelType, innerModel);
                       // }
                    }
                }
            }
        }

        private void MapObject(object destinationModel, List<PropertyInfo> sourceModelProps, Type sourceModelType, object sourceModel)
        {
            foreach (PropertyInfo sourceModelProp in sourceModelProps)
            {
                PropertyInfo destinationModelProp = destinationModel.GetType().GetProperty(sourceModelProp.Name, BindingFlags.Public | BindingFlags.Instance);
                  
                if (null != destinationModelProp && destinationModelProp.CanWrite)
                {
                    var modelValue = sourceModelProp.GetValue(sourceModel);
                    object result = modelValue;
                    if (destinationModelProp.PropertyType.BaseType != null
                        && destinationModelProp.PropertyType.GetInterfaces().Contains(typeof(IGenericModel))
                        && sourceModelType != null
                        && modelValue != null)
                    {
                        result = Activator.CreateInstance(destinationModelProp.PropertyType, new object[] { modelValue });
                    }
                    else if (modelValue != null 
                             && sourceModelProp.PropertyType.BaseType != null
                             && sourceModelProp.PropertyType.GetInterfaces().Contains(typeof(IGenericModel))
                             && sourceModelType != null)
                    {
                        var method = sourceModelProp.PropertyType.GetMethods().FirstOrDefault(m=> m.Name.Contains("MapTo"));

                        var methodType = method.MakeGenericMethod(destinationModelProp.PropertyType);

                        result = methodType.Invoke(null, new object[] { modelValue });
                    }
                    else if ((sourceModelProp.PropertyType.Name.Contains("Collection") || sourceModelProp.PropertyType.Name.Contains("List")) && modelValue != null)
                    {
                        try
                        {
                            var destinationListType = destinationModelProp.PropertyType.GetGenericArguments()[0];
                            var sourceListType = sourceModelProp.PropertyType.GetGenericArguments()[0];

                            MethodInfo method;
                            if (sourceListType.GetInterfaces().Contains(typeof(IGenericModel)))
                            {
                                method = sourceListType.GetMethodWithParent("GetListFromModels", BindingFlags.Public | BindingFlags.Static);
                                method = method.MakeGenericMethod(destinationListType);
                                
                            }
                            else if (destinationListType.GetInterfaces().Contains(typeof(IGenericModel)))
                            {
                                method = destinationListType.GetMethodWithParent("GetListOfModels",
                                    BindingFlags.Public | BindingFlags.Static);
                                method = method.MakeGenericMethod(sourceListType);

                            }
                            else
                            {
                                throw new Exception("source or destination need to be of type GenericModel");
                            }

                            //var methodType = method.MakeGenericMethod(destinationListType);
                            result = method.Invoke(null, new object[] { modelValue });
                        }
                        catch (Exception exception)
                        {
                            
                           
                        }
                        
                    }

                    

                    if (result != null 
                        && destinationModelProp.PropertyType.IsGenericType 
                        && destinationModelProp.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        var destinationModelPropUnderlyingType =
                            Nullable.GetUnderlyingType(destinationModelProp.PropertyType);

                        var sourceModelPropUnderlyingType = Nullable.GetUnderlyingType(sourceModelProp.PropertyType) ??
                                                            sourceModelProp.PropertyType;

                        if (destinationModelPropUnderlyingType.IsEnum)
                        {
                            result = Enum.ToObject(destinationModelPropUnderlyingType, result);
                        }
                    } 

                    destinationModelProp.SetValue(destinationModel, result, null);
                }
            }
        }

        
        
        /// <summary>
        /// Get a list of modeles from the model
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public static List<TModel> GetListOfModels<TSource>(ICollection<TSource> models)
        {
            var result = new List<TModel>();
            Type modelType = typeof(TModel);

            result = models.Select(m => (TModel)Activator.CreateInstance(modelType, new object[] { m })).ToList();
            return result;
        }

        /// <summary>
        /// Get a list of modeles from the model
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public static List<TResult> GetListFromModels<TResult>(List<TModel> models)
        {
            var result = new List<TResult>();

            result = models.Select(m => m.MapTo<TResult>()).ToList();
            return result;
        }

        /// <summary>
        /// Reflect from this model to Result model
        /// </summary>
        /// <typeparam name="TResult">type of Result model</typeparam>
        /// <param name="getIgnoreInnerModel">Models to ignore while mapping</param>
        /// <param name="getInnerModel">Models to map the inharite from the main model</param>
        /// <returns></returns>
        public virtual TResult MapTo<TResult>(Func<TResult, object> getInnerModel = null)
        {
            /// get E and M types
            Type resultType = typeof(TResult);

            /// create instance of E
            TResult result = (TResult)Activator.CreateInstance(typeof(TResult));
            TModel model = (TModel)Activator.CreateInstance(typeof(TModel));

            var modelProps = this.GetType().GetProperties().ToList();

           
            MapObject(result, modelProps, _modelType, this);

            if (getInnerModel != null)
            {
                var innerModel = getInnerModel(result);

                if (innerModel != null)
                {
                  //  foreach (var innerModel in innerModels)
                   // {
                        var innetModelType = innerModel.GetType();

                        var innerModelProps = innetModelType.GetProperties().ToList();

                        MapObject(innerModel, innerModelProps, _modelType, this);
                //    }
                }
            }

            return result;
        }
    }

    public static class TypeExtenstions
    {
        public static MethodInfo GetMethodWithParent(this Type type, string methodName,BindingFlags bindingFlags)
        {
            var currentType = type;
            MethodInfo method;
            do
            {
                method = currentType.GetMethod(methodName, bindingFlags);
                currentType = currentType.BaseType;

            } while (currentType != null && method == null);

            return method;
        }
    }
}
