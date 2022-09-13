using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Core.UI.DataTablesExtensions
{
    [DataContract, Serializable]
    public class OrderColumnRequest
    {
        [DataMember(Name = "order")]
        public ICollection<OrderRequest> Order { get; set; }
    }

    [DataContract, Serializable]
    public class OrderRequest
    {
        [DataMember(Name = "column")]
        public int ColumnNumber { get; set; }

        [DataMember(Name = "dir")]
        public string Direction { get; set; }
    }
}