using System.Collections.Generic;

namespace ProductManagement.Core.Models
{
    public class VendorSearchRequestModel
    {
        public List<long> ProvidingSeviceIds { get; set; }
        public List<long> LocationIds { get; set; }
    }
}
