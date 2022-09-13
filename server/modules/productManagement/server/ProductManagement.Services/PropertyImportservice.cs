using Core.Events;
using Core.Exporting;
using Core.IoC;
using ProductManagement.Core.Dto;
using ProductManagement.Core.Entities;
using ProductManagement.Core.Events;
using ProductManagement.Core.Services;
using Membership.Core;
using System;
using System.Collections.Generic;

namespace ProductManagement.Services
{
	public class PropertyImportService : IPropertyImportService
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IPropertyService _propertyService;
        private readonly IMembership _membership;
        private readonly IServiceResolver _serviceProvider;

        public PropertyImportService(IEventPublisher eventPublisher,
                                    IPropertyService propertyService,
                                    IMembership membership,
                                    IServiceResolver serviceProvider)
        {
            _eventPublisher = eventPublisher;
            _propertyService = propertyService;
            _membership = membership;
            _serviceProvider = serviceProvider;
        }


        public void ImportPropertyFromCsv(string csvSeparatedInput)
        {
            var importModel = new CsvImportModel<PropertyDto>();

            importModel.SetInstantiator(() => new PropertyDto
            {
                Status = PropertyStatus.AvailableForSell
            });

            importModel.AddColumn("Name", (csvRow, columIndex, value, p) =>
                {
                    p.Name = value;
                });

            importModel.AddColumn("Category", (csvRow, columIndex, value, p) =>
            {
                p.Category = new PropertyCategoryDto
                {
                    Name = value
                };
            });

            importModel.AddColumn("Description", (csvRow, columIndex, value, p) =>
            {
                p.ShortDescription = value;
            });

            importModel.AddColumn("Tags", (csvRow, columIndex, value, p) =>
            {
                p.Tags = value.Split(new[] { ",", ";" },
                                           StringSplitOptions.RemoveEmptyEntries);
            });

            importModel.AddColumn("ExternalKey", (csvRow, columIndex, value, p) =>
            {
                p.ExternalKey = value;
            });

            importModel.AddColumn("Price", (csvRow, columIndex, value, p) =>
            {
                decimal price;
                if (decimal.TryParse(value, out price))
                {
                    p.Price = price;
                }
            });

            importModel.AddColumn("UnitPrice", (csvRow, columIndex, value, p) =>
            {
                decimal price;
                if (decimal.TryParse(value, out price))
                {
                    p.UnitPrice = price;
                }
            });

            importModel.AddColumn("UPC", (csvRow, columIndex, value, p) =>
            {
                p.Upc = value;
            });

            importModel.AddColumn("UnitUPC", (csvRow, columIndex, value, p) =>
            {
                p.UnitUpc = value;
            });

            importModel.AddColumn("ImgUrl", (csvRow, columIndex, value, p) =>
            {
                p.Images = new List<PropertyImageDto>
                                                                 {
                                                                     new PropertyImageDto
                                                                     {
                                                                         Url = value
                                                                     }
                                                                 };
            });

            importModel.AddColumn("IsActive", (csvRow, columIndex, value, p) =>
            {
                bool isActive;
                p.IsActive = bool.TryParse(value, out isActive) && isActive;
            });

            _eventPublisher.Publish(new ImportPreparationEvent<PropertyDto>
            {
                ImportModel = importModel
            });

            importModel.SetCsvInput(csvSeparatedInput);

            //var externalKeys = new List<string>();

            // Parse all properties to memory
            var propertiesToImport = importModel.Render();

            _eventPublisher.Publish(new PropertyImportingEvent
            {
                ImportingByUserId = _membership.UserId,
                PropertiesToImport = propertiesToImport
            });
        }
    }
}