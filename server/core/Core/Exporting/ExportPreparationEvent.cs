namespace Core.Exporting
{
    public class ExportPreparationEvent<T>
    {
        public CsvExportModel<T> ExportModel { get; set; }
    }

    public class ImportPreparationEvent<T>
    {
        public CsvImportModel<T> ImportModel { get; set; }
    }
}