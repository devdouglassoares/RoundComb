using System;
using System.Collections.Generic;

namespace Core.Templating.Models
{
    public class TemplateModelDto
    {
        public string TemplateTypeString { get; set; }

        public string TemplateContent { get; set; }

        public string TemplateTitle { get; set; }


        public List<string> Fields { get; set; }
    }
}