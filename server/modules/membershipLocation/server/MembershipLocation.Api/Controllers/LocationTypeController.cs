using Core.WebApi.Controllers;
using MembershipLocation.Api.Dto;
using MembershipLocation.Api.Models;
using MembershipLocation.Api.Services;
using System.Web.Http;

namespace MembershipLocation.Api.Controllers
{
    [RoutePrefix("api/locationType")]
    public class LocationTypeController: BaseCrudController<LocationType, LocationTypeDto, ILocationTypeService>
    {
        public LocationTypeController(ILocationTypeService crudService) : base(crudService)
        {
        }
    }
}