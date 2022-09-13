using Core.Database;
using Core.Exceptions;
using Core.ObjectMapping;
using ProductManagement.Core.Dto;
using ProductManagement.Core.Entities;
using ProductManagement.Core.Repositories;
using ProductManagement.Core.Services;
using Membership.Core.Contracts;
using Membership.Core.Entities;
using System.Linq;

namespace ProductManagement.Services
{
    public class PropertyHistoryService : BaseService<PropertyHistory, PropertyHistoryDto>,
                                         IPropertyHistoryService
    {
        private readonly IUserService _userService;

        public PropertyHistoryService(IMappingService mappingService, IRepository repository, IUserService userService) : base(mappingService, repository)
        {
            _userService = userService;
        }

        public IQueryable<PropertyUserHistoryDto> GetPropertyHistoryRecordsGroupByUser(long propertyId, PropertyHistoryType historyType)
        {
            var histories = Fetch(
                x => x.PropertyId == propertyId && x.PropertyHistoryTypeString == historyType.ToString())
                .GroupBy(x => x.UserId)
                .Select(h => new PropertyUserHistoryDto
                {
                    UserId = h.Key,
                    PropertyHistoryType = historyType,
                    Count = h.Count(),
                    FirstRecorded = h.Min(x => x.RecordDate),
                    LastRecorded = h.Max(x => x.RecordDate)
                })
                .ToArray();

            MapUserInformation(histories);

            return histories.AsQueryable();
        }

        private void MapUserInformation(PropertyUserHistoryDto[] propertyUserHistoryDtos)
        {
            var userIds = propertyUserHistoryDtos.Select(x => x.UserId);

            var users = _userService.GetUsers()
                                    .Where(x => userIds.Contains(x.Id))
                                    .Select(x => new
                                    {
                                        UserId = x.Id,
                                        UserName = x.FirstName + ' ' + x.LastName
                                    });

            foreach (var propertyUserHistory in propertyUserHistoryDtos)
            {
                if (propertyUserHistory.UserId == 0 || propertyUserHistory.UserId == null)
                {
                    propertyUserHistory.UserName = "Anonymous users";
                    continue;
                }

                try
                {
                    propertyUserHistory.UserName = users.FirstOrDefault(x => x.UserId == propertyUserHistory.UserId.Value)?.UserName ?? "Unknown user";
                }
                catch (BaseNotFoundException<User>)
                {

                }
            }
        }
    }
}