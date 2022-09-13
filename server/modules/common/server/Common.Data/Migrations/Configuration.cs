using System.Data.Entity.Migrations;
using Common.Data.Context;

namespace Common.Data.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<CommonContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(CommonContext context)
        {

        }
    }
}
