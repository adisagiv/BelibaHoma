using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Helper;
using OfficeOpenXml;
using Services.Excel.Attributes;

namespace Services.Excel.Models
{
    /// <summary>
    /// Wrapper to the result
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExcelImportResult<T> : IEnumerable
    {
        private readonly Row<List<Header>> _header;

        public Row<List<Header>> Headers 
        {
            get { return _header; }
        }
        
        public List<Row<T>> Rows { get; set; }

        public Row<T> this[int rowNumber]
        {
            get { return Rows.FirstOrDefault(r => r.Number == rowNumber); }
        }

        public bool HasErrors {
            get { return Rows.Any(r => !r.IsSuccess); }
        }


        private GeneralRowModel<T> _generalRowModel; 

        public ExcelImportResult(GeneralRowModel<T> rowModel)
        {
            _generalRowModel = rowModel;
            Rows = new List<Row<T>>();
            
            _header = new Row<List<Header>>(_generalRowModel.HeaderRow,_generalRowModel.Headers);
        }

        public ExcelImportResult()
        {
            
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public ListEnum<Row<T>> GetEnumerator()
        {
            return new ListEnum<Row<T>>(Rows);
        }
    }

    public class Header
    {
        public string Name { get; set; }
        public int Column { get; set; }

        public Header(ExcelRangeBase header)
        {
            Name = header.Text;
            Column = ColumnAttribute.ExcelColumnNameToNumber(ExtractColumnLetters(header));
        }

        private string ExtractColumnLetters(ExcelRangeBase header)
        {
            var onlyLetters = header.Address.Where(a => a >= 'A' && a <= 'Z');

            return String.Concat(onlyLetters);
        }
    }

    /// <summary>
    /// Row Model
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Row<T>
    {
        private readonly int _number;
        private readonly List<Error> _errors;

        public int Number {
            get { return _number; }
        }

        public T Values { get; set; }

        public bool IsSuccess {
            get { return Errors == null || !Errors.Any(); }
        }

        public List<Error> Errors
        {
            get { return _errors; }
        }

        /// <summary>
        /// Row model 
        /// </summary>
        /// <param name="number">Number of the row</param>
        /// <param name="values">The values</param>
        /// <param name="exceptions"></param>
        public Row(int number, T values,List<Error> errors)
            :this(number,values)
        {
            _errors = errors;
        }

        public Row(int number, T values)
        {
            _number = number;
            Values = values;
        }
        
    }

    public class Error
    {
        private readonly int _row;
        private readonly int _column;
        private readonly string _propName;
        private readonly Exception _exception;

        public int Row
        {
            get { return _row; }
        }

        public int Column
        {
            get { return _column; }
        }

        public string PropName {
            get { return _propName; }
        }
        public Exception Exception
        {
            get { return _exception; }
        }

        public Error(int row, int column, string propName, Exception exception)
        {
            _row = row;
            _column = column;
            _propName = propName;
            _exception = exception;
        }
    }
}
