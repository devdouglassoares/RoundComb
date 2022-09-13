using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Exporting
{
    public class CsvExportModel<T>
    {
        private readonly IDictionary<string, Func<T, dynamic>> _columnMapping;

        public string Delimiter { get; set; }

        public CsvExportModel(string delimiter = ",")
        {
            Delimiter = delimiter;
            _columnMapping = new Dictionary<string, Func<T, dynamic>>();
        }

        public void AddColumn(string columnName, Func<T, dynamic> resolveFunc)
        {
            _columnMapping.Add(columnName, resolveFunc);
        }

        public string Render(IEnumerable<T> dataSource)
        {
            var stringBuilder = new StringBuilder();

            // append header
            stringBuilder.AppendLine(string.Join(Delimiter, _columnMapping.Keys));

            foreach (var item in dataSource)
            {
                var rowItem = _columnMapping.Select(x =>
                                                    {
                                                        var result = "";

                                                        if (x.Value(item) != null)
                                                        {
                                                            result = x.Value(item).ToString();
                                                        }

                                                        return result.Contains(Delimiter) ? "\"" + result + "\"" : result;
                                                    });

                stringBuilder.AppendLine(string.Join(Delimiter, rowItem));
            }


            return stringBuilder.ToString();
        }
    }
}