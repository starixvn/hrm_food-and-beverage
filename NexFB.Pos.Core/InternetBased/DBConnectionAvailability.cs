// --------------------------------------------------------------------------
// <copyright file="DBConnectionAvailability.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.11.5">Created</change>
// </history>

namespace NexFB.Pos.Core.InternetBased
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Data;
    using NexFB.Pos.Core.Constants;

    /// <summary>
    /// DB Connection Availability.
    /// </summary>
    public static class DBConnectionAvailability
    {
        /// <summary>
        /// Check Database connection.
        /// </summary>
        /// <returns>Boolean to check.</returns>
        public static bool MainDBConnectionAvailable()
        {
            string serverDatabase = ConfigurationManager.AppSettings[Constant.ServerDB];
            return InitializeDBConnection(serverDatabase);
        }

        public static bool LocalDBConnectionAvailable()
        {
            string localDatabase = ConfigurationManager.AppSettings[Constant.LocalDB];
            return InitializeDBConnection(localDatabase);
        }

        /// <summary>
        /// Initialize Database connection, and check for connection available.
        /// </summary>
        /// <param name="targetDatabase">Database to check.</param>
        /// <returns>Boolean to check.</returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed. Suppression is OK here.")]        
        private static bool InitializeDBConnection(string targetDatabase)
        {
            SqlConnection sqlConnection;
            bool connectionResult = false;
            if (!string.IsNullOrEmpty(targetDatabase))
            {
                string connectionString = ConnectionStringGetter.GetConnectionString(targetDatabase);
                sqlConnection = new SqlConnection(connectionString);
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
