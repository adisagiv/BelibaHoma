using System;
using System.Collections.Generic;
using System.Linq;
using OfficeOpenXml;
using Services.Excel.Models;

namespace Services.Excel
{
    public class ExcelService : IExcelService
    {
        public ExcelImportResult<T> ImportTo<T>(string path)
        {
            var results = new ExcelImportResult<T>();            

            using (var pck = new OfficeOpenXml.ExcelPackage())
            {
                using (var stream = System.IO.File.OpenRead(path))
                {
                    pck.Load(stream);
                }
                var workSheet = pck.Workbook.Worksheets.First();

                var columnsCount = workSheet.Dimension.End.Column;
                var rowCount = workSheet.Dimension.End.Row;

                //var headerRow = workSheet.Cells[1, 1, 1, columnsCount];
                //var headList = headerRow.Select(cell => cell).ToList();

                var rowModel = new GeneralRowModel<T>(workSheet);

                results = new ExcelImportResult<T>(rowModel);
                
                for (int rowNum = 1; rowNum <= rowCount; rowNum++)
                {
                    var errors = new List<Error>();
                    if (rowModel.HeaderRow != rowNum)
                    {
                        var row = workSheet.Cells[rowNum, 1, rowNum, columnsCount];

                        var values = Activator.CreateInstance<T>();

                        foreach (var generalCell in rowModel)
                        {
                            var cell = row[rowNum, generalCell.ForColumn];
                            try
                            {
                                SetValue(generalCell, values, cell);
                            }
                            catch (Exception ex)
                            {
                                var error = new Error(rowNum,generalCell.ForColumn,generalCell.Name,ex);
                                errors.Add(error);
                            }

                        }

                        var newRow = new Row<T>(rowNum, values, errors);

                        results.Rows.Add(newRow);
                    }
                }
            }

            return results;
        }

        private void SetValue(GeneralCellModel cellModel,object values, ExcelRange cell)
        {
            var property = values.GetType().GetProperty(cellModel.Name);

            var cellCastValue = Convert.ChangeType(cell.Value, cellModel.CellType);

            var castValue = Convert.ChangeType(cellCastValue, cellModel.PropType);

            property.SetValue(values, castValue, null);
        }
    }


    //public class ExcelService : IExcelService
    //{
    //    public List<T> ImportTo<T>(string path)
    //    {

    //        var rowModel = new GeneralRowModel<T>();

    //        var row = default(T);
    //        var t = GetAttribute<T>("Column");


    //        bool hasHeader = true;
    //        using (var pck = new OfficeOpenXml.ExcelPackage())
    //        {
    //            using (var stream = System.IO.File.OpenRead(path))
    //            {
    //                pck.Load(stream);
    //            }
    //            var workSheet = pck.Workbook.Worksheets.First();

    //            var columns = new List<string>();
    //            var rows = new List<T>();

    //            //foreach (var firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
    //            //{
    //            //    columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
    //            //}

    //            //var startRow = hasHeader ? 2 : 1;
    //            var startRow = 1;
    //            for (int rowNum = startRow; rowNum <= workSheet.Dimension.End.Row; rowNum++)
    //            {
    //                var wsRow = workSheet.Cells[rowNum, 1, rowNum, workSheet.Dimension.End.Column];
    //                //DataRow row = tbl.Rows.Add();
    //                foreach (var cell in wsRow)
    //                {
    //                    var key = columns[cell.Start.Column - 1];
    //                    //row.Add(key, cell.Text);
    //                }

    //                rows.Add(row);

    //            }
    //            return rows;
    //        }
    //    }

    //    Attribute GetAttribute<T>(string attributeName)
    //    {
    //        MemberInfo info = typeof(T);
    //        var attribute = info.GetCustomAttribute(typeof(ColumnAttribute));

    //        return attribute;
    //    }


    //}
}