using Core.Database;
using Core.ObjectMapping;
using ProductManagement.Core.Dto;
using ProductManagement.Core.Entities;
using ProductManagement.Core.Repositories;
using ProductManagement.Core.Services;
using Membership.Core;
using System.Linq;

namespace ProductManagement.Services
{
    public class PropertySearchRecordService : BaseService<PropertySearchRecord, PropertySearchRecordDto>, IPropertySearchRecordService
    {
        private readonly IMembership _membership;

        public PropertySearchRecordService(IMappingService mappingService,
                                          IRepository repository,
                                          IMembership membership)
            : base(mappingService, repository)
        {
            _membership = membership;
        }

        public override IQueryable<PropertySearchRecord> GetAll()
        {
            var propertySearchRecords = Fetch(x => x.UserId == _membership.UserId && !x.IsDeleted);

            return propertySearchRecords;
        }

        public override PropertySearchRecord PrepareForInserting(PropertySearchRecord entity, PropertySearchRecordDto model)
        {
            entity = base.PrepareForInserting(entity, model);

            entity.UserId = _membership.UserId;

            return entity;
        }
    }
}