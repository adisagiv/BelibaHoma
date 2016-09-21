using System;

namespace Generic.GenericModel.Interfaces
{
    /// <summary>
    /// Generic model type
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public interface IGenericModel<TModel> : IGenericModel
    {
        /// <summary>
        /// Map the model from the source object
        /// </summary>
        //TModel GetModel<TSource>(TSource entity);

        /// <summary>
        /// Get the object from the model
        /// </summary>
        /// <returns></returns>
        TResult MapTo<TResult>(Func<TResult, object> getInnerModel = null);
    }

    public interface IGenericModel
    {
        TResult MapTo<TResult>();
    }
}
