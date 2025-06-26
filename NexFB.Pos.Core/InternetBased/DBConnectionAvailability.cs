namespace NexFB.Pos.Core.InternetBased
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using Constants;
    using Fanex.Data;

    public static class DBConnectionAvailability
    {
        public static bool MainDBConnectionAvailable()
        {
            var serverDatabase = ConfigurationManager.AppSettings[Constant.ServerDB];
            return InitializeDBConnection(serverDatabase);
        }

        public static bool LocalDBConnectionAvailable()
        {
            var localDatabase = ConfigurationManager.AppSettings[Constant.LocalDB];
            return InitializeDBConnection(localDatabase);
        }

        private static bool InitializeDBConnection(string targetDatabase)
        {
            var connectionResult = false;
            if (!string.IsNullOrEmpty(targetDatabase))
            {
                var connectionString = ConnectionStringGetter.GetConnectionString(targetDatabase);
                var sqlConnection = new SqlConnection(connectionString);
                try
                {
                    sqlConnection.Open();
                    connectionResult = sqlConnection.State == ConnectionState.Open;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return connectionResult;
        }
    }
}