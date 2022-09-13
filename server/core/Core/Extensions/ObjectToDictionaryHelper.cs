using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Xml.Schema;
using Core.Logging;
using Newtonsoft.Json;

namespace Core.Extensions
{
	public class DictionaryIgnoreAttribute: Attribute { }
	public static class ObjectToDictionaryHelper
	{
		private static ILogger _logger = Logger.GetLogger(typeof(ObjectToDictionaryHelper));

		public static IDictionary<string, object> ToDictionary(this object source, bool nestedProperties = false)
		{
			return source.ToDictionary<object>(nestedProperties);
		}

		public static IDictionary<string, T> ToDictionary<T>(this object source, bool nestedProperties = false)
		{
			if (source == null)
				throw new ArgumentNullException(nameof(source),
												"Unable to convert object to a dictionary. The source object is null.");

			var dictionary = new Dictionary<string, T>();

			foreach (var property in source.GetType().GetProperties())
			{
				AddPropertyToDictionary<T>(property, source, dictionary, nestedProperties);
			}
			return dictionary;
		}

		private static void AddPropertyToDictionary<T>(PropertyInfo property, object source, Dictionary<string, T> dictionary,
													   bool nestedProperties = false, string prefix = "")
		{
			if (property.GetCustomAttributes<JsonIgnoreAttribute>().Any() || property.GetCustomAttributes<DictionaryIgnoreAttribute>().Any())
			{
				return;
			}

			object value = null;
			try
			{
				value = property.GetValue(source);
			}
			catch (Exception exception)
			{
				_logger.Error(exception);
				return;
			}

			var propertyName = string.IsNullOrEmpty(prefix) ? property.Name : $"{prefix}.{property.Name}";

			if (value == null ||
			    property.PropertyType.IsPrimitive ||
			    value is int ||
			    value is bool ||
			    value is string ||
			    value is decimal ||
			    value is float ||
				value is DateTime ||
				value is DateTimeOffset ||
				!nestedProperties)
			{
				if (value == null || !IsOfType<T>(value))
				{
					dictionary.Add(propertyName, default(T));
				}
				else
				{
					dictionary.Add(propertyName, (T)value);
				}
				return;
			}

			var baseType = property.PropertyType.BaseType ?? property.PropertyType;
			if (baseType.FullName.Contains("Dictionary"))
			{
				var dictionaryValue = value as IDictionary<string, object>;
				if (dictionaryValue != null)
				{
					foreach (var dictionaryValueKey in dictionaryValue.Keys)
					{
						dictionary.Add(propertyName + "." + dictionaryValueKey, (T) dictionaryValue[dictionaryValueKey]);
					}
				}
				return;
			}

			foreach (var subProperty in value.GetType().GetProperties())
			{
				AddPropertyToDictionary<T>(subProperty, value, dictionary, nestedProperties, propertyName);
			}
		}

		private static bool IsOfType<T>(object value)
		{
			return value is T;
		}
	}
}