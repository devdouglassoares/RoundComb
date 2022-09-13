using ProductManagement.Core.Dto;
using ProductManagement.Core.Entities;
using ProductManagement.Core.Services;
using ProductManagement.Core.SignalR;
using System;

namespace ProductManagement.Api.Hubs
{
    public class PropertyHistoryHub : AuthenticationEnabledGenericHub
    {
        private readonly IPropertyHistoryService _propertyHistoryService;
        private readonly IPropertyService _propertyService;

        public PropertyHistoryHub(IPropertyHistoryService propertyHistoryService, IPropertyService propertyService)
        {
            _propertyHistoryService = propertyHistoryService;
            _propertyService = propertyService;
        }

        public void SaveProductView(long productId)
        {
            var userId = GetUserIdInLong(Context.Request);

            var property = _propertyService.First(x => x.Id == productId);
            if (property == null || property.OwnerId == userId)
                return;

            var propertyHistoryRecord = new PropertyHistoryDto
                                       {
                                           RecordDate = DateTimeOffset.Now,
                                           UserId = userId,
                                           PropertyId = productId,
                                           PropertyHistoryType = PropertyHistoryType.UserVisited
                                       };

            if (userId == 0)
                propertyHistoryRecord.Comment = "Viewed by anonymous user";

            _propertyHistoryService.Create(propertyHistoryRecord);
        }

        public void SaveProductInterested(long productId, string message)
        {
            var userId = GetUserIdInLong(Context.Request);
            if (userId == 0)
            {
                return;
            }

            var property = _propertyService.First(x => x.Id == productId);
            if (property == null || property.OwnerId == userId)
                return;

            var propertyHistoryRecord = new PropertyHistoryDto
                                       {
                                           RecordDate = DateTimeOffset.Now,
                                           UserId = userId,
                                           PropertyId = productId,
                                           Comment = message,
                                           PropertyHistoryType = PropertyHistoryType.UserInterested
                                       };
            _propertyHistoryService.Create(propertyHistoryRecord);
        }
    }
}