using Membership.Core.Contracts;
using Membership.Core.Contracts.AuthAttributes;
using Membership.Core.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Membership.Api.Controllers
{
    [RequireAuthTokenApi]
    public class PermissionController : ApiController
    {
        private readonly IPermissionService _permissionService;

        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        public IList<PermissionModel> GetPermissions()
        {
            var source = _permissionService.GetAll();

            return source.ToList();
        }
    }
}