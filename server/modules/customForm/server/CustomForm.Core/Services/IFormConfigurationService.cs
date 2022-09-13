using Core;
using Core.Database;
using CustomForm.Core.Dtos;
using CustomForm.Core.Entities;

namespace CustomForm.Core.Services
{
    public interface IFormConfigurationService : IBaseService<FormConfiguration, FormConfigurationDto>, IDependency
    {
    }
}