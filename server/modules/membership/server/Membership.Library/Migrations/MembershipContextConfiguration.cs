using Membership.Library.Data.Context;
using System.Data.Entity.Migrations;

namespace Membership.Library.Migrations
{
    internal sealed class MembershipContextConfiguration : DbMigrationsConfiguration<MembershipContext>
    {
        public MembershipContextConfiguration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Membership.Library.Migrations.Configuration";
        }

        protected override void Seed(MembershipContext context)
        {
            
        }
    }
}