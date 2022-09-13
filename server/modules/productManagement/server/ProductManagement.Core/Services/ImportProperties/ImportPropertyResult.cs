using ProductManagement.Core.Dto;
using System;
using System.Collections.Generic;

namespace ProductManagement.Core.Services.ImportProperties
{
    public class ImportPropertyResult
    {
        public long UploadedItems { get; set; }

        public long ImportedItems { get; set; }

        public int Fails { get; set; }

        public List<ImportPropertyResultDetail> ImportPropertyResultDetails { get; set; }
    }

    public class ImportPropertyResultDetail
    {
        public PropertyDto Property { get; set; }

        public bool IsSuccess { get; set; }

        public string Detail { get; set; }

        public Exception Exception { get; set; }
    }

    public class PropertyImportProgressUpdated
    {
        public string State { get; set; }
        public long UserId { get; set; }
        public ImportPropertyResultDetail LastResult { get; set; }
        public long Success { get; set; }
        public int Failed { get; set; }
        public long Total { get; set; }
    }
}