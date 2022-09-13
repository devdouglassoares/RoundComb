using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoundComb.Commons.Models
{
    public class SignatureRequestModel
    {
        public string firstsignerRole { get; set; }
        public string firstsignerEmail { get; set; }
        public string firstsignerName { get; set; }
        public string secondsignerRole { get; set; }
        public string secondsignerEmail { get; set; }
        public string secondsignerName { get; set; }
        public string mysignerRole { get; set; }
        public string eventname { get; set; }
        public string Subject { get; set; }
        public string message { get; set; }
        public Guid guidchatroom { get; set; }
        public string templateId { get; set; }
    }
}