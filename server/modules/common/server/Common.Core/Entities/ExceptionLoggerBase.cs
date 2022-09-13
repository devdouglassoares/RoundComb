using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Core.Entities
{
    public abstract class ExceptionLoggerBase
    {
        public long Id { get; set; }

        public string Message { get; set; }

        public string ExceptionType { get; set; }

        public DateTimeOffset? DateTime { get; set; }

        public DateTimeOffset? Notified { get; set; }

        public string NotifiedTo { get; set; }

        [NotMapped]
        public dynamic Data { get; set; }
    }
}