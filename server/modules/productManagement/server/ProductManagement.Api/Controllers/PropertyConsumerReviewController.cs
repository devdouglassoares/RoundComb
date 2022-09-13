using Core.Exceptions;
using Core.WebApi.ActionFilters;
using Core.WebApi.Extensions;
using EntityReviews.Dtos;
using ProductManagement.Core.Services;
using Membership.Core.Contracts.AuthAttributes;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProductManagement.Api.Controllers
{
    [RoutePrefix("api/consumerReview")]
    [ErrorResponseHandler(typeof(BaseNotFoundException<>), HttpStatusCode.NotFound)]
    [ErrorResponseHandler(typeof(UnauthorizedAccessException), HttpStatusCode.Unauthorized)]
    public class PropertyConsumerReviewController : ApiController
    {
        private readonly IPropertyConsumerReviewService _propertyConsumerReviewService;

        public PropertyConsumerReviewController(IPropertyConsumerReviewService propertyConsumerReviewService)
        {
            _propertyConsumerReviewService = propertyConsumerReviewService;
        }

        [HttpGet, HttpHead, Route("{reviewId}")]
        public HttpResponseMessage GetReview(long reviewId)
        {
            var review = _propertyConsumerReviewService.GetReview(reviewId);

            return Request.CreateResponse(HttpStatusCode.OK, review, review.ModifiedDate);
        }

        [HttpGet, HttpHead, Route("{customerId}/{propertyId}")]
        public HttpResponseMessage GetReview(long customerId, long propertyId)
        {
            var review = _propertyConsumerReviewService.GetReview(customerId, propertyId);

            return Request.CreateResponse(HttpStatusCode.OK, review, review.ModifiedDate);
        }

        [HttpGet, HttpHead, Route("getReviews/{customerId}/{propertyId}")]
        public HttpResponseMessage GetReviews(long customerId, long propertyId)
        {
            var result = _propertyConsumerReviewService.GetReviews(customerId, propertyId);
            var latestUpdateDate = result.Max(x => x.ModifiedDate);

            return Request.CreateResponse(HttpStatusCode.OK, result, latestUpdateDate);
        }

        [HttpPut, Route("{reviewId}")]
        [RequireAuthTokenApi]
        public HttpResponseMessage UpdateReview(long reviewId, EntityReviewModel model)
        {
            _propertyConsumerReviewService.UpdateReview(reviewId, model);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPost, Route("")]
        [RequireAuthTokenApi]
        public HttpResponseMessage CreateReview(EntityReviewModel model)
        {
            _propertyConsumerReviewService.SaveReview(model);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpGet, HttpHead, Route("")]
        public HttpResponseMessage GetReviewForConsumer(long consumerId)
        {
            var result = _propertyConsumerReviewService.GetConsumerReviews(consumerId);

            var latestUpdateDate = result.Max(x => x.ModifiedDate);

            return Request.CreateResponse(HttpStatusCode.OK, result, latestUpdateDate);
        }
    }
}
