using System;
using System.Linq;
using Extensions;
using Generic.GenericModel.Interfaces;

namespace Generic.GenericModel.Models
{
    internal class TypeEquality
    {
        public ResolveModel SourceResolve { get; private set; }
    
        public ResolveModel TargetResolve { get; private set; }

        public IValueHandler ValueHandler { get; private set; }

        /// <summary>
        /// Are both target and source types equals
        /// </summary>
        public bool AreEquel { get { return SourceResolve.PropertyType == TargetResolve.PropertyType; } }

        /// <summary>
        /// Is the target typeof string
        /// </summary>
        public bool IsTargetString { get { return TargetResolve.IsString; } }

        /// <summary>
        /// Are bot target and source enumerable
        /// </summary>
        public bool AreEnumerables { get { return SourceResolve.IsEnumerable && TargetResolve.IsEnumerable; } }

        public bool AreEnumerablesTypesEquel
        {
            get
            {
                if (!AreEnumerables) throw new Exception("Source and Target are not Enumerables");
                if (TargetResolve.EnumerableTypes.Count() != SourceResolve.EnumerableTypes.Count() && TargetResolve.EnumerableTypes.Count() == 1 && SourceResolve.EnumerableTypes.Count() == 1)
                    throw new Exception(string.Format("target enumerable types count {0} is not equal to source enumerable types count {1} equel to 1", TargetResolve.EnumerableTypes.Count(), SourceResolve.EnumerableTypes.Count()));


                for (var i = 0; i < SourceResolve.EnumerableTypes.Count(); i++)
                {
                    var targetType = TargetResolve.EnumerableTypes.ElementAt(i);
                    var sourceType = SourceResolve.EnumerableTypes.ElementAt(i);


                    if (targetType != sourceType &&
                        (!targetType.IsAssignableFrom<IGenericModel>() && !sourceType.IsAssignableFrom<IGenericModel>()) && !targetType.IsString())
                        return false;
                }

                return true;
            }
        }

        public bool AreEnumerablesTypesSimple
        {
            get { return TargetResolve.EnumerableTypes.All(e => e.IsSimpleType()) && SourceResolve.EnumerableTypes.All(e => e.IsSimpleType()); }
        }

        public TypeEquality(ResolveModel targetResolve, ResolveModel sourceResolve)
        {
            SourceResolve = sourceResolve;
            TargetResolve = targetResolve;
        }

        public void UpdateValueHadler(IValueHandler valueHandler)
        {
            ValueHandler = valueHandler;
        }

        public object GetValue(object source)
        {
            return SourceResolve.PropertyInfo.GetValue(source, null);
        }
    }
}