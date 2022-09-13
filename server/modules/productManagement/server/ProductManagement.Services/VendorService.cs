using Core.Database;
using Core.DynamicProperties.Services;
using Core.Exceptions;
using Core.ObjectMapping;
using Core.SiteSettings;
using Core.Templating.Services;
using Membership.Core;
using Membership.Core.Contracts;
using Membership.Core.Entities;
//using NotifyService.RestClient.Services;
using ProductManagement.Core.Dto;
using ProductManagement.Core.EmailTemplates;
using ProductManagement.Core.Entities;
using ProductManagement.Core.Models;
using ProductManagement.Core.Repositories;
using ProductManagement.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductManagement.Services
{
    public class VendorService : BaseService<Vendor, VendorDto>, IVendorService
	{
		private readonly IDynamicPropertyValueService _dynamicPropertyValueService;
		private readonly IMappingService _mappingService;
		private readonly IMembership _membership;
		private readonly IUserService _userService;
	    private readonly IPropertyService _propertyService;
	    private readonly ISiteSettingService  _siteSettingService;
	    private readonly ITemplateService     _templateService;
	    //private readonly INotificationService _notificationService;
        public VendorService(IMappingService mappingService, IRepository repository, IDynamicPropertyValueService dynamicPropertyValueService, IMembership membership, IUserService userService, IPropertyService propertyService, ISiteSettingService siteSettingService, ITemplateService templateService/*, INotificationService notificationService*/) : base(mappingService, repository)
		{
			_mappingService = mappingService;
			_dynamicPropertyValueService = dynamicPropertyValueService;
			_membership = membership;
			_userService = userService;
		    _propertyService = propertyService;
		    _siteSettingService = siteSettingService;
		    _templateService = templateService;
		    //_notificationService = notificationService;
		}

		public override IQueryable<Vendor> GetAll()
		{
			return base.GetAll().Where(x => !x.IsDeleted);
		}

		public VendorProfileModel GetVendorByUserId(long userId)
		{
			var profile = TryGetVendorProfile(userId);

			var profileModel = new VendorProfileModel
			{
				Id = profile.Id,
				ProfilePhotoUrl = profile.ProfilePhotoUrl,
				ContactPhoneNumber = profile.ContactPhoneNumber,
				PhoneNumber = profile.ContactPhoneNumber,
				Address = profile.Address,
				Description = profile.Description,
				City = profile.City,
				Country = profile.Country,
				ZipCode = profile.ZipCode,
				State = profile.State,
				LocationId = profile.LocationId,
				ServiceProvidings = profile.ServiceProvidings.Select(r => new ServiceProvidingDto
				{
					Id = r.Id,
					Name = r.Name
				}).ToList()

			};

			profileModel.ExtendedProperties = _dynamicPropertyValueService.GetExtendedPropertyValuesForEntity<Vendor>(profileModel.Id);

			return profileModel;
		}

		private Vendor TryGetVendorProfile(long userId)
		{
			var user = _userService.GetUser(userId);
			if (user == null || user.IsDeleted)
				throw new BaseNotFoundException<User>();

			var profile = Repository.Get<Vendor>(userProfile => userProfile.UserId == userId);

			if (profile != null) return profile;

			profile = new Vendor
			{
				UserId = userId,
				CreatedDate = DateTimeOffset.Now,
				CreatedBy = _membership.Name,
				ModifiedDate = DateTimeOffset.Now,
				ModifiedBy = _membership.Name,
				ServiceProvidings = new List<ServiceProviding>()
			};
			Repository.Insert(profile);
			Repository.SaveChanges();

			// reload profile
			return Repository.Get<Vendor>(userProfile => userProfile.UserId == userId);
		}

		public List<ServiceProviding> GetServiceProvidings()
		{
			return Repository.GetAll<ServiceProviding>().ToList();
		}

		/// <summary>
		/// Update Profile Vendor
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="model"></param>
		public void UpdateVendorProfile(long userId, VendorProfileModel model)
		{
			var user = _userService.GetUser(userId);
			if (user == null || user.IsDeleted)
			{
				throw new BaseNotFoundException<User>();
			}

			// 1. Update vendor Profile
			var vendorUpdate = TryGetVendorProfile(userId);
			vendorUpdate.ProfilePhotoUrl = model.ProfilePhotoUrl;
			vendorUpdate.ContactPhoneNumber = model.PhoneNumber;
			vendorUpdate.Description = model.Description;
			vendorUpdate.Address = model.Address;
			vendorUpdate.City = model.City;
			vendorUpdate.Country = model.Country;
			vendorUpdate.State = model.State;
			vendorUpdate.ZipCode = model.ZipCode;
			vendorUpdate.LocationId = model.LocationId;

			// 2. Update ProvidingService - Vendor
			UpdateProviding(vendorUpdate, model);

		    _dynamicPropertyValueService.UpdateEntityAdditionalFields<Vendor>(vendorUpdate.Id, model.ExtendedProperties);

            Repository.Update(vendorUpdate);
			Repository.SaveChanges();
		}

		private void UpdateProviding(Vendor vendor, VendorProfileModel model)
		{
			// 1. new Service Providings
			var serviceProvidingIds = model.ServiceProvidings.Select(r => r.Id).ToList();

			// 2. Delete all ServiceProvidings that are not existed in new Service Providings
			var deleteServiceProvidings = vendor.ServiceProvidings.Where(r => !serviceProvidingIds.Contains(r.Id)).ToList();
			if (deleteServiceProvidings.Any())
			{
				foreach (var item in deleteServiceProvidings)
				{
					vendor.ServiceProvidings.Remove(item);
				}
			}

			// 3. Add ServiceProviding
			foreach (var item in model.ServiceProvidings)
			{
				var permision = Repository.Get<ServiceProviding>(item.Id);
				vendor.ServiceProvidings.Add(permision);
			}
		}

        /// <summary>
        /// Search Vendor
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
	    public VendorSearchResponseModel VendorSearch(VendorSearchRequestModel model)
        {
            var vendors = Repository
                          .Fetch<Vendor>(v => v.ServiceProvidings.Any(r => model.ProvidingSeviceIds.Count == 0 ||
                                                                           model.ProvidingSeviceIds.Contains(r.Id))
                                         && !v.IsDeleted
                                         && (!model.LocationIds.Any() 
                                             || v.LocationId.HasValue && model.LocationIds.Contains(v.LocationId.Value)))
                          .Select(r => new VendorItemModel
                          {
                              Id = r.Id,
                              UserId = r.UserId,
                              ProfilePhotoUrl = r.ProfilePhotoUrl,
                              ContactPhoneNumber = r.ContactPhoneNumber,
                              PhoneNumber = r.ContactPhoneNumber,
                              Address = r.Address,
                              Description = r.Description,
                              City = r.City,
                              Country = r.Country,
                              ZipCode = r.ZipCode,
                              State = r.State,
                              LocationId = r.LocationId,
                              ServiceProvidings = r.ServiceProvidings.Select(c => new ServiceProvidingDto
                              {
                                  Id = c.Id,
                                  Name = c.Name
                              }).ToList()
                          }).ToList();

            vendors.ForEach(r =>
            {
                var user = _userService.GetUser(r.UserId);

                r.UserInformation = new UserInformation
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email
                };
                
                r.ExtendedProperties = _dynamicPropertyValueService.GetExtendedPropertyValuesForEntity<Vendor>(r.Id);
            });

            var results = new VendorSearchResponseModel {Vendors = vendors};
            return results;
	    }

        /// <summary>
        /// Vendor Request from Landlord or Tenant
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
	    public bool VendorRequest(long currentUserId, VendorRequestModel model)
	    {
	        try
	        {
	            var fromUser = _userService.GetUser(currentUserId);
	            if (fromUser == null || fromUser.IsDeleted)
	            {
	                throw new BaseNotFoundException<User>();
	            }

	            foreach (var propetyId in model.Properties)
	            {
	                if (!Repository.Any<VendorRequest>(r => r.FromUserId == fromUser.Id && r.ToUserId == model.ToUserId && r.PropertyId == propetyId))
	                {
	                    var vendorRequest = new VendorRequest
	                                        {
	                                            FromUserId      = fromUser.Id,
	                                            ToUserId        = model.ToUserId, // this is UserId of Vendor
	                                            IsAccepted      = false,
	                                            PropertyId      = propetyId,
	                                            RequestSentDate = DateTimeOffset.Now
	                                        };
	                    Repository.Insert(vendorRequest);
                    }
                }

	            Repository.SaveChanges();

                // Send Email
	            SendEmailToVendor(fromUser, model.ToUserId);
                return true;
	        }
	        catch (Exception ex)
	        {
                return false;
	        }
	    }

	    private void SendEmailToVendor(User fromLandlord, long vendorId)
	    {
	        var generalSetting = _siteSettingService.GetSetting<GeneralSiteSetting>();
	        var adminEmail     = generalSetting.AdminContactEmails.FirstOrDefault(x => x.IsDefaultToSendNotification);
	        if (adminEmail == null)
	        {
	            return;
	        }

	        var model = new SendPropertyToVendorTemplate
	        {
	            LandlordName = $"{fromLandlord.FirstName} {fromLandlord.LastName}"
            };
	        var toVendor = _userService.GetUser(vendorId);
            var template = _templateService.ParseTemplate(model);

	        var title   = template.TemplateTitle;
	        var content = template.TemplateContent;
	        /*_notificationService.SendEmail(new[] { toVendor.Email }, title, content, adminEmail.EmailAddress,
	                                       adminEmail.DisplayName);*/
	    }

        /// <summary>
        /// Get My Vendor Request from Tenant or Landlord
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
	    public List<VendorRequestItemModel> GetMyVendorRequests(long currentUserId)
        {
            var vendorRequests = Repository.Fetch<VendorRequest>(r => r.FromUserId == currentUserId)
                                          .Select(r => new VendorRequestItemModel
                                          {
                                            Id = r.Id,
                                            ToUserId = r.ToUserId,
                                            IsAccepted = r.IsAccepted,
                                            PropertyId = r.PropertyId,
                                            RequestAcceptedDate = r.RequestAcceptedDate,
                                            RequestSentDate = r.RequestSentDate
                                          }).ToList();

            vendorRequests.ForEach(r =>
            {
                var vendor = Repository.Get<Vendor>(c => c.UserId == r.ToUserId);
                var user = _userService.GetUser(r.ToUserId);
                var property = Repository.Get<Property>(c => c.Id == r.PropertyId);
                if (vendor != null)
                {
                    r.Property = new PropertyDto { Id = r.PropertyId, Name = property.Name};
                    r.VendorInformation = new VendorItemModel
                    {
                        Address = vendor.Address,
                        City = vendor.City,
                        PhoneNumber = vendor.ContactPhoneNumber,
                        Description = vendor.Description,
                        UserInformation = new UserInformation
                        {
                            Id = r.ToUserId,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email
                        }
                    };
                }
            });

            return vendorRequests;
	    }
        /// <summary>
        /// Get My Vendor Request from Vendor (all requests from tenants or landlords)
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
	    public List<VendorRequestItemModel> GetRequestForVendor(long currentUserId)
        {
            var vendorRequests = Repository.Fetch<VendorRequest>(r => r.ToUserId == currentUserId)
                                          .Select(r => new VendorRequestItemModel
                                          {
                                              Id = r.Id,
                                              ToUserId = r.ToUserId,
                                              IsAccepted = r.IsAccepted,
                                              PropertyId = r.PropertyId,
                                              RequestAcceptedDate = r.RequestAcceptedDate,
                                              RequestSentDate = r.RequestSentDate
                                          }).ToList();

            vendorRequests.ForEach(r =>
            {
                var user = _userService.GetUser(r.ToUserId);
                var property = Repository.Get<Property>(c => c.Id == r.PropertyId);
                r.Property = _propertyService.ToDto(property);
                r.UserInformation = new UserInformation
                {
                    Id        = r.ToUserId,
                    FirstName = user.FirstName,
                    LastName  = user.LastName,
                    Email     = user.Email
                };
            });

            return vendorRequests;
        }

        /// <summary>
        /// Accept Request
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="vendorUserId"></param>
        /// <returns></returns>
	    public bool AcceptVendorRequest(long requestId, long vendorUserId)
	    {
	        try
	        {
	            var vendorRequest = Repository.Get<VendorRequest>(r => r.Id == requestId && r.ToUserId == vendorUserId);
	            if (vendorRequest != null)
	            {
	                vendorRequest.IsAccepted = true;
                    vendorRequest.RequestAcceptedDate = DateTimeOffset.Now;

	                Repository.Update(vendorRequest);
                    Repository.SaveChanges();

                    // Send Email
	                SendEmailToLandlord(vendorRequest.FromUserId, vendorUserId);

                    return true;
	            }

	            return false;
	        }
	        catch (Exception ex)
	        {
	            return false;
	        }
	    }

	    private void SendEmailToLandlord(long landordUserId, long vendorId)
	    {
	        var generalSetting = _siteSettingService.GetSetting<GeneralSiteSetting>();
	        var adminEmail     = generalSetting.AdminContactEmails.FirstOrDefault(x => x.IsDefaultToSendNotification);
	        if (adminEmail == null)
	        {
	            return;
	        }

	        var landlord = _userService.GetUser(landordUserId);
	        var vendor = _userService.GetUser(vendorId);

            var model = new VendorApproveProperty
	        {
	            UserName = $"{landlord.FirstName} {landlord.LastName}",
                VendorName = $"{vendor.FirstName} {vendor.LastName}"
            };

	        var template = _templateService.ParseTemplate(model);

	        var title   = template.TemplateTitle;
	        var content = template.TemplateContent;
	        /*_notificationService.SendEmail(new[] { landlord.Email }, title, content, adminEmail.EmailAddress,
	                                       adminEmail.DisplayName);*/
	    }
    }
}