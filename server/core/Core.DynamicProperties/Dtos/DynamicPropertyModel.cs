using Core.DynamicProperties.Models;
using System;
using System.Collections.Generic;

namespace Core.DynamicProperties.Dtos
{
    public class DynamicPropertyModel
    {
        public long Id { get; set; }

        public string DisplayName { get; set; }

        public string PropertyName { get; set; }

        public bool Searchable { get; set; }

        public bool RangeSearchable { get; set; }

        public bool IsRequired { get; set; }

        public bool AvailableToAllEntities { get; set; }

        public PropertyType PropertyType { get; set; }

        public List<string> AvailableOptions { get; set; }

        public string[] TargetEntityTypes { get; set; }


        public string UploadEndpointUrl { get; set; }

        public bool MultipleUpload { get; set; }

        public bool IsSame(DynamicProperty entity)
        {
            return DisplayName == entity.DisplayName &&
                   PropertyName == entity.PropertyName &&
                   Searchable == entity.Searchable &&
                   IsRequired == entity.IsRequired &&
                   RangeSearchable == entity.RangeSearchable &&
                   AvailableToAllEntities == entity.AvailableToAllEntities &&
                   PropertyType == entity.PropertyType &&
                   AvailableOptions.Count == entity.AvailableOptions.Count &&
                   AvailableOptions.TrueForAll(option => entity.AvailableOptions.Contains(option));

        }

        public DateTimeOffset? ModifiedDate { get; set; }

        public DateTimeOffset? ModifiedBy { get; set; }
    }
}