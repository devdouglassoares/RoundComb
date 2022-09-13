using Core.Database;
using Core.DynamicProperties.Services;
using Core.Events;
using Core.Exceptions;
using Core.Exporting;
using Core.Extensions;
using Core.ObjectMapping;
using Core.SiteSettings;
using Core.Templating.Services;
using Core.UI;
using Membership.Core;
using Membership.Core.Contracts;
using Membership.Core.Models;
using notifyService.Services;

//using NotifyService.RestClient.Services;
using ProductManagement.Core.Dto;
using ProductManagement.Core.EmailTemplates;
using ProductManagement.Core.Entities;
using ProductManagement.Core.Events;
using ProductManagement.Core.Models;
using ProductManagement.Core.Permissions;
using ProductManagement.Core.Repositories;
using ProductManagement.Core.Services;
using ProductManagement.Core.Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Hosting;

namespace ProductManagement.Services
{
    public class PropertyService : BaseService<Property, PropertyDto>,
								  IPropertyService,
								  IConsumer<EntitiesDtoWireUp<Property, PropertyDto>>,
								  IConsumer<DataTableDataSourceFiltered<PropertyDto>>
	{
		private readonly IPropertyCategoryService _categoryService;
		private readonly IDynamicPropertyValueService _dynamicPropertyValueService;
		private readonly IEventPublisher _eventPublisher;
		private readonly IMembership _membership;
		private readonly IPropertyImageService _propertyImageService;
		private readonly ITagService _tagService;
		private readonly IUserService _userService;
		private readonly ISiteSettingService _siteSettingService;
		private readonly ITemplateService _templateService;
		//private readonly INotificationService _notificationService;
		private readonly IPropertyCartService _propertyCartService;
		private readonly PropertyManagementSetting _propertyManagementSetting;
		private readonly bool _isFirstImportCompleted;

		public PropertyService(IMappingService mappingService,
							  IRepository repository,
							  ITagService tagService,
							  IPropertyImageService propertyImageService,
							  IDynamicPropertyValueService dynamicPropertyValueService,
							  IUserService userService,
							  IMembership membership,
							  IEventPublisher eventPublisher,
							  IPropertyCategoryService categoryService,
							  ISiteSettingService siteSettingService,
							  IPropertyCartService propertyCartService,
							  ITemplateService templateService
							//  INotificationService notificationService
            )
			: base(mappingService, repository)
		{
			_tagService = tagService;
			_propertyImageService = propertyImageService;
			_dynamicPropertyValueService = dynamicPropertyValueService;
			_userService = userService;
			_membership = membership;
			_eventPublisher = eventPublisher;
			_categoryService = categoryService;
			_siteSettingService = siteSettingService;
			_propertyCartService = propertyCartService;
			_templateService = templateService;
			//_notificationService = notificationService;

			_propertyManagementSetting = siteSettingService.GetSetting<PropertyManagementSetting>();

			bool.TryParse(ConfigurationManager.AppSettings["FirstImported"], out _isFirstImportCompleted);
		}

		public override IQueryable<Property> GetAll()
		{
			var inactiveUsers = _userService.QueryUsers()
											.Where(user => !user.IsActive || !user.IsApproved || user.IsDeleted)
											.Select(x => x.Id)
											.ToArray();

			return base.Fetch(
				x =>
					!x.IsDeleted && (x.Category == null || !x.Category.IsDeleted) &&
					(!x.OwnerId.HasValue || !inactiveUsers.Contains(x.OwnerId.Value)));
		}

		public override IQueryable<Property> Fetch(Expression<Func<Property, bool>> expression)
		{
			var inactiveUsers = _userService.QueryUsers()
											.Where(user => !user.IsActive || !user.IsApproved || user.IsDeleted)
											.Select(x => x.Id)
											.ToArray();

			return base.Fetch(
				x =>
				!x.IsDeleted && (x.Category == null || !x.Category.IsDeleted) &&
				(!x.OwnerId.HasValue || !inactiveUsers.Contains(x.OwnerId.Value)))
					   .Where(expression);
		}

		public IQueryable<Property> FetchDeleted()
		{
			return base.Fetch(x => x.IsDeleted);
		}


		public virtual IEnumerable<FeaturedPropertyDto> GetFeaturedProperties()
		{
			var properties = Repository.Fetch<Property>(
				p =>
				p.IsFeatured && !p.IsDeleted &&
				p.StatusString == PropertyStatus.AvailableForSell.ToString() &&
				p.OwnerId != null)
									 .OrderByDescending(p => p.ModifiedDate)
									 .GroupBy(p => p.OwnerId)
									 .Select(p => p.FirstOrDefault());

			var propertiesDto = MappingService.Map<List<PropertyDto>>(properties);

			return propertiesDto.Where(
				x =>
				_membership.IsAccessAllowed(
					PermissionAuthorize.Feature(
						PropertyManagementPermissions
							.CanCreateFeaturedProperties),
					x.OwnerId.Value))
							  .Select(x => MappingService.Map<FeaturedPropertyDto>(x));
		}

		public PropertyDto CreateAndGetPropertyVariant(long propertyId)
		{
			var parentPropertyId = propertyId;
			var propertyToClone = GetDto(propertyId);

			if (propertyToClone.ParentPropertyId != null)
			{
				parentPropertyId = propertyToClone.ParentPropertyId.Value;
				propertyToClone = GetDto(propertyToClone.ParentPropertyId);
			}

			propertyToClone.Sku = "";
			propertyToClone.Upc = "";

			propertyToClone.ExternalKey = "";

			propertyToClone.UnitUpc = "";

			if (propertyToClone.ParentPropertyId == null)
			{
				propertyToClone.ParentPropertyId = parentPropertyId;
			}

			propertyToClone.Images = propertyToClone.Images.Select(img => new PropertyImageDto
			{
				Url = img.Url
			});

			var clonedProperty = Create(propertyToClone);

			return ToDto(clonedProperty);
		}

        public IEnumerable<PropertyDto> GetSimilarsForProperty(long propertyId)
        {
            var property = GetEntity(propertyId);

            var properties = Fetch(p => p.StatusString == PropertyStatus.AvailableForSell.ToString());

            properties = properties
                .Where(p => p.LocationId == property.LocationId)
                .OrderBy(p => p.Price);

            return ToQueryableDtos(properties);
        }

        public IEnumerable<PropertyDto> GetVariantsForProperty(long propertyId)
		{
			var property = GetEntity(propertyId);

			var masterProperty = property.ParentProperty ?? property;

			return ToQueryableDtos(new[] { masterProperty }.Concat(masterProperty.PropertyVariants).AsQueryable());
		}

		public override PropertyDto GetDto(params object[] ids)
		{
			var propertyDto = base.GetDto(ids);

			_eventPublisher.Publish(new PropertyDtoWired { PropertyDto = propertyDto });

			return propertyDto;
		}

		public override Property Create(PropertyDto model)
		{
			if (!_propertyManagementSetting.UsePropertyActiveField)
			{
				model.IsActive = true;
			}

            model.Name = HandlePropertyName(model.NameFormat, model.UnitNumber, model.Name);

            var property = base.Create(model);
			UpdatePropertyAdditionalFields(property, model);

			UpdatePropertyImages(property, model);
			_eventPublisher.Publish(new PropertyCreated { Property = property, PropertyDto = model });


            notifyService.Services.notifyService _notificationService = new notifyService.Services.notifyService();


            _notificationService.SendEmailToVendorAddedNewProperty(_membership.Email, model.Images.ElementAt(0).Url, model.Name);
            // main image
            //model.Images.ElementAt(0).Url

            //model.name

            //_membership.Email
            return property;
		}

		public override Property Update(PropertyDto model, params object[] keys)
		{
			if (!_propertyManagementSetting.UsePropertyActiveField)
			{
				model.IsActive = true;
			}

            model.Name = HandlePropertyName(model.NameFormat, model.UnitNumber, model.Name);

            var currentLocationId = GetEntity(model.Id)?.LocationId;

            var property = base.Update(model, keys);

			UpdatePropertyImages(property, model);

            if (currentLocationId != model.LocationId)
            {
                UpdateVariantAddresses(property, model.NameFormat);
            }

            var entity = GetEntity(property.Id);

			_eventPublisher.Publish(new PropertyUpdated { Property = entity, PropertyDto = model });
			return property;
		}

        private string HandlePropertyName(string nameFormat, string unitNumber, string defaultName)
        {
            if (string.IsNullOrWhiteSpace(nameFormat))
            {
                return defaultName;
            }

            if (!nameFormat.Contains("{0}"))
            {
                return nameFormat;
            }

            var name = string.Format(nameFormat, unitNumber);
            if (name.StartsWith(","))
            {
                name = name.Substring(1);
            }

            return name;
        }

        private void UpdateVariantAddresses(Property entity, string nameFormat)
        {
            bool needToSave = false;

            if (entity.ParentProperty != null)
            {
                needToSave = true;
                entity.ParentProperty.Name = HandlePropertyName(nameFormat, entity.ParentProperty.UnitNumber, entity.ParentProperty.Name);
                entity.ParentProperty.LocationId = entity.LocationId;

                foreach (var variant in entity.PropertyVariants)
                {
                    variant.LocationId = entity.LocationId;
                    variant.Name = HandlePropertyName(nameFormat, variant.UnitNumber, variant.Name);
                }
            }

            if (entity.PropertyVariants != null && entity.PropertyVariants.Any())
            {
                needToSave = true;

                foreach (var variant in entity.PropertyVariants)
                {
                    variant.LocationId = entity.LocationId;
                    variant.Name = HandlePropertyName(nameFormat, variant.UnitNumber, variant.Name);
                }
            }

            if (needToSave)
            {
                Repository.SaveChanges();
            }
        }

        public override void Delete(params object[] keys)
		{
			var property = GetEntity(keys);

			property.IsDeleted = true;
			Repository.Update(property);
			Repository.SaveChanges();

			_eventPublisher.Publish(new PropertyDeleted { Property = property });
		}

		public virtual PropertyWithSettingDto GeneratePropertyDto()
		{
			var propertyDto = MappingService.Map<PropertyWithSettingDto>(new Property
			{
				//ProductSetting = new ProductSetting()
			});

			propertyDto.ExtendedProperties = _dynamicPropertyValueService.GetAvailableExtendedPropertyValues<Property>();
			propertyDto.Category = new PropertyCategoryDto();

			_eventPublisher.Publish(new PropertyDtoWired { PropertyDto = propertyDto });
			return propertyDto;
		}

		public List<Hashtable> GetPropertyDataForPrint(PropertyQueryRequestModel filterModel)
		{
			var propertyDtos = QueryProperties(filterModel, false);
			var properties = propertyDtos.Data.ToList();

			var propertyDataToRender = new List<Hashtable>();

			foreach (var propertyDto in properties)
			{
				var dictionary = propertyDto.ToDictionary(true);

				var hashtable = new Hashtable();

				foreach (var dictionaryKey in dictionary.Keys)
				{
					hashtable.Add($"{{{dictionaryKey}}}", dictionary[dictionaryKey]);
				}

				var propertyImage = propertyDto.Images.FirstOrDefault(x => x.IsMainImage) ?? propertyDto.Images.FirstOrDefault();

				var propertyImageUrl = propertyImage?.Url ?? "";

				if (!string.IsNullOrEmpty(propertyImageUrl) && !propertyImageUrl.StartsWith("http:") && !propertyImageUrl.StartsWith("https:"))
				{
					propertyImageUrl = HostingEnvironment.MapPath(propertyImageUrl);
				}
				hashtable.Add("{ImageUrl}", propertyImageUrl);

				propertyDataToRender.Add(hashtable);
			}
			return propertyDataToRender;
		}

		public void MovePropertiesToCategory(long[] propertyIds, long categoryId)
		{
			var category = Repository.Get<PropertyCategory>(categoryId);

			if (category == null)
				throw new BaseNotFoundException<PropertyCategory>();

			Repository.Update<Property>(x => propertyIds.Contains(x.Id), p => new Property { CategoryId = categoryId });
			Repository.SaveChanges();
		}

		public PropertyQueryResult QueryProperties(PropertyQueryRequestModel model, bool pagination = true)
		{
			var properties = Fetch(p => p.StatusString == PropertyStatus.AvailableForSell.ToString());

			if (_propertyManagementSetting.AlwaysEnableSimilarProperty)
			{
				properties = properties.Where(p => p.ParentPropertyId == null);
			}

			if (_propertyManagementSetting.UsePropertyActiveField)
			{
				properties = properties.Where(p => p.IsActive);
			}

			if (_propertyManagementSetting.EnablePropertySellType)
			{
				properties = properties.Where(p => p.PropertySellTypeString != PropertySellType.Unavailable.ToString());
			}

			if (model.CategoryId.HasValue)
			{
				var childrenCategories = _categoryService.Fetch(
					x =>
					x.ParentCategoryId != null &&
					x.ParentCategoryId == model.CategoryId.Value)
														 .Select(x => x.Id)
														 .ToArray()
														 .Concat(new[] { model.CategoryId.Value });

				properties =
					properties.Where(
						p => p.Category != null && childrenCategories.Contains(p.Category.Id));
			}

			if (model.PropertySellType.HasValue)
			{
				properties =
					properties.Where(p => p.PropertySellTypeString == model.PropertySellType.Value.ToString());
			}

			if (model.MinPrice.HasValue)
			{
				properties = properties.Where(p => p.Price >= model.MinPrice);
			}

			if (model.MaxPrice.HasValue)
			{
				properties = properties.Where(p => p.Price <= model.MaxPrice);
			}

			if (model.LocationIds != null && model.LocationIds.Any())
			{
				if (_propertyManagementSetting.SyncUserLocationsToPropertyLocation)
				{
					var userIds = _userService.GetUserIdsInLocations(model.LocationIds);

					properties =
						properties.Where(
							p =>
							p.OwnerId != null &&
							userIds.Contains(p.OwnerId.Value));
				}
				else
				{
					properties =
						properties.Where(
							p =>
							p.LocationId != null &&
							model.LocationIds.Contains(p.LocationId.Value));
				}
			}

			if (model.SearchByLocations && (model.LocationIds == null || !model.LocationIds.Any()))
			{
				properties = properties.Where(p => p.LocationId != null && p.LocationId == -1);
				// always false
			}

			if (model.TagNames != null && model.TagNames.Any())
			{
				properties =
					properties.Where(
						p =>
						p.Tags.Any() && p.Tags.Any(tag => model.TagNames.Contains(tag.Name)));
			}

			if (!string.IsNullOrEmpty(model.Keyword))
			{
				properties = properties.Where(x => x.Name.ToLower().Contains(model.Keyword.ToLower()) ||
											   x.ShortDescription.ToLower().Contains(model.Keyword.ToLower()) ||
											   x.FullDescription.ToLower().Contains(model.Keyword.ToLower()));
			}

			if (model.DynamicPropertyFilters != null && model.DynamicPropertyFilters.Any() &&
				model.DynamicPropertyFilters.Values.Any(filter => filter.HasFilter))
			{
				var propertyIdsFromDynamicProps =
					_dynamicPropertyValueService.GetEntitiesHasValues<Property>(model.DynamicPropertyFilters).ToArray();

				properties = properties.Where(x => propertyIdsFromDynamicProps.Contains(x.Id));
			}

			var counts = properties.Count();

			properties = properties.OrderBy(product => product.Name);

			if (pagination)
			{
				if (!model.PageSize.HasValue)
					model.PageSize = _propertyManagementSetting.MaxItemPerPage;

				properties = properties
								   .Skip(model.PageSize.Value * (model.PageIndex ?? 0))
								   .Take(model.PageSize.Value);
			}


			var result = ToDtos(properties).ToArray();

			_eventPublisher.Publish(new DataTableDataSourceFiltered<PropertyDto> { Datasource = result });

			return new PropertyQueryResult
			{
				Count = counts,
				Data = result,
				PageIndex = model.PageIndex ?? 0
			};
		}

        public PropertyQueryResult SearchProperties(PropertyQueryRequestModel model, bool pagination = true)
        {
            var properties = Fetch(p => p.StatusString == PropertyStatus.AvailableForSell.ToString());

            if (_propertyManagementSetting.AlwaysEnableSimilarProperty)
            {
                properties = properties.Where(p => p.ParentPropertyId == null);
            }

            if (_propertyManagementSetting.UsePropertyActiveField)
            {
                properties = properties.Where(p => p.IsActive);
            }

            if (_propertyManagementSetting.EnablePropertySellType)
            {
                properties = properties.Where(p => p.PropertySellTypeString != PropertySellType.Unavailable.ToString());
            }

            if (model.CategoryId.HasValue)
            {
                var childrenCategories = _categoryService.Fetch(
                    x =>
                    x.ParentCategoryId != null &&
                    x.ParentCategoryId == model.CategoryId.Value)
                                                         .Select(x => x.Id)
                                                         .ToArray()
                                                         .Concat(new[] { model.CategoryId.Value });

                properties =
                    properties.Where(
                        p => p.Category != null && childrenCategories.Contains(p.Category.Id));
            }

            if (model.PropertySellType.HasValue)
            {
                properties =
                    properties.Where(p => p.PropertySellTypeString == model.PropertySellType.Value.ToString());
            }

            if (model.MinPrice.HasValue)
            {
                properties = properties.Where(p => p.Price >= model.MinPrice);
            }

            if (model.MaxPrice.HasValue)
            {
                properties = properties.Where(p => p.Price <= model.MaxPrice);
            }

            if (model.LocationIds != null && model.LocationIds.Any())
            {
                if (_propertyManagementSetting.SyncUserLocationsToPropertyLocation)
                {
                    var userIds = _userService.GetUserIdsInLocations(model.LocationIds);

                    properties =
                        properties.Where(
                            p =>
                            p.OwnerId != null &&
                            userIds.Contains(p.OwnerId.Value));
                }
                else
                {
                    properties =
                        properties.Where(
                            p =>
                            p.LocationId != null &&
                            model.LocationIds.Contains(p.LocationId.Value));
                }
            }

            if (model.SearchByLocations && (model.LocationIds == null || !model.LocationIds.Any()))
            {
                properties = properties.Where(p => p.LocationId != null && p.LocationId == -1);
                // always false
            }

            if (model.TagNames != null && model.TagNames.Any())
            {
                properties =
                    properties.Where(
                        p =>
                        p.Tags.Any() && p.Tags.Any(tag => model.TagNames.Contains(tag.Name)));
            }

            if (!string.IsNullOrEmpty(model.Keyword))
            {
                properties = properties.Where(x => x.Name.ToLower().Contains(model.Keyword.ToLower()) ||
                                               x.ShortDescription.ToLower().Contains(model.Keyword.ToLower()) ||
                                               x.FullDescription.ToLower().Contains(model.Keyword.ToLower()));
            }

            if (model.DynamicPropertyFilters != null && model.DynamicPropertyFilters.Any() &&
                model.DynamicPropertyFilters.Values.Any(filter => filter.HasFilter))
            {
                var propertyIdsFromDynamicProps =
                    _dynamicPropertyValueService.GetEntitiesHasValues<Property>(model.DynamicPropertyFilters).ToArray();

                properties = properties.Where(x => propertyIdsFromDynamicProps.Contains(x.Id));
            }

            var counts = properties.Count();

            if (pagination)
            {
                if (!model.PageSize.HasValue)
                    model.PageSize = _propertyManagementSetting.MaxItemPerPage;

                properties = properties
                    .OrderBy(p => p.Price)
                    .ThenBy(p => p.Name)
                    .Skip(model.PageSize.Value * (model.PageIndex ?? 0))
                    .Take(model.PageSize.Value);
            }

            var count = properties.Count();

            var result = ToDtos(properties.OrderBy(p => p.Price).ThenBy(p => p.Name)).ToArray();

            _eventPublisher.Publish(new DataTableDataSourceFiltered<PropertyDto> { Datasource = result });

            return new PropertyQueryResult
            {
                Count = counts,
                Data = result,
                PageIndex = model.PageIndex ?? 0
            };
        }

        public PropertyCartDto AddToCart(long propertyId, int quantity)
		{
			var cart = _propertyCartService.GetAvailableCartForUser(_membership.UserId);

			if (cart == null)
			{
				cart = new PropertyCart
				{
					UserId = _membership.UserId,
					CreatedDate = DateTimeOffset.Now,
				};
				Repository.Insert(cart);
				Repository.SaveChanges();
			}

			if (cart.Items == null)
				cart.Items = new List<PropertyCartItem>();

			var exitingPropertyItem = cart.Items.FirstOrDefault(cartitem => cartitem.PropertyId == propertyId);

			if (exitingPropertyItem == null)
				cart.Items.Add(new PropertyCartItem
				{
					PropertyId = propertyId,
					Quantity = quantity,
					ModifiedDate = DateTimeOffset.Now
				});
			else
			{
				exitingPropertyItem.Quantity += quantity;
				exitingPropertyItem.ModifiedDate = DateTimeOffset.Now;
			}

			Repository.Update(cart);
			Repository.SaveChanges();

			return _propertyCartService.ToDto(cart);
		}

		public void UpdatePropertySellType(long[] propertyIds, string selectedSellType)
		{
			PropertySellType value;
			if (!Enum.TryParse(selectedSellType, out value))
			{
				throw new InvalidOperationException($"Selected sell type '{selectedSellType}' is not supported");
			}

			Repository.Update<Property>(p => propertyIds.Contains(p.Id), p => new Property
			{
				PropertySellTypeString = selectedSellType
			});
			Repository.SaveChanges();
		}

		public void UpdatePropertyStatus(long[] propertyIds, string selectedStatus)
		{
			PropertyStatus value;
			if (!Enum.TryParse(selectedStatus, out value))
			{
				throw new InvalidOperationException($"Selected status '{selectedStatus}' is not supported");
			}

			Repository.Update<Property>(p => propertyIds.Contains(p.Id), p => new Property
			{
				StatusString = selectedStatus,
				ModifiedDate = DateTimeOffset.Now,
				ModifiedBy = _membership.Name
			});
			Repository.SaveChanges();
		}

		public void SendPropertySuggestion(long userId, long[] propertyIds)
		{
			var generalSetting = _siteSettingService.GetSetting<GeneralSiteSetting>();
			var adminEmail = generalSetting.AdminContactEmails.FirstOrDefault(x => x.IsDefaultToSendNotification);
			if (adminEmail == null)
			{
				return;
			}

			var recipient = _userService.GetUser(userId);

			var model = new PropertySuggestionEmailTemplate
			{
				SenderName = _membership.Name,
				RecipientName = $"{recipient.FirstName} {recipient.LastName}",
				SentDate = DateTimeOffset.Now,
				SuggestPropertyItems = new List<SuggestPropertyItem>()
			};

			foreach (var propertyId in propertyIds)
			{
				var property = GetEntity(propertyId);
				model.SuggestPropertyItems.Add(new SuggestPropertyItem
				{
					Id = propertyId,
					Name = property.Name,
					ImageUrl = property.Images.FirstOrDefault()?.Url
				});
			}

			var template = _templateService.ParseTemplate(model);

			var title = template.TemplateTitle;
			var content = template.TemplateContent;
			/*_notificationService.SendEmail(new[] { recipient.Email }, title, content, adminEmail.EmailAddress,
										   adminEmail.DisplayName);*/
		}

		public IEnumerable<PropertyDto> GetDtos(long[] propertyIds)
		{
			var entities = Fetch(x => propertyIds.Contains(x.Id));

			var dtos = ToDtos(entities);

			return dtos;
		}

		public void MarkPropertiesAsInactiveBeforeSynchronizing(string[] activePropertiesExternalCodes)
		{
			Repository.Update<Property>(p => !activePropertiesExternalCodes.Contains(p.ExternalKey), x => new Property
			{
				StatusString = PropertyStatus.NotAvailableOrTaken.ToString(),
				IsActive = false,
				ModifiedDate = DateTimeOffset.Now,
				ModifiedBy = "Set inactive status from importing"
			});
			Repository.SaveChanges();
		}

		public void ImportProperty(string propertyCode, PropertyDto model, bool fireImportEvent = true)
		{
			var propertyEntity = First(p => p.ExternalKey == propertyCode);

			model.IsActive = true;
			Property entity;

			if (propertyEntity != null)
			{
				// if property is already inactive or not available, ignore it
				if (!propertyEntity.IsActive || propertyEntity.Status == PropertyStatus.NotAvailableOrTaken)
					return;

				/**
                 * 2016-11-19
                 * if product already have category assigned, then ignore the category from importing model.
                 */
				if (propertyEntity.Category != null || propertyEntity.CategoryId != null)
				{
					if (propertyEntity.Category.IsDeleted)
					{
						var category =
							_categoryService.First(cat => cat.Name == propertyEntity.Category.Name && !cat.IsDeleted);

						if (category != null)
						{
							propertyEntity.CategoryId = category.Id;
						}
					}
					model.Category = null;
					model.CategoryId = propertyEntity.CategoryId;
				}
				model.ModifiedBy = "Importing";

				entity = base.Update(model, propertyEntity.Id);
				UpdatePropertyImages(entity, model);
			}
			else
			{
				if (_isFirstImportCompleted)
				{
					model.Category = new PropertyCategoryDto
					{
						Name = PropertyCategory.UnknownCategory
					};
					model.CategoryId = null;
				}
				entity = Create(model);
			}

			if (fireImportEvent)
				_eventPublisher.Publish(new PropertyImportedEvent { Property = entity, PropertyDto = model });
		}

		public string ExportProperties()
		{
			var exportModel = new CsvExportModel<PropertyDto>();

			exportModel.AddColumn("Id", dto => dto.Id);
			exportModel.AddColumn("Name", dto => dto.Name);
			exportModel.AddColumn("Description", dto => dto.ShortDescription);
			exportModel.AddColumn("SKU", dto => dto.Sku);
			exportModel.AddColumn("ExternalItemCode", dto => dto.ExternalKey);
			exportModel.AddColumn("Price", dto => dto.Price);
			exportModel.AddColumn("UnitPrice", dto => dto.UnitPrice);
			exportModel.AddColumn("UPC", dto => dto.Upc);
			exportModel.AddColumn("UnitUPC", dto => dto.UnitUpc);
			exportModel.AddColumn("IsActive", dto => dto.IsActive ? "True" : "False");

			_eventPublisher.Publish(new ExportPreparationEvent<PropertyDto> { ExportModel = exportModel });

			var properties = base.GetAll();

			var dtos = ToDtos(properties);

			return exportModel.Render(dtos);
		}

		public void AssignPropertiesToTags(long[] propertyIds, string[] tags)
		{
			var properties = Repository.Fetch<Property>(x => propertyIds.Contains(x.Id));

			foreach (var tag in tags)
			{
				var t = _tagService.First(x => x.Name == tag) ?? new Tag
				{
					Name = tag,
					CreatedDate = DateTimeOffset.Now
				};
				foreach (var property in properties)
				{
					property.Tags.Add(t);
				}
			}
		}

		public override PropertyDto ToDto(Property entity)
		{
			var propertyDto = base.ToDto(entity);

			propertyDto.ExtendedProperties =
				_dynamicPropertyValueService.GetExtendedPropertyValuesForEntity<Property>(entity.Id);

			MapUserInfoToProperty(propertyDto);

			_eventPublisher.Publish(new PropertyDtoWired { PropertyDto = propertyDto });
			return propertyDto;
		}

		private void MapUserInfoToProperty(PropertyDto propertyDto)
		{
			if (propertyDto.OwnerId.HasValue && propertyDto.OwnerId != 0)
			{
				var user = _userService.GetUser(propertyDto.OwnerId.Value);
				if (user != null)
				{
					propertyDto.OwnerName = user.FirstName + " " + user.LastName;
				}
			}
		}

		public override Property PrepareForInserting(Property entity, PropertyDto model)
		{
			entity = base.PrepareForInserting(entity, model);

			try
			{
				entity.OwnerId = _membership.UserId;
			}
			catch (UnauthorizedAccessException)
			{
                entity.OwnerId = model.OwnerId;
			}

			if (entity.OwnerId.HasValue &&
				_membership.IsAccessAllowed(
					PermissionAuthorize.Feature(
						PropertyManagementPermissions
							.CanCreateFeaturedProperties),
					entity.OwnerId.Value))
			{
				if (entity.IsFeatured)
				{
					Repository.Update<Property>(p => p.OwnerId == entity.OwnerId,
											   p => new Property { IsFeatured = false });
				}
			}
			else
			{
				entity.IsFeatured = false;
			}

			entity.Category = model.CategoryId.HasValue && model.CategoryId > 0
								  ? Repository.Get<PropertyCategory>(model.CategoryId)
								  : null;

			if ((model.CategoryId == null || model.CategoryId == 0) && !string.IsNullOrEmpty(model.Category?.Name))
			{
				var category = _categoryService.CreateOrUpdate(model.Category);
				entity.Category = category;
			}

			UpdatePropertyTags(entity, model.Tags);

			if (!_propertyManagementSetting.EnablePropertySellType)
			{
				entity.PropertySellType = _propertyManagementSetting.DefaultPropertySellType;
			}

			return entity;
		}

		public override Property PrepareForUpdating(Property entity, PropertyDto model)
		{
			entity = base.PrepareForUpdating(entity, model);

			if (entity.OwnerId.HasValue &&
				_membership.IsAccessAllowed(
					PermissionAuthorize.Feature(
						PropertyManagementPermissions
							.CanCreateFeaturedProperties),
					entity.OwnerId.Value))
			{
				if (entity.IsFeatured)
				{
					Repository.Update<Property>(p => p.OwnerId == entity.OwnerId,
											   p => new Property { IsFeatured = false });
				}
			}
			else
			{
				entity.IsFeatured = false;
			}
			entity.Tags.Clear();


			if (!model.CategoryId.HasValue || model.CategoryId == 0)
			{
				if (!string.IsNullOrEmpty(model.Category?.Name))
				{
					var category = _categoryService.CreateOrUpdate(model.Category);
					entity.Category = category;
					entity.CategoryId = category.Id;
				}
			}
			else if (model.CategoryId != entity.Category?.Id)
			{
				entity.Category = Repository.Get<PropertyCategory>(model.CategoryId.Value);
				entity.CategoryId = entity.Category?.Id;
			}

			UpdatePropertyTags(entity, model.Tags);

			UpdatePropertyAdditionalFields(entity, model);

			if (!_propertyManagementSetting.EnablePropertySellType)
			{
				entity.PropertySellType = _propertyManagementSetting.DefaultPropertySellType;
			}

			return entity;
		}

		protected virtual void UpdatePropertyAdditionalFields(Property entity, PropertyDto model)
		{
			_dynamicPropertyValueService.UpdateEntityAdditionalFields<Property>(entity.Id, model.ExtendedProperties);
		}

		protected virtual void UpdatePropertyImages(Property entity, PropertyDto model)
		{
			var imageIds = model.Images.Select(img => img.Id)
								.Where(id => id != 0)
								.ToArray();

			if (!imageIds.Any())
			{
				foreach (var propertyImage in entity.Images)
				{
					_propertyImageService.Delete(propertyImage.Id);
				}
			}

			foreach (var image in model.Images)
			{
				var existingImage = entity.Images.FirstOrDefault(img => img.Url == image.Url);
				if (existingImage != null)
				{
					existingImage.IsDeleted = false;
					Repository.Update(existingImage);
					continue;
				}

				if (image.Id != 0)
				{
					_propertyImageService.Update(image, image.Id);
				}
				else
				{
					entity.Images.Add(MappingService.Map<PropertyImage>(image));
				}
			}
			Repository.SaveChanges();
		}

		protected virtual void UpdatePropertyTags(Property entity, string[] tags)
		{
			foreach (var tag in tags)
			{
				var t = _tagService.First(x => x.Name == tag);
				if (t != null)
				{
					entity.Tags.Add(t);
				}
				else
				{
					entity.Tags.Add(new Tag
					{
						Name = tag,
						CreatedDate = DateTimeOffset.Now
					});
				}
			}
		}

		public void UndoDeletionOfProperty(long propertyId)
		{
			var property = Repository.Get<Property>(x => x.Id == propertyId);
			if (property == null)
			{
				throw new BaseNotFoundException<Property>();
			}
			property.IsDeleted = false;
			Repository.Update(property);
			Repository.SaveChanges();
		}

		#region IConsumer<DataTableDataSourceFiltered<PropertyDto>> members

		public int Order => 10;

		public void HandleEvent(DataTableDataSourceFiltered<PropertyDto> eventMessage)
		{
			foreach (var propertyDto in eventMessage.Datasource.Where(x => x.OwnerId != null && x.OwnerId > 0))
			{
				MapUserInfoToProperty(propertyDto);
			}
		}

		public void HandleEvent(EntitiesDtoWireUp<Property, PropertyDto> eventMessage)
		{
			foreach (var propertyDto in eventMessage.Dto)
			{
				MapUserInfoToProperty(propertyDto);
				var propertyExtension =
				   _dynamicPropertyValueService.GetExtendedPropertyValuesForEntity<Property>(propertyDto.Id); ;

				if (propertyExtension != null)
				{
					propertyDto.ExtendedProperties = propertyExtension;
				}
			}
		}

		#endregion
	}
}