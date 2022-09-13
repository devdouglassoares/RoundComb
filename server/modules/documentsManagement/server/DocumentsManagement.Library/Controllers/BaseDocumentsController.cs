using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Core.Database.Repositories;
using Core.UI;
using Core.UI.DataTablesExtensions;
using Core.UploadService;
using Core.UploadService.FileSystem;
using Core.WebApi.ActionFilters;
using DocumentsManagement.Library.Dtos;
using DocumentsManagement.Library.Entities;
using DocumentsManagement.Library.Services;
using Newtonsoft.Json;

namespace DocumentsManagement.Library.Controllers
{
    public class BaseDocumentsController<TDto, TEntity> : ApiController where TDto : BaseDocumentDto, new() where TEntity : BaseDocumentEntity
    {
        
        private readonly IBaseDocumentsService<TDto, TEntity> documentsService;
        private readonly IDataTableService dataTableService;
        private readonly IStorageProvider storageProvider;
        private readonly IUploadService uploadService;

        public BaseDocumentsController(IBaseDocumentsService<TDto, TEntity> documentsService,
                                       IDataTableService dataTableService,
                                       IStorageProvider storageProvider,
                                       IUploadService uploadService)
        {
            this.documentsService = documentsService;
            this.dataTableService = dataTableService;
            this.storageProvider = storageProvider;
            this.uploadService = uploadService;
        }


        [HttpPost, Route("{masterId:int}/datatables")]
        public HttpResponseMessage DataTables(long masterId, [ModelBinder(typeof(DataTableModelBinderProvider))]DefaultDataTablesRequest requestModel)
        {
            var result = documentsService.GetAll(masterId);

            var response = this.dataTableService.GetResponse(result, requestModel);

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpGet, Route("")]
        public HttpResponseMessage GetAll(long masterId)
        {
            var result = documentsService.GetAll(masterId);

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpGet]
        public HttpResponseMessage Details(long id)
        {
            var result = documentsService.Get(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost, Route("create")]
        public IHttpActionResult Create(TDto model)
        {
            return Ok(documentsService.Create(model));
        }


        [HttpPut]
        public IHttpActionResult Update(long id, TDto model)
        {
            documentsService.Update(id, model);
            return Ok();
        }

        [HttpDelete, Route("{id:int}")]
        public IHttpActionResult Delete(long id)
        {
            documentsService.Delete(id);
            return Ok();
        }

        [HttpGet]
        [ErrorResponseHandler(typeof(FileNotFoundException), HttpStatusCode.NotFound)]
        public HttpResponseMessage Download(long id)
        {
            var subcontractorDocument = documentsService.Get(id);

            var fileUrl = subcontractorDocument.FileUrl;

            var filePath = storageProvider.GetStoragePath(fileUrl);

            var file = storageProvider.GetFile(filePath);

            var result = new HttpResponseMessage(HttpStatusCode.OK);

            result.Content = new StreamContent(file.OpenRead());
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = subcontractorDocument.FileName
            };

            return result;
        }

        [HttpPost, Route("upload")]
        public async Task<HttpResponseMessage> Upload()
        {
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count == 0)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No file available in upload request");

            var tempFolderPath = ".Temp" + await GenerateRandomString();
            var tempFolder = storageProvider.MapPath(tempFolderPath);
            var provider = new MultipartFormDataStreamProvider(tempFolder);
            var result = await Request.Content.ReadAsMultipartAsync(provider);
            var originalFileName = GetDeserializedFileName(result.FileData.First());
            var fileUploadObj = GetFormData<UploadControlRequestDto>(result);

            var module = typeof (TEntity).Assembly.GetName().Name.Split('.').First();
            var uploadFolderPath = fileUploadObj == null
                ? "Uploaded"
                : module + "/Entity_" + fileUploadObj.Identifier;

            var postedFile = httpRequest.Files[0];
            var uploadResult = uploadService.UploadFile(uploadFolderPath, fileUploadObj != null ? fileUploadObj.FileName : originalFileName, postedFile.InputStream);

            var fileRecord = documentsService.SaveFileRecord(uploadResult.FileName, uploadResult.FileUrl);

            try
            {
                storageProvider.DeleteFolder(tempFolderPath);
            }
            catch
            {
            }

            return Request.CreateResponse(HttpStatusCode.OK,
                                          new
                                          {
                                              success = true,
                                              fileRecord
                                          });
        }

        private T GetFormData<T>(MultipartFormDataStreamProvider result)
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

        private async Task<string> GenerateRandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            var result = await Task.Run(() =>
            {
                var random = new Random();
                var stringLength = random.Next(7, random.Next(170));

                return new string(
                    Enumerable.Repeat(chars, stringLength)
                        .Select(s => s[random.Next(s.Length)])
                        .ToArray());
            });

            return result;
        }

        private string GetDeserializedFileName(MultipartFileData fileData)
        {
            var fileName = GetFileName(fileData);
            return JsonConvert.DeserializeObject(fileName).ToString();
        }

        private string GetFileName(MultipartFileData fileData)
        {
            return fileData.Headers.ContentDisposition.FileName;
        }
    }
}