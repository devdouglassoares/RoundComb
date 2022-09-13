using Core.Templating.Services;
using System;

namespace Core.Templating.Models
{
    public abstract class BaseTemplateModel : ITemplateModel
    {
        public string SiteUrl { get; set; }

        public string Today { get { return DateTime.Today.ToString("MM/dd/yyyy"); } }

        public string CurrentDateTime { get { return DateTimeOffset.Now.ToString("MM/dd/yyyy hh:mm:ss tt, ddd"); } }
    }
}
