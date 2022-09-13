using Core;
using Core.Database;
using ProductManagement.Core.Dto;
using ProductManagement.Core.Entities;
using ProductManagement.Core.Models;
using System.Collections.Generic;

namespace ProductManagement.Core.Services
{
    public interface IVendorService : IBaseService<Vendor, VendorDto>, IDependency
	{

		VendorProfileModel GetVendorByUserId(long userId);
		List<ServiceProviding> GetServiceProvidings();

		/// <summary>
		/// Update Vendor Profile
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="model"></param>
		void UpdateVendorProfile(long userId, VendorProfileModel model);

        /// <summary>
        /// Search Vendor
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        VendorSearchResponseModel VendorSearch(VendorSearchRequestModel model);

        /// <summary>
        /// Vendor Request from Landlord or Tenant
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        bool VendorRequest(long currentUserId, VendorRequestModel model);

        /// <summary>
        /// Get My Vendor Request from Tenant or Landlord
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
	    List<VendorRequestItemModel> GetMyVendorRequests(long currentUserId);

        /// <summary>
        /// Get My Vendor Request from Vendor (all requests from tenants or landlords)
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
	    List<VendorRequestItemModel> GetRequestForVendor(long currentUserId);

        /// <summary>
        /// Accept Request
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="vendorUserId"></param>
        /// <returns></returns>
	    bool AcceptVendorRequest(long requestId, long vendorUserId);

	}
}