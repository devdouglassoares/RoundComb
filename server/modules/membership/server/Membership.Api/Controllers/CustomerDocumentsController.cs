using Core.UI;
using Core.UploadService;
using Core.UploadService.FileSystem;
using DocumentsManagement.Library.Controllers;
using Membership.Library.Contracts;
using Membership.Library.Dto.Customer;
using Membership.Library.Entities;
using System.Web.Http;
using Membership.Core.Contracts.AuthAttributes;

namespace Membership.Api.Controllers
{
    [RoutePrefix("customerDocuments")]
    [RequireAuthTokenApi]
    public class CustomerDocumentsController : BaseDocumentsController<CompanyDocumentDto, CompanyDocument>
    {
        public CustomerDocumentsController(ICustomerDocumentService documentsService,
                                           IDataTableService dataTableService,
                                           IStorageProvider storageProvider,
                                           IUploadService uploadService)
            : base(documentsService, dataTableService, storageProvider, uploadService)
        {
        }
    }
}