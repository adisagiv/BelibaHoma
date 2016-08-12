using System;
using System.IO;

namespace Services.Excel.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute : Attribute
    {
        private readonly int _columnNumber;

        /// <summary>
        /// The column letter for mapping this property
        /// </summary>
        /// <param name="column"></param>
        public ColumnAttribute(string column)
        {
            _columnNumber = ExcelColumnNameToNumber(column);
        }

        /// <summary>
        /// The column number for mapping this property
        /// </summary>
        /// <param name="column"></param>
        public ColumnAttribute(int column)
        {
            _columnNumber = column;
        }

        public int Column
        {
            get { return _columnNumber; }
        }

        public static int ExcelColumnNameToNumber(string columnName)
        {
            if (string.IsNullOrEmpty(columnName)) throw new ArgumentNullException("columnName");

            columnName = columnName.ToUpperInvariant();

            int sum = 0;

            foreach (char chr in columnName)
            {
                if (chr >= 'A' || chr <= 'Z')
                {

                    sum *= 26;
                    sum += (chr - 'A' + 1);
                }
                else
                {
                    throw new InvalidDataException("columnName should be only [A-Z]+");
                }
            }

            return sum;
        }
    }

    public class RowAttribute : Attribute
    {
        private readonly bool _hasHeader;
        private readonly int _rowNumber;

        public bool HasHeader {
            get { return _hasHeader; }
        }

        public int HeaderInRow
        {
            get { return _rowNumber; }
        }

        /// <summary>
        /// Has header and which row
        /// </summary>
        /// <param name="hasHeader"></param>
        /// <param name="rowNumber"></param>
        public RowAttribute(bool hasHeader = true,int rowNumber = 1)
        {
            _hasHeader = hasHeader;
            _rowNumber = rowNumber;
        }
    }
}