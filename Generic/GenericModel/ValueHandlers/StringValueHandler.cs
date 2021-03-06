﻿using System;
using Generic.GenericModel.Interfaces;
using Generic.GenericModel.Models;

namespace Generic.GenericModel.ValueHandlers
{
    internal class StringValueHandler : IValueHandler
    {

        public object ConvertValue(TypeEquality typeEquality, object value)
        {
            return value.ToString();
        }
    }
}