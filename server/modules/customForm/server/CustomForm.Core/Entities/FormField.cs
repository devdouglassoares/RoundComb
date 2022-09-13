using Core.Database.Entities;
using Core.DynamicProperties.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CustomForm.Core.Entities
{
    public class FormField : BaseEntity
    {
        public string FieldName { get; set; }

        public string DisplayName { get; set; }

        public string LocalizationKey { get; set; }

        [NotMapped]
        public PropertyType PropertyType { get; set; }

        [Column("FieldType")]
        public virtual string PropertyTypeString
        {
            get
            {
                return PropertyType.ToString();
            }
            set
            {
                PropertyType newValue;
                if (Enum.TryParse(value, out newValue))
                {
                    PropertyType = newValue;
                }
            }
        }

        [NotMapped]
        public string[] AvailableOptions
        {
            get
            {
                if (string.IsNullOrEmpty(PredefinedValuesString))
                    return new string[] { };

                return JsonConvert.DeserializeObject<IList<string>>(PredefinedValuesString).ToArray();
            }

            set
            {
                if (value == null || !value.Any())
                {
                    PredefinedValuesString = JsonConvert.SerializeObject(new List<string>());
                    return;
                }

                PredefinedValuesString = JsonConvert.SerializeObject(value);
            }
        }

        public string UploadEndpointUrl { get; set; }

        public bool MultipleUpload { get; set; }

        [Column("PredefinedValues")]
        public string PredefinedValuesString { get; set; }
    }
}