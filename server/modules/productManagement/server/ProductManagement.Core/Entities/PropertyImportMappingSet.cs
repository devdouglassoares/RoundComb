using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProductManagement.Core.Settings;
using Newtonsoft.Json;

namespace ProductManagement.Core.Entities
{
    public class PropertyImportMappingSet
    {
        [Key]
        public long Id { get; set; }

        public string Name { get; set; }

        public DateTimeOffset? CreatedDate { get; set; }

        [Column("Configuration")]
        public string PropertyImportMappingConfigSerializedString { get; set; }

        [NotMapped]
        public List<PropertyImportMappingConfig> Configuration
        {
            get
            {
                if (string.IsNullOrEmpty(PropertyImportMappingConfigSerializedString))
                    return new List<PropertyImportMappingConfig>();

                return
                    JsonConvert.DeserializeObject<List<PropertyImportMappingConfig>>(
                        PropertyImportMappingConfigSerializedString);
            }
            set { PropertyImportMappingConfigSerializedString = JsonConvert.SerializeObject(value); }
        } 
    }
}