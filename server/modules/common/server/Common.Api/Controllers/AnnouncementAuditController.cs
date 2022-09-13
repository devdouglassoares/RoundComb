using System;
using Common.Services.Interfaces;
using Membership.Core;
using Membership.Core.Contracts.AuthAttributes;
using System.Web.Http;

namespace Common.Api.Controllers
{
    [RequireAuthTokenApi]
    public class AnnouncementAuditController : ApiController
    {
        public readonly IMembership membership;
        public readonly IAnnouncementService announcementService;

        public AnnouncementAuditController(IMembership membership, IAnnouncementService announcementService)
        {
            this.membership = membership;
            this.announcementService = announcementService;
        }

        [HttpGet]
        public IHttpActionResult LogAnnouncementViewed(string id)
        {
            announcementService.LogAnnouncementViewed(id, membership.UserId);
            return Ok();
        }

        [HttpGet]
        public IHttpActionResult IsAnnouncementViewed(string id, DateTime lastUpdateDate)
        {
            return Ok(announcementService.IsAnnouncementViewed(id, lastUpdateDate.ToUniversalTime(), membership.UserId));
        }
    }
}
