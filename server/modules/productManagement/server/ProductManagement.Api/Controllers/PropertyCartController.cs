using Core.Exceptions;
using Core.WebApi.ActionFilters;
using Core.WebApi.Controllers;
using Core.WebApi.Extensions;
using Membership.Core;
using Membership.Core.Contracts.AuthAttributes;
using ProductManagement.Core.Dto;
using ProductManagement.Core.Entities;
using ProductManagement.Core.Services;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProductManagement.Api.Controllers
{
    [RoutePrefix("api/propertyCarts")]
    [ErrorResponseHandler(typeof(BaseNotFoundException<>), HttpStatusCode.NotFound)]
    [ErrorResponseHandler(typeof(UnauthorizedAccessException), HttpStatusCode.Unauthorized)]
    [ErrorResponseHandler(typeof(InvalidOperationException), HttpStatusCode.BadRequest)]
    public class PropertyCartController : BaseCrudController<PropertyCart, PropertyCartDto, IPropertyCartService>
    {
        private readonly IMembership _membership;

        public PropertyCartController(IPropertyCartService crudService, IMembership membership) : base(crudService)
        {
            _membership = membership;
        }

        [RequireAuthTokenApi]
        [HttpGet, HttpHead]
        public override HttpResponseMessage GetAll()
        {
            var cart = CrudService.GetAvailableCartForUser(_membership.UserId);

            if (cart == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            return Request.CreateResponse(HttpStatusCode.OK, CrudService.ToDto(cart), cart.ModifiedDate);
        }

        [HttpPut, Route("{cartId:int}/{propertyId:int}/{quantity:int}")]
        [RequireAuthTokenApi]
        public HttpResponseMessage UpdateCardItem(long propertyId, long cartId, int quantity)
        {
            CrudService.UpdateCartItem(cartId, propertyId, quantity);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPost, Route("{cartId:int}/checkedout")]
        [RequireAuthTokenApi]
        public HttpResponseMessage MarkCartAsCheckout(long cartId)
        {
            CrudService.MarkCartAsCheckedOut(cartId);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpDelete, Route("{cartId:int}/{propertyId:int}")]
        [RequireAuthTokenApi]
        public HttpResponseMessage RemovePropertyFromCart(long cartId, long propertyId)
        {
            CrudService.RemovePropertyFromCart(cartId, propertyId);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        public override HttpResponseMessage Create(PropertyCartDto model)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Delete(long id)
        {
            throw new NotImplementedException();
        }
    }
}