using Core.Templating.Models;
using System;
using System.Collections.Generic;

namespace ProductManagement.Core.EmailTemplates
{
    public class PropertySuggestionEmailTemplate : BaseTemplateModel
    {
        public string RecipientName { get; set; }

        public string SenderName { get; set; }

        public List<SuggestPropertyItem> SuggestPropertyItems { get; set; }

        public DateTimeOffset? SentDate { get; set; }
    }

    public class SuggestPropertyItem
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string ImageUrl { get; set; }
    }
}