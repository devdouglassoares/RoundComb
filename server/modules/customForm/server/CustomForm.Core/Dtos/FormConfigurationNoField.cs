using System;

namespace CustomForm.Core.Dtos
{
    public class FormConfigurationNoField
    {
        public long Id { get; set; }
        public string FormCode { get; set; }
        public string FormName { get; set; }
        public long? OwnerId { get; set; }
        public bool IsSystemConfig { get; set; }


        public virtual DateTimeOffset? CreatedDate { get; set; }

        public virtual DateTimeOffset? ModifiedDate { get; set; }
    }
}