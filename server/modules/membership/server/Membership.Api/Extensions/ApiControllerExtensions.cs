using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Web.Http;
using DTWrapper.Core;
using DTWrapper.Core.DTModel;

namespace Membership.Api.Extensions
{
    public static class ApiControllerExtensions
    {
        public static HttpResponseMessage DataSource<T>(this ApiController controller, FormDataCollection formData,
            IQueryable<T> query, Action<DataTablesConfig<T>> customize = null) where T : class
        {
            DataTablesModel model = DTExtensions.GetModelForApi(formData);

            var dtCfg = new DataTablesConfig<T>(query);

            //Customize DTWrapper configurations (Set Columns)
            if (customize != null)
            {
                customize(dtCfg);
            }

            var dataQuery = dtCfg.GetData(model);
            var jsonResult = dtCfg.BuildResponse(dataQuery.ToList(), model);

            var response = controller.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");

            return response;
        }

        public static HttpResponseMessage DataSource<T>(this ApiController controller, DataTablesModel model,
            IQueryable<T> query, Action<DataTablesConfig<T>> customize = null) where T : class
        {
            //DataTablesModel model = DTExtensions.GetModelForApi(formData);

            var dtCfg = new DataTablesConfig<T>(query);

            //Customize DTWrapper configurations (Set Columns)
            if (customize != null)
            {
                customize(dtCfg);
            }

            var dataQuery = dtCfg.GetData(model);
            var jsonResult = dtCfg.BuildResponse(dataQuery.ToList(), model);

            var response = controller.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");

            return response;
        }

    }
}