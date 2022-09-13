using Core.SiteSettings;
using ProductManagement.Core.Entities;
using ProductManagement.Core.Models;
using System;
using System.Collections.Generic;

namespace ProductManagement.Core.Settings
{
	public class PropertyManagementSetting : ISiteSettingBase
    {
        public PropertyManagementSetting()
        {
            DefaultPropertyStatusWhenImport = PropertyStatus.Draft;
            PropertyListDisplay = PropertyListDisplay.ListView;
        }

        public int MaxItemPerPage { get; set; }

        public PropertyListDisplay PropertyListDisplay { get; set; }

        public IEnumerable<PropertyPropertyDisplayConfiguration> PropertyDisplayConfig { get; set; }

        public PropertyStatus DefaultPropertyStatusWhenImport { get; set; }

        public bool EnablePropertySellType { get; set; }

        public bool UsePropertyActiveField { get; set; }

        public bool EnableEcommerceManagement { get; set; }

        public PropertySellType? DefaultPropertySellType { get; set; }

        public bool PromptUserForSubscriptionToPropertyManagementModule { get; set; }

        public bool EnableContactPropertyOwner { get; set; }

        public bool AlwaysEnableSimilarProperty { get; set; }

        public bool EnableSendPropertyRequestApplication { get; set; }

        public bool SeparatePropertyLocationFromUserLocation { get; set; }

        public bool SyncUserLocationsToPropertyLocation { get; set; }

        public bool EnablePropertyOrdering { get; set; }

        public string SubmitApplicationErrorNotifyToLandlordEmailSubject { get; set; }

        public string SubmitApplicationErrorNotifyToLandlordEmailTemplate { get; set; }

		public DateTimeOffset? LastImportDate { get; set; }
    }
}