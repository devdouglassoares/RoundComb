using Core;
using Core.Database;
using Core.DynamicProperties.Dtos;
using CustomForm.Core.Dtos;
using CustomForm.Core.Entities;
using System.Collections.Generic;

namespace CustomForm.Core.Services
{
    public interface IFormInstanceService : IBaseService<FormInstance, FormInstanceDto>, IDependency
    {
        IEnumerable<FormInstance> QueryFormByCode(string formCode, Dictionary<long, DynamicPropertyFilterModel> dynamicPropertyFilters);
    }
}