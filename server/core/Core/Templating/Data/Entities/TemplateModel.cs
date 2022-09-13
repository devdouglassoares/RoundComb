using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Core.Database.Entities;

namespace Core.Templating.Data.Entities
{
    public class TemplateModel : BaseEntity
    {
        [Column("TemplateType")]
        public string TemplateTypeString { get; set; }

        public string TemplateTitle { get; set; }

        public string TemplateContent { get; set; }

        [NotMapped]
        public List<string> Fields
        {
            get { return FieldsString.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList(); }
            set { FieldsString = string.Join(";", value); }
        }

        [Column("Fields")]
        public string FieldsString { get; set; }

        public string FromAssemblyName { get; set; }
    }
}