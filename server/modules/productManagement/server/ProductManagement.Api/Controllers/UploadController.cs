using ProductManagement.Api.Models;
using RestSharp;
using RestSharp.Extensions;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Http;

namespace ProductManagement.Api.Controllers
{
    public class UploadController : ApiController
    {
        [HttpPost, Route("api/uploadphoto")]
        public async Task<HttpResponseMessage> UploadPhoto()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new Exception();
            }

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);

            var file = provider.Contents.FirstOrDefault();

            if (file == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No file found for upload");


            var filename = file.Headers.ContentDisposition.FileName.Trim('\"');

            var stream = await file.ReadAsStreamAsync();

            var origin = Request.Headers.Contains("Origin")
                             ? Request.Headers.GetValues("Origin").FirstOrDefault()
                             : new Uri(Request.RequestUri, "/").ToString();

            var client = new RestClient();
            var uploadServer = WebConfigurationManager.AppSettings["UploadServer"];

            var uploadEndpoint = WebConfigurationManager.AppSettings["UploadResourceEndpoint"];

            if (!uploadServer.StartsWith("http://") && !uploadServer.StartsWith("https://"))
            {
                client.BaseUrl = new Uri(Request.RequestUri, uploadServer + "/" + uploadEndpoint);
                uploadEndpoint = string.Empty;
            }
            else
                client.BaseUrl = new Uri(uploadServer);


            var request = new RestRequest(uploadEndpoint, Method.POST);

            request.AddHeader("Origin", origin);
            request.AddHeader("PrivateKey", WebConfigurationManager.AppSettings["UploadValidationKey"]);

            request.AddFile(filename, stream.ReadAsBytes(), filename);
            var response = client.Execute(request);

            var responseBody = new HttpResponseMessage(response.StatusCode);
            responseBody.Content = new StringContent(response.Content, Encoding.UTF8, response.ContentType.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).First());

            return responseBody;
        }


        [HttpPost, Route("~/api/uploadDataUri")]
        public HttpResponseMessage UploadPhotoUsingBase64([FromBody]ImageUploadModel model)
        {
            var origin = Request.Headers.Contains("Origin")
                             ? Request.Headers.GetValues("Origin").FirstOrDefault()
                             : new Uri(Request.RequestUri, "/").ToString();

            var client = new RestClient();
            var uploadServer = WebConfigurationManager.AppSettings["UploadServer"];

            var uploadEndpoint = "api/uploadDataUri";

            if (!uploadServer.StartsWith("http://") && !uploadServer.StartsWith("https://"))
            {
                client.BaseUrl = new Uri(Request.RequestUri, uploadServer + "/" + uploadEndpoint);
                uploadEndpoint = string.Empty;
            }
            else
                client.BaseUrl = new Uri(uploadServer);

            var request = new RestRequest(uploadEndpoint, Method.POST);

            request.AddHeader("Origin", origin);
            request.AddJsonBody(model);
            var response = client.Execute(request);

            var responseBody = new HttpResponseMessage(response.StatusCode);
            responseBody.Content = new StringContent(response.Content, Encoding.UTF8, "application/json");

            return responseBody;
        }
    }
}