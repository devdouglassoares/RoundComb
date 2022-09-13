namespace Subscription.Core.Entities
{
    /// <summary>
    ///     Subscription Interval
    /// </summary>
    public enum SubscriptionInterval
    {
        /// <summary>
        ///     Monthly
        /// </summary>
        Monthly = 1,

        /// <summary>
        ///     Every 3 months
        /// </summary>
        EveryThreeMonths = 3,

        /// <summary>
        ///     Every 6 months
        /// </summary>
        EverySixMonths = 6,

        /// <summary>
        ///     Yearly
        /// </summary>
        Yearly = 12
    }
}