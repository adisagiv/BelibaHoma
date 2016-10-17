using System;
using Generic.GenericModel.Interfaces;
using Generic.GenericModel.Models;

namespace Generic.GenericModel.ValueHandlers
{
    internal class SameTypeValueHandler : IValueHandler
    {
            HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();

        public object ConvertValue(TypeEquality typeEquality, object value)
        {
            return value;
        }
         
    }
}