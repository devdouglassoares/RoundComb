using Core.Database.Entities;
using Newtonsoft.Json;
using Subscription.Core.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Subscription.Core.Entities
{
    /// <summary>
    ///     Subscription Plan
    /// </summary>
    public class SubscriptionPlan : BaseEntity
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SubscriptionPlan" /> class.
        /// </summary>
        public SubscriptionPlan()
        {
            AccessEntities = new List<SubscriptionPlanAccessEntity>();
        }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        [Required]
        public string Name { get; set; }


        /// <summary>
        /// Indicates this subscriptin plan to be trial and no need to make payment
        /// </summary>
        public bool IsTrialSubscription { get; set; }

        /// <summary>
        ///     Gets or sets the price.
        /// </summary>
        /// <value>
        ///     The price.
        /// </value>
        [Required]
        [Range(0.0, 1000000)]
        public double Price { get; set; }

        /// <summary>
        ///     Gets or sets the currency.
        /// </summary>
        /// <value>
        ///     The currency.
        /// </value>
        public string Currency { get; set; }

        /// <summary>
        ///     Gets the currency details.
        /// </summary>
        /// <value>
        ///     The currency details.
        /// </value>
        [NotMapped]
        public Currency CurrencyDetails
        {
            get { return CurrencyHelper.GetCurrencyInfo(Currency); }
        }

        /// <summary>
        ///     Gets or sets the interval.
        /// </summary>
        /// <value>
        ///     The interval.
        /// </value>
        [Required]
        public SubscriptionInterval Interval { get; set; }

        /// <summary>
        ///     Gets or sets the trial period in days.
        /// </summary>
        /// <value>
        ///     The trial period in days.
        /// </value>
        [Display(Name = "Trial period in days")]
        [Range(0, 365)]
        public int TrialPeriodInDays { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="SubscriptionPlan" /> is disabled.
        /// </summary>
        /// <value>
        ///     <c>true</c> if disabled; otherwise, <c>false</c>.
        /// </value>
        public bool Disabled { get; set; }

        [Column("Properties")]
        public string PropertiesString { get; set; }

        [NotMapped]
        public IDictionary<string, string> Properties
        {
            get
            {
                return string.IsNullOrEmpty(PropertiesString) ? new Dictionary<string, string>() : JsonConvert.DeserializeObject<IDictionary<string, string>>(PropertiesString);
            }
            set
            {
                PropertiesString = JsonConvert.SerializeObject(value);
            }
        }

        public long? AssignRoleId { get; set; }

        public virtual ICollection<SubscriptionPlanAccessEntity> AccessEntities { get; set; }

        /// <summary>
        ///     Specifies if the current plan has more rank than the others (Level)
        /// </summary>
        public int PlanLevel { get; set; }
    }
}