namespace Subscription.Api.Models
{
    public class StripeProcessModel
    {
        public string PlanId { get; set; }
        public string StripeToken { get; set; }
    }
}