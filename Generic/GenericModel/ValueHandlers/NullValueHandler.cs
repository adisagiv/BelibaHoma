using Generic.GenericModel.Interfaces;
using Generic.GenericModel.Models;

namespace Generic.GenericModel.ValueHandlers
{
    internal class NullValueHandler : IValueHandler
    {
        public object ConvertValue(TypeEquality typeEquality, object source)
        {
            throw new System.NotImplementedException();
        }
    }
}