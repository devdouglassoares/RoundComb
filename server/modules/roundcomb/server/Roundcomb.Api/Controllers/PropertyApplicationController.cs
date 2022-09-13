using Core.UI;
using Core.UI.DataTablesExtensions;
using Core.WebApi.ActionFilters;
using Core.WebApi.Extensions;
using Membership.Core;
using Membership.Core.Contracts.AuthAttributes;
using Membership.Core.Models;
using Roundcomb.Core.Dtos;
using Roundcomb.Core.Exceptions;
using Roundcomb.Core.Permissions;
using Roundcomb.Core.Services;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace Roundcomb.Api.Controllers
{
	[RoutePrefix("api/application")]
	[RequireAuthTokenApi]
	[ErrorResponseHandler(typeof(InvalidOperationException), HttpStatusCode.BadRequest,
		ErrorCode = "PMApplicationPermissionError0001")]
	public class PropertyApplicationController : ApiController
	{
		private readonly IMembership _membership;

		private readonly IDataTableService _dataTableService;
		private readonly IPropertyApplicationFormInstanceService _applicationFormInstanceService;

		public PropertyApplicationController(IMembership membership,
									 IDataTableService dataTableService,
									 IPropertyApplicationFormInstanceService applicationFormInstanceService)
		{
			_membership = membership;
			_dataTableService = dataTableService;
			_applicationFormInstanceService = applicationFormInstanceService;
		}

		[HttpPost, Route("property/{propertyId:long}")]
		public HttpResponseMessage SaveFormInstance(long propertyId, PropertyApplicationFormDocumentConfigDto model)
		{
			_applicationFormInstanceService.SavePropertyApplicationForm(new PropertyApplicationFormInstanceDto
			{
				UserId = _membership.UserId,
				PropertyId = propertyId,
				UploadedApplicationFileName = string.Join(",", model.FileNames),
				UploadedApplicationFileUrl = string.Join(",", model.FileUrls),
				ResultUrl = model.ResultUrl,
				IsExternalSite = model.IsExternalSite
			});

			return Request.CreateResponse(HttpStatusCode.OK);
		}

		[HttpPost, Route("{formId:long}/property/{propertyId:long}")]
		public HttpResponseMessage SaveFormInstance(long propertyId, long formId)
		{
			_applicationFormInstanceService.SavePropertyApplicationForm(new PropertyApplicationFormInstanceDto
			{
				FormInstanceId = formId,
				UserId = _membership.UserId,
				PropertyId = propertyId
			});

			return Request.CreateResponse(HttpStatusCode.OK);
		}

		[HttpGet, Route("{applicationId:int}")]
		[ErrorResponseHandler(typeof(PropertyApplicationInvalidOwnershipAccessException), HttpStatusCode.BadRequest, ErrorCode = "PMApplicationInvalidOwnershipAccessError0001")]
		public HttpResponseMessage GetApplications(long applicationId)
		{
			var result = _applicationFormInstanceService.GetApplication(applicationId);

			if (result.UserId != _membership.UserId && !_membership.IsAccessAllowed(PermissionAuthorize.Feature(RoundcombPermissions.CanReceiveApplications)))
				throw new UnauthorizedAccessException();

			return Request.CreateResponse(HttpStatusCode.OK, result);
		}

		[HttpPost, Route("{applicationId:int}/markAsViewed")]
		[PermissionAuthorize(RoundcombPermissions.CanReceiveApplications)]
		[ErrorResponseHandler(typeof(PropertyApplicationInvalidOwnershipAccessException), HttpStatusCode.BadRequest, ErrorCode = "PMApplicationInvalidOwnershipAccessError0001")]
		public HttpResponseMessage MarkAsViewed(long applicationId)
		{
			_applicationFormInstanceService.MarkApplicationAsViewed(applicationId);

			return Request.CreateResponse(HttpStatusCode.OK);
		}

		[HttpGet, HttpHead, Route("mine")]
		[RequireAuthTokenApi]
		public HttpResponseMessage GetApplicationsForCurrentUser()
		{
			var result = _applicationFormInstanceService.GetApplicationHistoryForUser(_membership.UserId).AsQueryable()
                .OrderByDescending(x => x.ModifiedDate ?? x.CreatedDate);

			return Request.CreateResponse(HttpStatusCode.OK, result, result.Max(x => x.ModifiedDate));
		}

		[HttpPost, Route("datatables")]
		[PermissionAuthorize(RoundcombPermissions.CanReceiveApplications)]
		public HttpResponseMessage GetApplications(
			[ModelBinder(typeof(DataTableModelBinderProvider))] DefaultDataTablesRequest requestModel)
		{
			var result = _applicationFormInstanceService.GetApplicationRequestsForUser(_membership.UserId).AsQueryable();

			var response = _dataTableService.GetResponse(result, requestModel);

			return Request.CreateResponse(HttpStatusCode.OK, response);
		}

		[HttpPost, Route("{id:int}/approve")]
		[ErrorResponseHandler(typeof(PropertyApplicationInvalidOwnershipApprovalException), HttpStatusCode.BadRequest, ErrorCode = "PMApplicationInvalidOwnershipApprovalError0001")]
		[PermissionAuthorize(RoundcombPermissions.CanReceiveApplications)]
		public HttpResponseMessage ApproveApplication(long id)
		{
			_applicationFormInstanceService.ApproveApplication(id);

			return Request.CreateResponse(HttpStatusCode.OK);
		}

		[HttpPost, Route("{id:int}/reject")]
		[ErrorResponseHandler(typeof(PropertyApplicationInvalidOwnershipApprovalException), HttpStatusCode.BadRequest, ErrorCode = "PMApplicationInvalidOwnershipApprovalError0001")]
		[PermissionAuthorize(RoundcombPermissions.CanReceiveApplications)]
		public HttpResponseMessage RejectApplication(long id, PropertyApplicationRejectRequestModel model)
		{
			_applicationFormInstanceService.RejectApplication(id, model);

			return Request.CreateResponse(HttpStatusCode.OK);
		}

		[HttpPost, Route("{id:int}/accept")]
		public HttpResponseMessage AcceptApplication(long id)
		{
			_applicationFormInstanceService.AcceptApplication(id);

			return Request.CreateResponse(HttpStatusCode.OK);
		}

		[HttpPost, Route("{id:int}/decline")]
		public HttpResponseMessage DeclineApplication(long id, PropertyApplicationRejectRequestModel model)
		{
			_applicationFormInstanceService.DeclineApplication(id, model);

			return Request.CreateResponse(HttpStatusCode.OK);
		}


		[HttpPost, Route("checkContactRequestCapability/{propertyId}")]
		public HttpResponseMessage CheckReceiveApplicationPermissionAbility(long propertyId)
		{
			if (!_applicationFormInstanceService.CheckReceiveApplicationPermissionAbility(propertyId))
				throw new InvalidOperationException();

			return Request.CreateResponse(HttpStatusCode.OK);
		}
	}
}