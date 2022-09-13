using Core.ObjectMapping;
using Subscription.Core.Dtos;
using Subscription.Core.Entities;
using System.Linq;

namespace Subscription.Core.MappingRegistration
{
    public class SubscriptionMappingRegistration : IObjectMappingRegistration
    {
        public void ConfigureMapping(IMappingService map)
        {
            map.ConfigureMapping<SubscriptionPlan, SubscriptionPlanDto>()
                .ForMember(dto => dto.AccessEntityIds, a => a.MapFrom(entity => entity.AccessEntities.Select(accessEntity => accessEntity.AccessEntityId)));

            map.ConfigureMapping<SubscriptionPlanDto, SubscriptionPlan>()
                .ForMember(entity => entity.Properties, a => a.Ignore());

            map.ConfigureMapping<SubscriptionInvoice, InvoiceDto>().ReverseMap();
        }
    }
}