using Core.Database;
using Core.ObjectMapping;
using CustomForm.Core.Dtos;
using CustomForm.Core.Entities;
using CustomForm.Core.Services;
using CustomForm.Data.Repositories;

namespace CustomForm.Services
{
    public class FormFieldService :BaseService<FormField, FormFieldDto>, IFormFieldService
    {
        public FormFieldService(IMappingService mappingService, IRepository repository) : base(mappingService, repository)
        {
        }
    }
}