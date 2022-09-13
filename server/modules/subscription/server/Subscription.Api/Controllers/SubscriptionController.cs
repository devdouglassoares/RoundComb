using Membership.Core;
using Membership.Core.Contracts.AuthAttributes;
using Subscription.Core.Entities;
using Subscription.Service.Contracts;
using Subscription.Service.DataServices.Interfaces;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Subscription.Api.Controllers
{
    [RoutePrefix("api/subscriptions")]
    public class SubscriptionController : ApiController
    {
        private readonly IMembership _membership;
        private readonly ISubscriptionDataService _subscriptionDataService;
        private readonly ISubscriptionPlanService _subscriptionPlanService;
        private readonly IInvoiceDataService _invoiceDataService;

        public SubscriptionController(IMembership membership,
                                      ISubscriptionDataService subscriptionDataService,
                                      ISubscriptionPlanService subscriptionPlanService,
                                      IInvoiceDataService invoiceDataService)
        {
            _membership = membership;
            _subscriptionDataService = subscriptionDataService;
            _subscriptionPlanService = subscriptionPlanService;
            _invoiceDataService = invoiceDataService;
        }

        [HttpGet, Route("plans")]
        public HttpResponseMessage Plans()
        {
            var all = _subscriptionPlanService.GetAll();
            return Request.CreateResponse(HttpStatusCode.OK, all);
        }

        [HttpGet, Route("activeSubscription")]
        [RequireAuthTokenApi]
        public HttpResponseMessage GetActiveSubscriptionForCurrentUser()
        {
            var subscription = _subscriptionDataService.GetCurrentActiveSubscriptionForUser(_membership.UserId);
            return Request.CreateResponse(HttpStatusCode.OK, subscription);
        }

        [HttpPost, Route("subscribe/{planId}")]
        [RequireAuthTokenApi]
        public HttpResponseMessage SubscribeTrial(long planId)
        {
            var subscriptionPlan = _subscriptionPlanService.GetEntity(planId);

            if (subscriptionPlan == null || !subscriptionPlan.IsTrialSubscription)
                throw new InvalidOperationException("Cannot subscribe to specified subscription because this is not evaluation-enabled subscription");

            var subscription = _subscriptionDataService.SubscribeUserToSubscriptionPlan(_membership.UserId, planId.ToString());

            _invoiceDataService.CreateOrUpdateInvoice(new SubscriptionInvoice
            {
                AmountDue = 0,
                Closed = true,
                Currency = subscription.SubscriptionPlan.Currency,
                Date = DateTime.UtcNow,
                Description = $"Subscribed to subscription: {subscription.SubscriptionPlan.Name}",
                Paid = true,
                ReceiptNumber = "N/A",
                ExternalId = null,
                Total = 0,
                CustomerId = _membership.UserId,
                StatementDescriptor = subscription.SubscriptionPlan.Name
            });

            return Request.CreateResponse(HttpStatusCode.OK, subscription);
        }
    }
}