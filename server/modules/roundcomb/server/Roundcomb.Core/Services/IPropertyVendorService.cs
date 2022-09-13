using Core;
using ProductManagement.Core.Entities;
using Membership.Core;
using System.Linq;

namespace Roundcomb.Core.Services
{
    public interface IPropertyVendorService : IDependency
    {
        IQueryable<Property> GetMyPropertiesForVendor(IMembership membership);
    }
}
