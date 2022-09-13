using Core.UploadService;
using Core.UploadService.FileSystem;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Upload.Api.Models;

namespace Upload.Api.Controllers
{
	public class UploadController : ApiController
	{
		private readonly IUploadService _uploadService;

		public UploadController(IUploadService uploadService)
		{
			_uploadService = uploadService;
		}

		[HttpPost, Route("api/upload")]
		public async Task<HttpResponseMessage> UploadAction()
		{
			var provider = new MultipartMemoryStreamProvider();
			await Request.Content.ReadAsMultipartAsync(provider);

			var content = provider.Contents.FirstOrDefault();

			if (content == null)
				throw new Exception();

			var filename = content.Headers.ContentDisposition.FileName.Trim('\"');

			var stream = await content.ReadAsStreamAsync();

			string origin = null;

			if (Request.Headers.Contains("Origin"))
			{
				origin = Request.Headers.GetValues("Origin").FirstOrDefault();
			}

			var baseUrl = new Uri(origin ?? Request.RequestUri.AbsoluteUri).Host;

			var uploadResult = _uploadService.UploadFile(baseUrl, filename, stream);

			uploadResult.FileUrl = ResolveApplicationUri(uploadResult.FileUrl, baseUrl);

			uploadResult.AbsoluteUrl = ResolveApplicationUri(uploadResult.FileUrl, baseUrl, true);

			return Request.CreateResponse(HttpStatusCode.OK, uploadResult);
		}

		[HttpPost, Route("api/uploadDataUri")]
		public HttpResponseMessage UploadBase64String(ImageUploadModel model)
		{
			var origin = Request.Headers.GetValues("Origin").FirstOrDefault();

			var baseUrl = new Uri(origin ?? Request.RequestUri.AbsoluteUri).Host;

			var stream = ImageProcessingService.GetImageStreamFromBase64String(model.Base64String, FileInfoExtensions.GetImageFormatFromName(model.FileName));

			var uploadResult = _uploadService.UploadFile(baseUrl, model.FileName, stream);

			uploadResult.FileUrl = ResolveApplicationUri(uploadResult.FileUrl, baseUrl);

			uploadResult.AbsoluteUrl = ResolveApplicationUri(uploadResult.FileUrl, baseUrl, true);

			return Request.CreateResponse(HttpStatusCode.OK, uploadResult);
		}


		public string ResolveApplicationUri(string relativeUri, string originHost, bool getAbsoluteUrlOnly = false)
		{
			if (!getAbsoluteUrlOnly)
			{
				var requestUrlHost = new Uri(Request.RequestUri.AbsoluteUri).Host;

				if (requestUrlHost == originHost)
					return relativeUri;
			}

			var baseUri = new Uri(Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.PathAndQuery, String.Empty));

			return new Uri(baseUri, VirtualPathUtility.ToAbsolute(relativeUri)).ToString();
		}
	}
}
