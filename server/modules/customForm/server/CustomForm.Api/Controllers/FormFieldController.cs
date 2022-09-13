using Core.WebApi.Controllers;
using CustomForm.Core.Dtos;
using CustomForm.Core.Entities;
using CustomForm.Core.Services;
using System.Web.Http;

namespace CustomForm.Api.Controllers
{
    [RoutePrefix("api/formField")]
    public class FormFieldController : BaseCrudController<FormField, FormFieldDto, IFormFieldService>
    {
        
        public FormFieldController(IFormFieldService formFieldService) : base(formFieldService)
        {
        }
    }
}