using Membership.Core;
using Membership.Core.Contracts.AuthAttributes;
using Roundcomb.Core.Services;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Roundcomb.Api.Controllers
{
    [RoutePrefix("api/propertyCustomers")]
    [RequireAuthTokenApi]
    public class PropertyCustomerController : ApiController
    {
        private readonly IPropertyCustomerConsumingMappingService _propertyCustomerConsumingMappingService;
        private readonly IMembership _membership;
        private readonly IPropertyVendorService _propertyVendorService;

        public PropertyCustomerController(IPropertyCustomerConsumingMappingService propertyCustomerConsumingMappingService, IMembership membership, IPropertyVendorService propertyVendorService)
        {
            _propertyCustomerConsumingMappingService = propertyCustomerConsumingMappingService;
            _membership = membership;
            _propertyVendorService = propertyVendorService;
        }

        [HttpGet, Route("")]
        public HttpResponseMessage GetCurrentUserCustomers(bool pastCustomers = false)
        {
            var customerLists =
                _propertyCustomerConsumingMappingService.GetPropertyCustomerMappingForUser(_membership.UserId, pastCustomers);

            return Request.CreateResponse(HttpStatusCode.OK, customerLists);
        }

        [HttpPost, Route("{id}/release")]
        public HttpResponseMessage ReleasePropertyCustomerMapping(long id)
        {
            _propertyCustomerConsumingMappingService.EndContractConsumingMapping(id);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        /// <summary>
        /// Get Properties for User to send to Vendor Requests
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("myPropertiesForVendor")]
        //[PermissionAuthorize(PropertyManagementPermissions.AccessManageProperty)]
        public HttpResponseMessage MyPropertiesForVendor()
        {
            var properties = _propertyVendorService.GetMyPropertiesForVendor(_membership).Select(r => new { Id = r.Id, Name = r.Name }).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, properties);
        }
    }
}