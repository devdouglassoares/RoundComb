using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Common.Core.Entities
{
    public class ExceptionLogger : ExceptionLoggerBase
    {
        [Column("Data")]
        public string DataString
        {
            get
            {
                return JsonConvert.SerializeObject(Data);
            }
            set
            {
                Data = JObject.Parse(value);
            }
        }
    }
}