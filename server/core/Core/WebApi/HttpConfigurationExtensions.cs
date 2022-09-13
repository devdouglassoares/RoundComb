using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Web.Http;

namespace Core.WebApi
{
    public static class HttpConfigurationExtensions
    {
        public static void AcceptJsonFormatterOnly(this HttpConfiguration config)
        {
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new StringEnumConverter());
        }
    }
}