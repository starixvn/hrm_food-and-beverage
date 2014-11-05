// --------------------------------------------------------------------------
// <copyright file="DBConnectionAvailability.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.11.5">Created</change>
// </history>

namespace NexFB.WinApp.Core.InternetBased
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using Fanex.Data;
    using NexFB.WinApp.Core.Constants;

    /// <summary>
    /// DB Connection Availability.
    /// </summary>
    public static class DBConnectionAvailability
    {
        /// <summary>
        /// Check Database connection.
        /// </summary>
        /// <returns>Boolean to check.</returns>
        public static bool IsDBConnectionAvailable()
        {
            SqlConnection sqlConnection;
            bool connectionResult = false;
            string serverDatabase = ConfigurationManager.AppSettings[Constant.ServerDB];
            if (!string.IsNullOrEmpty(serverDatabase))
            {
                try
                {
                    string connectionString = ConnectionStringGetter.GetConnectionString(serverDatabase);
                    sqlConnection = new SqlConnection(connectionString);
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
