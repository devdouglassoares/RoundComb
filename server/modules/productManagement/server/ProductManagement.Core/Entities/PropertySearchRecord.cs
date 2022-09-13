
using Core.Database.Entities;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManagement.Core.Entities
{
    public class PropertySearchRecord : BaseEntity
    {
        public long UserId { get; set; }

        [Column("SearchQuery")]
        public string SearchQueryString { get; set; }

        [NotMapped]
        public Dictionary<string, object> SearchQuery
        {
            get
            {
                if (string.IsNullOrEmpty(SearchQueryString))
                    return new Dictionary<string, object>();

                return JsonConvert.DeserializeObject<Dictionary<string, object>>(SearchQueryString);
            }
            set { SearchQueryString = JsonConvert.SerializeObject(value); }
        }
    }
}