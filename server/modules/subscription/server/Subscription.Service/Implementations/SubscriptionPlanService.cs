using Core.Database;
using Core.Events;
using Core.ObjectMapping;
using Membership.Core.Data;
using Membership.Core.Entities;
using Subscription.Core.Dtos;
using Subscription.Core.Entities;
using Subscription.Core.Repositories;
using Subscription.Service.Contracts;

namespace Subscription.Service.Implementations
{
    public class SubscriptionPlanService : BaseService<SubscriptionPlan, SubscriptionPlanDto>, ISubscriptionPlanService,
        IConsumer<EntityUpdated<SubscriptionPlan, SubscriptionPlanDto>>
    {
        private readonly ICoreRepository _membershipCoreRepository;

        public SubscriptionPlanService(IMappingService mappingService, IRepository repository, ICoreRepository membershipCoreRepository)
            : base(mappingService, repository)
        {
            _membershipCoreRepository = membershipCoreRepository;
        }

        public override SubscriptionPlan PrepareForInserting(SubscriptionPlan entity, SubscriptionPlanDto model)
        {
            entity = base.PrepareForInserting(entity, model);
            entity.Properties = model.Properties;

            AssignPermissionToSubscription(model, entity);

            return entity;
        }

        public override SubscriptionPlan PrepareForUpdating(SubscriptionPlan entity, SubscriptionPlanDto model)
        {
            entity = base.PrepareForUpdating(entity, model);
            entity.Properties = model.Properties;

            entity.AccessEntities.Clear();

            return entity;
        }

        private void AssignPermissionToSubscription(SubscriptionPlanDto model, SubscriptionPlan entity)
        {
            if (entity.AssignRoleId.HasValue)
                return;

            foreach (var accessEntityId in model.AccessEntityIds)
            {
                var accessEntity = _membershipCoreRepository.Get<AccessEntity>(accessEntityId);
                if (accessEntity != null)
                    entity.AccessEntities.Add(new SubscriptionPlanAccessEntity { AccessEntityId = accessEntityId });
            }
        }

        public int Order => 10;

        public void HandleEvent(EntityUpdated<SubscriptionPlan, SubscriptionPlanDto> eventMessage)
        {
            AssignPermissionToSubscription(eventMessage.Dto, eventMessage.Entity);

            Repository.Update(eventMessage.Entity);
            Repository.SaveChanges();
        }
    }
}