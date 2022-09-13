using Membership.Core;
using ProductManagement.Core.Entities;
using ProductManagement.Core.Services;
using Roundcomb.Core.Entities;
using Roundcomb.Core.Repositories;
using Roundcomb.Core.Services;
using System.Linq;

namespace Roundcomb.Services
{
    public class PropertyVendorService : IPropertyVendorService
    {
        private readonly IPropertyService _propertyService;
        private readonly IRoundcombRepository _repository;
        public PropertyVendorService(IPropertyService propertyService, IRoundcombRepository repository)
        {
            _propertyService = propertyService;
            _repository = repository;
        }

        public IQueryable<Property> GetMyPropertiesForVendor(IMembership membership)
        {
            IQueryable<Property> properties = _propertyService.Fetch(x => x.ParentPropertyId == null);

            if (membership.IsCurrentUserInRole("BizLandlord"))
            {
                properties = properties.Where(p => p.OwnerId == membership.UserId);
            }
            else if (membership.IsCurrentUserInRole("Tenants"))
            {
                // Get Properties from Tenants
                var propertiesApproved = _repository
                                         .Fetch<PropertyApplicationFormInstance>(r => r.UserId == membership.UserId
                                                                                      && r.IsApproved
                                                                                      && !r.IsDeleted
                                                                                      && r.UserAccepted)
                                         .Select(r => r.PropertyId).ToList();
                var ids = properties.Select(r => r.Id).ToList();
                properties = properties.Where(r => propertiesApproved.Contains(r.Id));
            }
            else
            {
                properties = null;
            }

            return properties;
        }
    }
}
