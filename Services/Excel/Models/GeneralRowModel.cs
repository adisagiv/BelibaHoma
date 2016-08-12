using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Helper;
using OfficeOpenXml;
using Services.Excel.Attributes;

namespace Services.Excel.Models
{
    public class GeneralRowModel<T> : IEnumerable
    {
        private readonly List<Attribute> _attributes;
        private readonly List<Header> _headers; 

        public List<GeneralCellModel> Cells { get; set; }
        public Type Type { get; set; }

        public int HeaderRow
        {
            get { return GetHeaderRowNumber(); }
        }

        public List<Header> Headers
        {
            get { return _headers; }
        }

        public GeneralRowModel(ExcelWorksheet worksheet)
        {
            Type = typeof(T);
            _attributes = Type.GetCustomAttributes(true).Cast<Attribute>().ToList();

            var columnsCount = worksheet.Dimension.End.Column;
            var rowsCount = worksheet.Dimension.End.Row;
            var headerRow = worksheet.Cells[HeaderRow + 1, 1, rowsCount, columnsCount];

            _headers = headerRow.Select(cell => new Header(cell)).ToList();
            

            var i = 1;
            Cells = Type.GetProperties().Select(p => new GeneralCellModel(p, _headers, i++)).ToList();
        }


        private int GetHeaderRowNumber()
        {
            var rowAttribute = (RowAttribute)_attributes.FirstOrDefault(a => a is RowAttribute);

            if (rowAttribute != null && rowAttribute.HasHeader)
            {
                return rowAttribute.HeaderInRow;
            }

            return 0;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public ListEnum<GeneralCellModel> GetEnumerator()
        {
            return new ListEnum<GeneralCellModel>(Cells);
        }
    }

    public class GeneralCellModel
    {
        private readonly string _name;
        private readonly int _headerNumber;
        private readonly Type _cellType;
        private readonly Type _propType;
        private readonly List<Attribute> _attributes;

        /// <summary>
        /// Property Name
        /// </summary>
        public string Name { get { return _name; } }

        /// <summary>
        /// The cell is in column
        /// </summary>
        public int ForColumn
        {
            get { return GetForColumn(); }
        }

        public string ForHeader
        {
            get { return GetForHeader(); }
        }

        /// <summary>
        /// The cell type
        /// </summary>
        public Type CellType
        {
            get { return GetCellType(); }
        }

        /// <summary>
        /// The property Type
        /// </summary>
        public Type PropType
        {
            get { return _propType; }
        }



        public GeneralCellModel(PropertyInfo propertyInfo, List<Header> headers, int index)
        {
            _name = propertyInfo.Name;
            _propType = propertyInfo.PropertyType;
            _attributes = propertyInfo.GetCustomAttributes(true).Cast<Attribute>().ToList();

            var header = headers.FirstOrDefault(h => h.Name.ToLower() == ForHeader.ToLower());

            if (header != null)
            {
                index = headers.IndexOf(header) + 1;
            }

            _headerNumber = index;
        }

        private int GetForColumn()
        {
            var columnAttribute = (ColumnAttribute)_attributes.FirstOrDefault(a => a is ColumnAttribute);

            if (columnAttribute != null)
            {
                return columnAttribute.Column;
            }

            return _headerNumber;

        }

        private string GetForHeader()
        {
            var headerAttribute = (HeaderAttribute)_attributes.FirstOrDefault(a => a is HeaderAttribute);

            if (headerAttribute == null)
            {
                return _name;
            }

            return headerAttribute.ForHeader;
        }

        private Type GetCellType()
        {
            var cellTypeAttribute = (CellTypeAttribute)_attributes.FirstOrDefault(a => a is CellTypeAttribute);

            if (cellTypeAttribute != null)
            {
                return cellTypeAttribute.CellType;
            }

            return typeof(string);
        }

    }
}