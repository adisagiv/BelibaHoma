using System;

namespace Services.Excel.Attributes
{
    public class CellTypeAttribute : Attribute
    {
        private readonly Type _type;

        /// <summary>
        /// Get the type of data in the cell
        /// </summary>
        /// <param name="type"></param>
        public CellTypeAttribute(Type type)
        {
            _type = type;
        }

        public Type CellType {
            get { return _type; }
            }
    }
}