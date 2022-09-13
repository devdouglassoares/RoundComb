using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Membership.Core.Contracts.AuthAttributes;
using Membership.Library.Contracts;
using Membership.Library.Dto;

namespace Membership.Api.Controllers
{
    public class ConnectionInfoController : ApiController
    {
        private readonly IConnectionInfoService connectionInfoService;

        public ConnectionInfoController(IConnectionInfoService connectionInfoService)
        {
            this.connectionInfoService = connectionInfoService;
        }

        [HttpGet]
        [RequireAuthTokenApi]
        public IHttpActionResult GetConnectionInfo(int id)
        {
            return Ok(this.connectionInfoService.GetConnectionInfo(id));
        }

        [HttpGet]
        [RequireAuthTokenApi]
        public IHttpActionResult GetConnectionInfoWithPin(int id, int pin)
        {
            return Ok(connectionInfoService.GetConnectionInfo(id, pin));
        }

        [HttpPost]
        [RequireAuthTokenApi]
        public IHttpActionResult RequestPin(long id)
        {
            return Ok(connectionInfoService.RequestPin(id));
        }

        [HttpPost]
        [RequireAuthTokenApi]
        public IHttpActionResult SaveConnectionInfo(ConnectionInfoDto model)
        {
            this.connectionInfoService.SaveConnectionInfo(model);
            return Ok();
        }
    }
}