using Core.WebApi.Controllers;
using Membership.Core.Contracts;
using Membership.Core.Dto;
using Membership.Core.Entities;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Membership.Api.Controllers
{
    [RoutePrefix("api/contacts")]
    public class UserContactController : BaseCrudController<Contact, ContactDto, IContactService>
    {
        public UserContactController(IContactService crudService) : base(crudService)
        {
        }

        [HttpPost, Route("{userId}/{contactType}")]
        public HttpResponseMessage CreateContactForUser(long userId, string contactType, ContactDto model)
        {
            model.Type = contactType;
            model.UserId = userId;

            var entity = CrudService.Create(model);

            return Request.CreateResponse(HttpStatusCode.OK, CrudService.ToDto(entity));
        }

        [HttpGet, Route("{userId}/{contactType}")]
        public HttpResponseMessage GetContactForUser(long userId, string contactType)
        {
            var entities = CrudService.Fetch(x => x.Type == contactType && x.UserId == userId);

            return Request.CreateResponse(HttpStatusCode.OK, CrudService.ToDtos(entities));
        }
    }
}
