using System;
using System.Reflection;

namespace Generic.GenericModel.Attributes
{
    public class GenericAttribute : Attribute
    {
        public PropertyInfo PropertyInfo { get; set; }
        /// <summary>
        /// Map to property on the source object
        /// </summary>
        /// <param name="predict">return the property info of the property to map to</param>
        public GenericAttribute(Func<PropertyInfo> predict)
        {
            if (predict != null)
            {
                PropertyInfo = predict();
            }
            else
            {
                throw new ArgumentNullException("predict");
            }

        }
    }
}