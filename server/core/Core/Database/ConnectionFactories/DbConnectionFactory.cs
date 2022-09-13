using System;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Web;
using Core.MultiTenancy;

namespace Core.Database.ConnectionFactories
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private const string DefaultConnectionString = "portalConnectionString";

        public DbConnection CreateConnection(string nameOrConnectionString)
        {
            TenantConfiguration connectionSetting;

            if (!ApplicationHost.SupportMultitenant)
            {
                var connectionStr = ConfigurationManager.ConnectionStrings[DefaultConnectionString];
                connectionSetting = new TenantConfiguration
                {
                    DataConnectionString = connectionStr.ConnectionString,
                    DataProvider = connectionStr.ProviderName
                };
            }
            else
            {
                try
                {
                    connectionSetting = HttpContext.Current.GetTenantInfo();
                }
                catch
                {
                    connectionSetting = null;
                }
            }

            if (connectionSetting == null)
                throw new ArgumentException("Cannot get the correct connection string");

            var providerName = connectionSetting.DataProvider;

            var conn = DbProviderFactories.GetFactory(providerName).CreateConnection();

            if (conn == null)
                throw new AccessViolationException(string.Format("Db provider '{0}' could not be found.", providerName));

            conn.ConnectionString = connectionSetting.DataConnectionString;

            return conn;
        }
    }
}