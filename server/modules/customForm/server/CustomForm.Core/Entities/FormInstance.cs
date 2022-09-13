using Core.Database.Entities;
using Core.DynamicProperties.Models;
using System.Collections.Generic;

namespace CustomForm.Core.Entities
{
    public class FormInstance : BaseEntity, IHasDynamicProperty
    {
        public FormInstance()
        {
            Answers = new List<FormFieldValue>();
        }

        public long UserId { get; set; }

        public long FormConfigurationId { get; set; }

        public virtual FormConfiguration FormConfiguration { get; set; }
        
        public virtual ICollection<FormFieldValue> Answers { get; set; }
    }
}