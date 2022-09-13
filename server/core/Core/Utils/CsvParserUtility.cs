using Core.ObjectMapping;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;

namespace Core.Utils
{
    public class CsvParserUtility
    {
        public static string[] GetHeadersRowFromDelimitedString(string csvInput, string delimiter = ",")
        {
            var keys = new string[] { };
            
            var stringReader = new StringReader(csvInput);

            var parser = new TextFieldParser(stringReader)
            {
                HasFieldsEnclosedInQuotes = true
            };

            parser.SetDelimiters(delimiter);
            
            keys = parser.ReadFields();
            return keys;
        }

        public static IList<DictionaryBaseObject> GetObjectsListFromDelimitedString(string csvInput, string delimiter = ",")
        {
            var keys = new string[] { };

            var dictionaryBaseObjects = new List<DictionaryBaseObject>();

            var stringReader = new StringReader(csvInput);

            var parser = new TextFieldParser(stringReader)
                                     {
                                         HasFieldsEnclosedInQuotes = true
                                     };

            parser.SetDelimiters(delimiter);

            var firstLine = false;

            while (!parser.EndOfData)
            {
                if (!firstLine)
                {
                    firstLine = true;
                    keys = parser.ReadFields();
                }
                else
                {
                    var obj = new DictionaryBaseObject(keys);
                    try
                    {
                        var linesplited = parser.ReadFields();
                        if (linesplited == null)
                            continue;

                        for (var i = 0; i < keys.Length; i++)
                        {
                            var k = keys[i];
                            obj[k] = linesplited[i];
                        }
                        dictionaryBaseObjects.Add(obj);
                    }
                    catch (Exception exception)
                    {
                        
                    }
                }
            }

            return dictionaryBaseObjects;
        }
    }
}