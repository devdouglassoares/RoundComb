using System;

namespace Core.Logging.Models
{
    public class LogEntry
    {
        public long Id { get; set; }

        public DateTimeOffset? Date { get; set; }

        public string Thread { get; set; }

        public string Level { get; set; }

        public string Logger { get; set; }

        public string Message { get; set; }

        public string Exception { get; set; }
    }

    public class LogEntryModel
    {
        public long Id { get; set; }

        public DateTimeOffset? Date { get; set; }

        public string Thread { get; set; }

        public string Level { get; set; }

        public string Logger { get; set; }

        public string Message { get; set; }
    }
}
