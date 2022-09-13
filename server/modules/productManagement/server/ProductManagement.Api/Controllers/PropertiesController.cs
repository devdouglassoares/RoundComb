using Core.Database.Transactions;
using Core.Exceptions;
using Core.UI;
using Core.UI.DataTablesExtensions;
using Core.WebApi;
using Core.WebApi.ActionFilters;
using ProductManagement.Api.Models;
using ProductManagement.Core.Dto;
using ProductManagement.Core.Entities;
using ProductManagement.Core.Models;
using ProductManagement.Core.Permissions;
using ProductManagement.Core.Services;
using Membership.Core;
using Membership.Core.Contracts.AuthAttributes;
using Membership.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace ProductManagement.Api.Controllers
{
	[RoutePrefix("api/properties")]
	[ErrorResponseHandler(typeof(BaseNotFoundException<>), HttpStatusCode.NotFound)]
	public class PropertiesController : ApiController
	{
		private readonly IPropertyService _propertyService;
		private readonly IDataTableService _dataTableService;
		private readonly IMembership _membership;

		private readonly IPropertyImportService _propertyImportService;

		public PropertiesController(IPropertyService propertyService,
								  IDataTableService dataTableService,
								  IMembership membership,
								  IPropertyImportService propertyImportService)
		{
			_propertyService = propertyService;
			_dataTableService = dataTableService;
			_membership = membership;
			_propertyImportService = propertyImportService;
		}

		[HttpGet, Route("generatePropertyObject")]
		public HttpResponseMessage GeneratePropertyObject()
		{
			return Request.CreateResponse(HttpStatusCode.OK, _propertyService.GeneratePropertyDto());
		}

		[HttpPost, Route("query")]
		public HttpResponseMessage GetProperties(PropertyQueryRequestModel model)
		{
			PropertyQueryResult result = _propertyService.SearchProperties(model);

			return Request.CreateResponse(HttpStatusCode.OK, result);
		}

		[HttpPost, Route("moveProperties/{propertyIds}/{categoryId}")]
		public HttpResponseMessage MovePropertiesToCategory([ModelBinder(typeof(CommaDelimitedArrayModelBinder))]long[] propertyIds, long categoryId)
		{
			_propertyService.MovePropertiesToCategory(propertyIds, categoryId);

			return Request.CreateResponse(HttpStatusCode.OK);
		}

		[HttpPost, Route("tagging/{propertyIds}/{tags}")]
		public HttpResponseMessage AssignPropertiesToTags([ModelBinder(typeof(CommaDelimitedArrayModelBinder))]long[] propertyIds, [ModelBinder(typeof(CommaDelimitedArrayModelBinder))]string[] tags)
		{
			_propertyService.AssignPropertiesToTags(propertyIds, tags);

			return Request.CreateResponse(HttpStatusCode.OK);
		}

		[HttpPost, Route("updateStatus/{propertyIds}/{propertyStatus}")]
		public HttpResponseMessage UpdatePropertiesStatus([ModelBinder(typeof(CommaDelimitedArrayModelBinder))]long[] propertyIds, string propertyStatus)
		{
			_propertyService.UpdatePropertyStatus(propertyIds, propertyStatus);

			return Request.CreateResponse(HttpStatusCode.OK);
		}

		[HttpPost, Route("updateSellTypes/{propertyIds}/{selectedSellType}")]
		[ErrorResponseHandler(typeof(InvalidOperationException), HttpStatusCode.BadRequest)]
		public HttpResponseMessage UpdatePropertySellTypes([ModelBinder(typeof(CommaDelimitedArrayModelBinder))]long[] propertyIds, string selectedSellType)
		{
			_propertyService.UpdatePropertySellType(propertyIds, selectedSellType);

			return Request.CreateResponse(HttpStatusCode.OK);
		}

		[HttpGet, Route("featured")]
		public HttpResponseMessage GetFeaturedProperties()
		{
			IEnumerable<FeaturedPropertyDto> properties = _propertyService.GetFeaturedProperties();

			return Request.CreateResponse(HttpStatusCode.OK, new
			{
				total = properties.Count(),
				data = properties
			});
		}

		[HttpGet, Route("myProperties")]
		[PermissionAuthorize(PropertyManagementPermissions.AccessManageProperty)]
		public HttpResponseMessage GetMyProperties()
		{
			IQueryable<Property> properties = InternalGetProperties(currentUserOny: true);

			return Request.CreateResponse(HttpStatusCode.OK, properties);
		}

        [HttpPost, Route("suggestProperty/{userId}/{propertyIds}")]
		[RequireAuthTokenApi]
		public HttpResponseMessage SuggestProperty(long userId, [ModelBinder(typeof(CommaDelimitedArrayModelBinder))]long[] propertyIds)
		{
			_propertyService.SendPropertySuggestion(userId, propertyIds);
			return Request.CreateResponse(HttpStatusCode.OK);
		}

		[HttpPost, Route("datatables")]
		[PermissionAuthorize(PropertyManagementPermissions.AccessManageProperty)]
		public HttpResponseMessage GetPropertiesForDatatables([ModelBinder(typeof(DataTableModelBinderProvider))] PropertyBackendFilterQueryRequest requestModel)
		{
			bool currentUserOnly = !_membership.IsAccessAllowed(
											PermissionAuthorize.Feature(
																		PropertyManagementPermissions
																			.CanManageAllUsersProperties));
            IQueryable<Property> propeties = InternalGetProperties(requestModel.CategoryId, currentUserOnly, requestModel.QueryString, requestModel.PropertySellType, requestModel.PropertyStatus, requestModel.IsShowDeleted);

			DataTablesResponse tempResponse = _dataTableService.GetResponse(propeties, requestModel);

			IEnumerable<PropertyDto> propertyDtos = _propertyService.ToDtos(tempResponse.data.Cast<Property>());

			DataTablesResponse response = new DataTablesResponse(tempResponse.draw, propertyDtos, tempResponse.recordsFiltered, tempResponse.recordsTotal);

			return Request.CreateResponse(HttpStatusCode.OK, response);
		}

		private IQueryable<Property> InternalGetProperties(long? categoryId = null, bool currentUserOny = false,
														   string queryString = "",
														   PropertySellType? propertySellType = null,
														   PropertyStatus? propertyStatus = null,
														   bool isShowDeleted = false)
		{
			// only load master products, not loading variant products
			IQueryable<Property> properties;

			if (isShowDeleted && _membership.IsAccessAllowed(
					PermissionAuthorize.Feature(PropertyManagementPermissions.UndoDeletionOfProperties)))
			{
				properties = _propertyService.FetchDeleted();
			}
			else
			{
				properties = _propertyService.Fetch(x => x.ParentPropertyId == null);
			}


			if (categoryId.HasValue)
			{
				properties = properties.Where(p => p.CategoryId != null && p.CategoryId == categoryId);
			}

			if (currentUserOny)
			{
				properties = properties.Where(p => p.OwnerId == _membership.UserId);
			}

			if (propertySellType.HasValue)
			{
				properties = properties.Where(p => p.PropertySellTypeString == propertySellType.Value.ToString());
			}

			if (propertyStatus.HasValue)
			{
				properties = properties.Where(p => p.StatusString == propertyStatus.Value.ToString());
			}

			if (!string.IsNullOrEmpty(queryString))
			{
				properties = properties.Where(x => x.Name.ToLower().Contains(queryString.ToLower()));
			}

			return properties;
		}

		[HttpGet, Route("autocomplete/{query?}")]
		public HttpResponseMessage GetPropertyAutocomplete(string query)
		{
			IQueryable<Property> properties = _propertyService.Fetch(x => x.StatusString == PropertyStatus.AvailableForSell.ToString() && x.Name.ToLower().Contains(query.ToLower()))
										  .OrderBy(x => x.Name)
										  .Skip(0)
										  .Take(20);

			IEnumerable<PropertyDto> result = _propertyService.ToDtos(properties, false);

			return Request.CreateResponse(HttpStatusCode.OK, result);
		}

		[HttpGet, Route("{propertyId:int}")]
		public HttpResponseMessage GetProperty(long propertyId)
		{
			Property property = _propertyService.GetEntity(propertyId);

			return Request.CreateResponse(HttpStatusCode.OK, _propertyService.ToDto(property));
		}

		[HttpGet, Route("{propertyId:int}/similars")]
		public HttpResponseMessage GetPropertySimilars(long propertyId)
		{
			IEnumerable<PropertyDto> properties = _propertyService.GetSimilarsForProperty(propertyId);

			return Request.CreateResponse(HttpStatusCode.OK, properties);
		}

		[HttpGet, Route("{propertyId:int}/variants")]
		public HttpResponseMessage GetPropertyVariants(long propertyId)
		{
			IEnumerable<PropertyDto> properties = _propertyService.GetVariantsForProperty(propertyId);

			return Request.CreateResponse(HttpStatusCode.OK, properties);
		}

		[HttpPost, Route("{propertyId:int}/variants")]
		[RunInTransaction]
		public HttpResponseMessage CreatePropertyVariants(long propertyId)
		{
			PropertyDto property = _propertyService.CreateAndGetPropertyVariant(propertyId);

			return Request.CreateResponse(HttpStatusCode.OK, property);
		}

		[HttpPost, Route("{propertyId:int}/addToCart")]
		[RequireAuthTokenApi]
		[RunInTransaction]
		public HttpResponseMessage AddToCart(long propertyId, int quantity = 1)
		{
			PropertyCartDto propertyCart = _propertyService.AddToCart(propertyId, quantity);

			return Request.CreateResponse(HttpStatusCode.OK, propertyCart);
		}

		[HttpPost, Route("")]
		[RunInTransaction]
		[PermissionAuthorize(PropertyManagementPermissions.AccessManageProperty)]
		public HttpResponseMessage CreateProperty(PropertyDto model)
		{
			Property property = _propertyService.Create(model);
			return Request.CreateResponse(HttpStatusCode.Created, new { property.Id });
		}

		[HttpGet, Route("exportProperties")]
		[PermissionAuthorize(PropertyManagementPermissions.CanExportProperties)]
		public HttpResponseMessage ExportProperties()
		{
			string exportedText = _propertyService.ExportProperties();

			HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK)
			{
				Content = new ByteArrayContent(Encoding.Unicode.GetBytes(exportedText))
			};
			result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
			result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
			{
				FileName = $"property_exported_{DateTimeOffset.Now:MM/dd/yyyy_hh_mm_ss}.csv"
			};
			return result;
		}

		[HttpPost, Route("importProperties")]
		[PermissionAuthorize(PropertyManagementPermissions.CanImportProperties)]
		public async Task<HttpResponseMessage> ImportProperties()
		{
			if (!Request.Content.IsMimeMultipartContent())
			{
				throw new Exception();
			}

			MultipartMemoryStreamProvider provider = new MultipartMemoryStreamProvider();
			await Request.Content.ReadAsMultipartAsync(provider);

			HttpContent file = provider.Contents.FirstOrDefault();

			if (file == null)
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No file to import");

			Stream stream = await file.ReadAsStreamAsync();

			stream.Position = 0;

			string csvString;
			using (StreamReader reader = new StreamReader(stream, Encoding.Default))
			{
				csvString = reader.ReadToEnd().Replace("\0", string.Empty);
			}

			_propertyImportService.ImportPropertyFromCsv(csvString);

			return Request.CreateResponse(HttpStatusCode.OK, new
			{
				message = "Properties import process is scheduled. Please wait..."
			});
		}

		[HttpPost, Route("importProperties/{propertyCode}")]
		[RunInTransaction]
		public HttpResponseMessage SynchronizeProperty(string propertyCode, PropertyDto model)
		{
			_propertyService.ImportProperty(propertyCode, model);

			return Request.CreateResponse(HttpStatusCode.OK);
		}

		[HttpPost, Route("users/{userId}/ownedProperties")]
		public HttpResponseMessage GetOwnedPropertiesForUser(long userId,
														   [ModelBinder(typeof(DataTableModelBinderProvider))] DefaultDataTablesRequest requestModel)
		{
			IQueryable<Property> result = _propertyService.Fetch(x => x.OwnerId == userId);

			IQueryable<PropertyDto> propertyDtos = _propertyService.ToQueryableDtos(result);

			DataTablesResponse response = _dataTableService.GetResponse(propertyDtos, requestModel);

			return Request.CreateResponse(HttpStatusCode.OK, response);
		}

		[HttpPut, Route("{propertyId:int}")]
		[RunInTransaction]
		[PermissionAuthorize(PropertyManagementPermissions.AccessManageProperty)]
		public HttpResponseMessage UpdateProperty(long propertyId, PropertyDto model)
		{
			Property property = _propertyService.Update(model, propertyId);
			return Request.CreateResponse(HttpStatusCode.OK, new { property.Id });
		}

		[HttpDelete, Route("{propertyId:int}")]
		[RunInTransaction]
		[PermissionAuthorize(PropertyManagementPermissions.AccessManageProperty)]
		public HttpResponseMessage DeleteProperty(long propertyId)
		{
			_propertyService.Delete(propertyId);
			return Request.CreateResponse(HttpStatusCode.OK);
		}

		[HttpGet, Route("{propertyId:int}/undoDeletion")]
		[RunInTransaction]
		[PermissionAuthorize(PropertyManagementPermissions.UndoDeletionOfProperties)]
		public HttpResponseMessage UndoDeletionOfProperty(long propertyId)
		{
			_propertyService.UndoDeletionOfProperty(propertyId);
			return Request.CreateResponse(HttpStatusCode.OK);
		}
	}

	public class ActivePropertyToImport
	{
		public string[] ExternalKey { get; set; }
	}
}
