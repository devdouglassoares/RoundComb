using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoundComb.Commons.Models
{
    public class ContractTemplateDocument
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string ContractHTMLContent { get; set; }
        public bool isSelected { get; set; }
        
    }
}