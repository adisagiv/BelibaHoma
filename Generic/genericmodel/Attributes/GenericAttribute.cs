using System;

namespace Generic.GenericModel.Attributes
{
    public class GenericAttribute : Attribute
    {

        public GenericAttribute(Func<Type, Type> predicate)
        {
            
        }
    }
}