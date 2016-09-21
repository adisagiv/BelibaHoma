using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generic.Interfaces
{
    /// <summary>
    /// Generic model type
    /// </summary>
    /// <typeparam name="E">E - entity</typeparam>
    /// <typeparam name="M">M - Model type of GenericModel</typeparam>
    public interface IGenericModel<TModel> : IGenericModel
    {
        /// <summary>
        /// Reflect from this model to Result model
        /// </summary>
        /// <typeparam name="TResult">type of Result model</typeparam>
        /// <param name="getInnerModel">Models to map the inharite from the main model</param>
        /// <returns></returns>
        TResult MapTo<TResult>(Func<TResult, object> getInnerModel = null);
    }

    public interface IGenericModel
    {
    }
}
