using Core;
using Core.Database;
using CustomForm.Core.Dtos;
using CustomForm.Core.Entities;

namespace CustomForm.Core.Services
{
    public interface IFormFieldService : IBaseService<FormField, FormFieldDto>, IDependency
    {

    }
}