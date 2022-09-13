using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Core.Exporting
{
    public class CsvImportModel<T>
    {
        private readonly IDictionary<string, Action<string[], int, string, T>> _columnMapping;

        private Func<T> _modelInstantiation;

        private string inputCsvString;

        public string Delimiter { get; set; }

        public CsvImportModel(string delimiter = ",")
        {
            Delimiter = delimiter;
            _columnMapping = new Dictionary<string, Action<string[], int, string, T>>();
        }

        public void SetInstantiator(Func<T> instantiator)
        {
            _modelInstantiation = instantiator;
        }

        public void AddColumn(string columnName, Action<string[], int, string, T> resolveFunc)
        {
            _columnMapping.Add(columnName, resolveFunc);
        }

        public void SetCsvInput(string csvInput)
        {
            inputCsvString = csvInput;
        }

        public int GetImportQuantities()
        {
            var result = 0;

            using (var stringReader = new StringReader(inputCsvString))
            {
                var lineIndex = 0;

                while (!string.IsNullOrEmpty(stringReader.ReadLine()))
                {
                    if (lineIndex == 0) // header
                    {
                        lineIndex++;
                        continue;
                    }
                    lineIndex++;

                    result++;
                }
            }

            return result;
        }

        public List<T> Render(Action<T> proceed = null)
        {
            if (_modelInstantiation == null)
                throw new NotImplementedException("The instantiation method has not been implemented");

            var fileColumns = new List<string>();

            var listResult = new List<T>();

            using (var stringReader = new StringReader(inputCsvString))
            {
                var lineIndex = 0;

                string line;

                while (!string.IsNullOrEmpty(line = stringReader.ReadLine()))
                {
                    var columns = line.Split(new[] { Delimiter }, StringSplitOptions.None);
                    if (lineIndex == 0) // header
                    {
                        fileColumns = columns.ToList();
                        lineIndex++;
                        continue;
                    }

                    var model = _modelInstantiation();

                    for (var i = 0; i < fileColumns.Count; i++)
                    {
                        var columnName = fileColumns[i];
                        Action<string[], int, string, T> resolveFunc;

                        if (_columnMapping.TryGetValue(columnName, out resolveFunc))
                        {
                            resolveFunc(columns, i, columns[i], model);
                        }

                    }
                    proceed?.Invoke(model);

                    listResult.Add(model);
                    lineIndex++;
                }
            }

            return listResult;
        }
    }
}