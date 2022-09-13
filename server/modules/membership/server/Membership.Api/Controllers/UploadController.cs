﻿using RestSharp;
using RestSharp.Extensions;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Http;

namespace Membership.Api.Controllers
{
    public class UploadController : ApiController
    {
        [HttpPost, Route("api/uploadphoto")]
        public async Task<HttpResponseMessage> UploadProfilePhoto()
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

            var origin = Request.Headers.GetValues("Origin").FirstOrDefault();

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

        [HttpPost, Route("image/upload")]
        public async Task<IHttpActionResult> Upload()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new Exception();
            }

            var uploadsPath = string.Format("{0}{1}", HostingEnvironment.MapPath("~/"), "Images");
            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }

            var provider = new MultipartFormDataStreamProvider(uploadsPath);

            await Request.Content.ReadAsMultipartAsync(provider);

            foreach (MultipartFileData file in provider.FileData)
            {
                var uploadedFile = new FileInfo(file.LocalFileName);

                var ext = file.Headers.ContentDisposition.FileName.Trim('\"').Split('.').Last();
                var filename = Guid.NewGuid() + "." + ext;

                var destFile = new FileInfo(uploadsPath + "\\" + filename);
                if (destFile.Exists)
                {
                    destFile.Delete();
                }

                uploadedFile.MoveTo(uploadsPath + "\\" + filename);

                return Ok(new
                {
                    uploaded = true,
                    fileName = filename,
                    //url = Request.RequestUri.Scheme + "://" + Request.RequestUri.Host + ":" + Request.RequestUri.Port +
                    //"/" + Request.RequestUri.AbsolutePath.Trim('/').Split('/').First() + "/image/download?id=" + filename,
                    url = new Uri(Request.RequestUri, "/" + Request.RequestUri.AbsolutePath.Trim('/').Split('/').First() + "/image/download?id=" + filename).ToString()
                });
            }

            return BadRequest();
        }

        [HttpGet, Route("image/download")]
        public HttpResponseMessage Download(string id)
        {
            var uploadsPath = string.Format("{0}{1}", HostingEnvironment.MapPath("~/"), "Images");

            var response = new HttpResponseMessage();

            FileStream stream;

            try
            {
                stream = File.Open(uploadsPath + "\\" + id, FileMode.Open);
            }
            catch (Exception)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.Content = new StringContent("File not found");
                return response;
            }

            response.Content = new StreamContent(stream);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

            return response;
        }
    }
}
