using Core.Extensions;
using Core.Logging;
using Core.ObjectMapping;
using ProductManagement.Core.Settings;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ProductManagement.Services
{
    public class PropertyImportCsvHelper
    {
        public static void MapPropertyInfo<T>(DictionaryBaseObject csvParsedObject, T model,
                                             List<PropertyImportMappingConfig> mappingConfigs,
                                             Action<T, object, PropertyImportMappingConfig> actionHandlerForExternalKey = null,
                                             Action<T, object, PropertyImportMappingConfig> actionHandlerForImage = null,
                                             ILogger logger = null)
        {
            foreach (var mappingConfig in mappingConfigs)
            {
                if (!csvParsedObject.Keys.Contains(mappingConfig.FromField))
                    continue;

                var value = csvParsedObject[mappingConfig.FromField];

                if (mappingConfig.ImportAsExternalKey)
                {
                    actionHandlerForExternalKey?.Invoke(model, value, mappingConfig);
                }

                if (mappingConfig.ImportAsImage)
                {
                    actionHandlerForImage?.Invoke(model, value, mappingConfig);
                }
                
                else if (mappingConfig.ImportAsDateTime)
                {
                    try
                    {
                        var dateValue = DateTimeOffset.ParseExact(value.ToString(),
                                                                  mappingConfig.DateTimeFormat,
                                                                  CultureInfo.CurrentCulture);

                        ObjectCopyExtensions.TrySetValue(model, mappingConfig.ToField, dateValue);
                    }
                    catch (Exception exception)
                    {
                        logger?.Error(exception);
                    }
                }
                else
                    ObjectCopyExtensions.TrySetValue(model, mappingConfig.ToField, value);
            }
        }
    }
}