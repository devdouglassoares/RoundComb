namespace Subscription.Api.Models
{
    public class PaypalCallbackProcessModel
    {
        public string custom { get; set; }
        public decimal mc_gross { get; set; }
        public string payment_status { get; set; }
        public string txn_id { get; set; }
    }
}