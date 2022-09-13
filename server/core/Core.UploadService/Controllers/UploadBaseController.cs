using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Core.UploadService.FileSystem;
using Newtonsoft.Json;

namespace Core.UploadService.Controllers
{
    public abstract class UploadFileController : ApiController
    {
        private readonly IStorageProvider _storageProvider;
        private readonly IUploadService _uploadService;

        protected UploadFileController(IStorageProvider storageProvider, IUploadService uploadService)
        {
            _storageProvider = storageProvider;
            _uploadService = uploadService;
        }

        protected async Task<UploadResult> Upload()
        {
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count == 0)
                throw new InvalidOperationException("Cannot find file to upload");

            var tempFolderPath = ".Temp";
            var tempFolder = _storageProvider.MapPath(tempFolderPath);
            var provider = new MultipartFormDataStreamProvider(tempFolder);
            var result = await Request.Content.ReadAsMultipartAsync(provider);
            var originalFileName = GetDeserializedFileName(result.FileData.First());

            var uploadFolderPath = GetUploadFolderPath(result);
            var fileNameToSave = GetFileNameToSave(result);

            var postedFile = httpRequest.Files[0];
            return _uploadService.UploadFile(uploadFolderPath, fileNameToSave ?? originalFileName, postedFile.InputStream);
        }

        protected abstract string GetFileNameToSave(MultipartFormDataStreamProvider result);

        protected abstract string GetUploadFolderPath(MultipartFormDataStreamProvider result);

        protected T GetFormData<T>(MultipartFormDataStreamProvider result)
        {
            if (result.FormData.HasKeys())
            {
                var unescapedFormData = Uri.UnescapeDataString(result.FormData.ToString());
                var dict = HttpUtility.ParseQueryString(unescapedFormData);

                var json = JsonConvert.SerializeObject(dict.Keys.Cast<string>().ToDictionary(k => k, k => dict[k]));
                if (!string.IsNullOrEmpty(json))
                    return JsonConvert.DeserializeObject<T>(json);
            }

            return default(T);
        }

        private string GetDeserializedFileName(MultipartFileData fileData)
        {
            var fileName = GetFileName(fileData);
            return JsonConvert.DeserializeObject(fileName).ToString();
        }

        private static string GetFileName(MultipartFileData fileData)
        {
            return fileData.Headers.ContentDisposition.FileName;
        }
    }
}