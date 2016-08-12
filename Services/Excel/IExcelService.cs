using Services.Excel.Models;

namespace Services.Excel
{
    public interface IExcelService
    {
        ExcelImportResult<T> ImportTo<T>(string path);
    }
}
