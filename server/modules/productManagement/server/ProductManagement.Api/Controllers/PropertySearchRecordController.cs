using Core.WebApi.Controllers;
using ProductManagement.Core.Dto;
using ProductManagement.Core.Entities;
using ProductManagement.Core.Services;
using Membership.Core.Contracts.AuthAttributes;
using System.Web.Http;

namespace ProductManagement.Api.Controllers
{
    [RoutePrefix("api/propertySearchRecord")]
    [RequireAuthTokenApi]
    public class PropertySearchRecordController : BaseCrudController<PropertySearchRecord, PropertySearchRecordDto, IPropertySearchRecordService>
    {
        public PropertySearchRecordController(IPropertySearchRecordService crudService) : base(crudService)
        {
        }
    }
}