using Core.Database.Entities;
using System.Collections.Generic;

namespace CustomForm.Core.Entities
{
    public class FormConfiguration : BaseEntity
    {
        public FormConfiguration()
        {
            Fields = new List<FormFieldFormConfiguration>();
        }

        public string FormName { get; set; }

        public string FormCode { get; set; }

        public long? OwnerId { get; set; }

        public bool IsSystemConfig { get; set; }

        public virtual ICollection<FormFieldFormConfiguration> Fields { get; set; }

        public virtual ICollection<FormDynamicPropertyConfig> DynanicFields { get; set; }
    }
}