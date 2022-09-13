using Core.UI.DataTablesExtensions;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

namespace LocationService.Library.Models
{
    public class ProductLocationQueryRequestModelBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            var context = new HttpContextWrapper(HttpContext.Current);
            var request = context.Request;

            var model = new LocationQueryRequest();

            NameValueCollection requestParameters = ResolveNameValueCollection(request);

            if (!requestParameters.AllKeys.Contains("location")) return false;

            var strings = requestParameters.G<string>("location");

            if (string.IsNullOrEmpty(strings)) return false;

            model = JsonConvert.DeserializeObject<LocationQueryRequest>(strings);

            bindingContext.Model = model;
            return true;
        }

        protected virtual NameValueCollection ResolveNameValueCollection(HttpRequestBase request)
        {
            if (request.HttpMethod.ToLower().Equals("get")) return request.QueryString;
            if (request.HttpMethod.ToLower().Equals("post")) return request.Form;
            throw new ArgumentException(String.Format("The provided HTTP method ({0}) is not a valid method to use with DataTablesBinder. Please, use HTTP GET or POST methods only.", request.HttpMethod), "method");
        }
    }
}