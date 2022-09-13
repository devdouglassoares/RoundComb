using Core.WebApi.Controllers;
using ProductManagement.Core.Dto;
using ProductManagement.Core.Entities;
using ProductManagement.Core.Models;
using ProductManagement.Core.Services;
using Membership.Core;
using Membership.Core.Contracts.AuthAttributes;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProductManagement.Api.Controllers
{
    [RoutePrefix("api/vendors")]
	public class VendorController : BaseCrudController<Vendor, VendorDto, IVendorService>
	{
		private readonly IMembership _membership;

		public VendorController(IVendorService crudService, IMembership membership) : base(crudService)
		{
			_membership = membership;
		}


		[HttpGet, Route("getServiceProvidings")]
		public HttpResponseMessage GetServiceProvidings()
		{
			return Request.CreateResponse(HttpStatusCode.OK, CrudService.GetServiceProvidings());
		}

		[RequireAuthTokenApi]
		[HttpGet, HttpHead, Route("myVendorProfile")]
		//[ErrorResponseHandler(typeof(InvalidOperationException), HttpStatusCode.BadRequest)]
		public HttpResponseMessage GetMyVendorProfile()
		{
			return GetProfileForUser(_membership.UserId);
		}

		[RequireAuthTokenApi]
		[HttpGet, HttpHead, Route("{userId}/vendorProfile")]
		[AllowAnonymous]
		public HttpResponseMessage GetProfileForUser(long userId)
		{
			var vendorProfile = CrudService.GetVendorByUserId(userId);
			return Request.CreateResponse(HttpStatusCode.OK, vendorProfile);
		}

		[RequireAuthTokenApi]
		[HttpPost, Route("myVendorProfile")]
		public HttpResponseMessage SaveProfileForCurrentUser(VendorProfileModel model)
		{
			CrudService.UpdateVendorProfile(_membership.UserId, model);
			return Request.CreateResponse(HttpStatusCode.OK);
		}

		[RequireAuthTokenApi]
		[HttpPost, Route("{userId}/vendorProfile")]
		public HttpResponseMessage SaveProfileForUser(long userId, VendorProfileModel model)
		{
			CrudService.UpdateVendorProfile(userId, model);
			return Request.CreateResponse(HttpStatusCode.OK);
		}

	    [HttpPost, Route("vendorSearch")]
	    public HttpResponseMessage VendorSearch(VendorSearchRequestModel model)
	    {
	        return Request.CreateResponse(HttpStatusCode.OK, CrudService.VendorSearch(model));
	    }


	    [HttpPost, Route("sendVendorRequest")]
	    [RequireAuthTokenApi]
        public HttpResponseMessage SendVendorRequest(VendorRequestModel model)
	    {
            return Request.CreateResponse(HttpStatusCode.OK, CrudService.VendorRequest(_membership.UserId, model));
	    }

        /// <summary>
        /// Get from Tenants or Landlord
        /// </summary>
        /// <returns></returns>
	    [RequireAuthTokenApi]
	    [HttpGet, HttpHead, Route("myVendorRequests")]
	    public HttpResponseMessage MyVendorRequests()
	    {
	        var vendorRequests = CrudService.GetMyVendorRequests(_membership.UserId);
	        return Request.CreateResponse(HttpStatusCode.OK, vendorRequests);
	    }

	    /// <summary>
	    /// Get from Vendor
	    /// </summary>
	    /// <returns></returns>
	    [RequireAuthTokenApi]
	    [HttpGet, HttpHead, Route("getRequestsForVendor")]
	    public HttpResponseMessage GetRequestsForVendor()
	    {
	        var vendorRequests = CrudService.GetRequestForVendor(_membership.UserId);
	        return Request.CreateResponse(HttpStatusCode.OK, vendorRequests);
	    }

	    /// <summary>
	    /// Accept Vendor Request
	    /// </summary>
	    /// <returns></returns>
	    [RequireAuthTokenApi]
	    [HttpPost, HttpHead, Route("acceptVendorRequest/{requestId}")]
	    public HttpResponseMessage AcceptVendorRequest(long requestId)
	    {
	        var vendorRequests = CrudService.AcceptVendorRequest(requestId, _membership.UserId);
	        return Request.CreateResponse(HttpStatusCode.OK, vendorRequests);
	    }
    }
}