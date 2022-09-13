using ProductManagement.Core.Dto;

namespace Roundcomb.Services.Events
{
    public class ApplicationSubmissionValidationFailed
    {
        public PropertyDto Property { get; set; }
    }
}