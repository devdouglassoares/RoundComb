using Core.BackgroundTasks;
using Core.Events;
using Core.Extensions;
using Core.Logging;
using Core.SiteSettings;
using ProductManagement.Core.Dto;
using ProductManagement.Core.Events;
using ProductManagement.Core.Settings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductManagement.Core.Services.ImportProperties
{
	public class PropertyImportBackgroundTask : IBackgroundTask, IConsumer<PropertyImportingEvent>
    {
        private static readonly object LockObject = new object();
        private static List<PropertyDto> _propertiesToImport;
        private static long _notifyToUserId;

        private readonly IPropertyService _propertyService;
        private readonly IEventPublisher _eventPublisher;
	    private readonly ISiteSettingService _siteSettingService;
        private readonly ILogger _logger = Logger.GetLogger<PropertyImportBackgroundTask>();

        public PropertyImportBackgroundTask(IPropertyService propertyService, IEventPublisher eventPublisher, ISiteSettingService siteSettingService)
        {
            _propertyService = propertyService;
            _eventPublisher = eventPublisher;
	        _siteSettingService = siteSettingService;
        }

        public void Execute()
        {
            if (_propertiesToImport == null || !_propertiesToImport.Any())
                return;

            lock (LockObject)
            {
                if (_propertiesToImport == null || !_propertiesToImport.Any())
                    return;

	            var propertySetting = _siteSettingService.GetSetting<PropertyManagementSetting>();

				propertySetting.LastImportDate = DateTimeOffset.Now;
	            
				_siteSettingService.SaveSetting(propertySetting);

	            // mark the inactive property before importing.
	            _propertyService.MarkPropertiesAsInactiveBeforeSynchronizing(_propertiesToImport.Select(x => x.ExternalKey).ToArray());

				// proceed import properties
				var result = new ImportPropertyResult
                {
                    UploadedItems = _propertiesToImport.Count,
                    ImportedItems = 0,
                    ImportPropertyResultDetails = new List<ImportPropertyResultDetail>()
                };

                PropertyDto propertyDto;

                // loop through each property and import
                while ((propertyDto = _propertiesToImport.Pop()) != null)
                {
                    try
                    {
                        _propertyService.ImportProperty(propertyDto.ExternalKey, propertyDto, false);

                        result.ImportedItems++;
                        result.ImportPropertyResultDetails.Add(new ImportPropertyResultDetail
                        {
                            Property = propertyDto,
                            IsSuccess = true,
                            Detail = $"Imported property {propertyDto.Name} successfully"
                        });
                    }
                    catch (Exception exception)
                    {
                        result.Fails++;

                        result.ImportPropertyResultDetails.Add(new ImportPropertyResultDetail
                        {
                            Property = propertyDto,
                            IsSuccess = false,
                            Detail = $"Imported property {propertyDto.Name} failed",
                            Exception = exception
                        });
                    }

                    _eventPublisher.Publish(new PropertyImportProgressUpdated
                    {
                        State = "ProductImporting",
                        Total = result.UploadedItems,
                        Failed = result.Fails,
                        Success = result.ImportedItems,
                        LastResult = result.ImportPropertyResultDetails.Last(),
                        UserId = _notifyToUserId
                    });
                }

                // clean up memory
                _propertiesToImport = null;
                _notifyToUserId = 0;
            }
        }

        public int Order => 10;

        public void HandleEvent(PropertyImportingEvent eventMessage)
        {
            SetPropertiesToImport(eventMessage.PropertiesToImport);
            SetNotifyChannel(eventMessage.ImportingByUserId);
        }
        private static void SetPropertiesToImport(List<PropertyDto> propertiesToImport)
        {
            lock (LockObject)
            {
                if (_propertiesToImport == null)
                    _propertiesToImport = new List<PropertyDto>();

                _propertiesToImport.AddRange(propertiesToImport);
            }
        }

        private static void SetNotifyChannel(long membershipUserId)
        {
            _notifyToUserId = membershipUserId;
        }
    }
}