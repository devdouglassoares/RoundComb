using System.Collections.Generic;

namespace CustomForm.Core.Dtos
{
    public class FormConfigurationDto : FormConfigurationNoField
    {
        public virtual ICollection<FormFieldFormConfigurationDto> Fields { get; set; }
    }


}