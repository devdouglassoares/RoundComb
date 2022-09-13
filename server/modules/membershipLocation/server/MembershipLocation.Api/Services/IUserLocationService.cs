using Core;
using Core.ObjectMapping;
using MembershipLocation.Api.Dto;
using MembershipLocation.Api.Models;
using MembershipLocation.Api.Repositories;
using System.Collections.Generic;

namespace MembershipLocation.Api.Services
{
    public interface IUserLocationService : IDependency
    {
        void AssignLocation(long userId, long locationId, long? locationTypeId, bool isDefault);

        UserLocationDto GetUserLocation(long userId, long locationId);

        IEnumerable<UserLocationDto> GetUserLocations(long userId);

        void DeleteUserLocation(long userId, long locationId);
    }

    public class UserLocationService : IUserLocationService
    {
        private readonly IRepository _repository;
        private readonly IMappingService _mappingService;

        public UserLocationService(IRepository repository, IMappingService mappingService)
        {
            _repository = repository;
            _mappingService = mappingService;
        }

        public void AssignLocation(long userId, long locationId, long? locationTypeId, bool isDefault)
        {
            if (isDefault)
                RemoveDefaultFromAllOtherLocations(userId);

            DeleteUserLocation(userId, locationId);
            _repository.Insert(new UserLocation
            {
                UserId = userId,
                LocationId = locationId,
                LocationTypeId = locationTypeId,
                IsDefaultAddress = isDefault
            });
            _repository.SaveChanges();
        }

        public UserLocationDto GetUserLocation(long userId, long locationId)
        {
            return
                _mappingService.Map<UserLocationDto>(
                                                     _repository.First<UserLocation>(
                                                                                     x =>
                                                                                     x.UserId == userId &&
                                                                                     x.LocationId == locationId));
        }

        public IEnumerable<UserLocationDto> GetUserLocations(long userId)
        {
            var userLocations = _repository.Fetch<UserLocation>(location => location.UserId == userId);

            return _mappingService.Project<UserLocation, UserLocationDto>(userLocations);
        }

        public void DeleteUserLocation(long userId, long locationId)
        {
            _repository.DeleteByCondition<UserLocation>(x => x.UserId == userId && locationId == x.LocationId);
            _repository.SaveChanges();
        }

        private void RemoveDefaultFromAllOtherLocations(long userId)
        {
            _repository.Update<UserLocation>(userLocation => userLocation.UserId == userId, location => new UserLocation
            {
                IsDefaultAddress = false
            });
            _repository.SaveChanges();
        }
    }
}