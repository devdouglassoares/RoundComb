using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace RoundComb.DataAccess
{
    internal class ConnectionProvider
    {
        internal static IDbConnection OpenConnectionDBServer()
        {
            string conn = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;
            IDbConnection connection = new SqlConnection(conn);
            connection.Open();
            return connection;
        }

    }
}
