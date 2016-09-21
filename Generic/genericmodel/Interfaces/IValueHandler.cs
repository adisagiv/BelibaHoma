using Generic.GenericModel.Models;
using Generic.Models;

namespace Generic.GenericModel.Interfaces
{
    internal interface IValueHandler
    {
        object ConvertValue(TypeEquality typeEquality, object source);
    }
}