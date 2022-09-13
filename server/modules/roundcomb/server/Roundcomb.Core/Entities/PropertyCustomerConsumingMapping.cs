using ProductManagement.Core.Entities;
using Roundcomb.Core.Base;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Roundcomb.Core.Entities
{
    public class PropertyCustomerConsumingMapping : PropertyCustomerConsumingMappingBase
    {
        [Column("Assignment")]
        public string ProductAssignment
        {
            get { return Assignment.ToString(); }
            set { Assignment = (PropertyAssignment)Enum.Parse(typeof(PropertyAssignment), value, true); }
        }

        public virtual Property Property { get; set; }

        public virtual PropertyApplicationFormInstance PropertyApplicationFormInstance { get; set; }
    }
}