using Membership.Core.Contracts;
using Newtonsoft.Json;
using Subscription.Api.Models;
using Subscription.Core.Entities;
using Subscription.Service.DataServices.Interfaces;
using Subscription.Service.Infrastructure.Facades;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Subscription.Api.Controllers
{
    /// <summary>
    /// Response handler for Paypal hook integration
    /// </summary>
    [RoutePrefix("api/paypalIntegration")]
    public class PayPalSubscriptionCallbackController : ApiController
    {
        private readonly IInvoiceDataService _invoiceDataService;
        private readonly IUserService _userService;
        private readonly ISubscriptionsFacade _subscriptionsFacade;

        public PayPalSubscriptionCallbackController(IUserService userService,
                                                    IInvoiceDataService invoiceDataService,
                                                    ISubscriptionsFacade subscriptionsFacade)
        {
            _userService = userService;
            _invoiceDataService = invoiceDataService;
            _subscriptionsFacade = subscriptionsFacade;
        }

        [Route("paypalprocess")]
        public async Task<HttpResponseMessage> PaypalProcess(PaypalCallbackProcessModel model)
        {
            var paypalSubscriptionUserModel = JsonConvert.DeserializeObject<PaypalSubscriptionUserInformation>(model.custom);

            var subscriptionPlanId = paypalSubscriptionUserModel.subscriptionPlanId;

            var user = _userService.GetUser(paypalSubscriptionUserModel.userId);

            if (model.payment_status == "Completed" &&
                (await _invoiceDataService.UserInvoicesAsync(user.Id)).All(i => i.ExternalId != model.txn_id))
            {
                var subscription = _subscriptionsFacade.SubscribeUserAsync(user, subscriptionPlanId.ToString());
                _invoiceDataService.CreateOrUpdateInvoice(new SubscriptionInvoice
                {
                    AmountDue = (int)(model.mc_gross * 100),
                    Closed = true,
                    Currency = subscription.SubscriptionPlan.Currency,
                    Date = DateTime.UtcNow,
                    Description = "Paid via PayPal",
                    Paid = true,
                    ReceiptNumber = model.txn_id,
                    ExternalId = model.txn_id,
                    Total = (int)(model.mc_gross * 100),
                    CustomerId = user.Id,
                    StatementDescriptor = subscription.SubscriptionPlan.Name
                });
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
