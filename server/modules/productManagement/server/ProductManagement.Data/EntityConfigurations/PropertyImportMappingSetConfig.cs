﻿using ProductManagement.Core.Entities;
using System.Data.Entity.ModelConfiguration;

namespace ProductManagement.Data.EntityConfigurations
{
    public class PropertyImportMappingSetConfig : EntityTypeConfiguration<PropertyImportMappingSet>
    {
        public PropertyImportMappingSetConfig()
        {
            HasKey(x => x.Id);
        }
    }
}