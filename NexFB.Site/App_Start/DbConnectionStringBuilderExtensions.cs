namespace NexFB.Site
{
    using System.Data.Common;

    public static class DbConnectionStringBuilderExtensions
    {
        internal const string DefaultServerKeyword = "data source";

        internal const string DefaultDatabaseKeyword = "initial catalog";

        internal const string DefaultUserIdKeyword = "user id";

        internal const string DefaultPasswordKeyword = "password";

        internal static readonly string[] ServerAliases = new[] { "data source", "server", "host", "datasource", "address", "addr", "network address" };

        internal static readonly string[] DatabaseAliases = new[] { "initial catalog", "database" };

        internal static readonly string[] UserIdAliases = new[] { "user id", "uid", "username", "user name", "user", "userid" };

        internal static readonly string[] PasswordAliases = new[] { "password", "pwd" };

        public static string GetServer(this DbConnectionStringBuilder connectionStringBuilder)
        {
            return GetValue(connectionStringBuilder, ServerAliases);
        }

        public static void SetServer(this DbConnectionStringBuilder connectionStringBuilder, string serverName)
        {
            if (!ChangeValue(connectionStringBuilder, ServerAliases, serverName))
            {
                connectionStringBuilder["data source"] = serverName;
            }
        }

        public static string GetDatabase(this DbConnectionStringBuilder connectionStringBuilder)
        {
            return GetValue(connectionStringBuilder, DatabaseAliases);
        }

        public static void SetDatabase(this DbConnectionStringBuilder connectionStringBuilder, string databaseName)
        {
            if (!ChangeValue(connectionStringBuilder, DatabaseAliases, databaseName))
            {
                connectionStringBuilder["initial catalog"] = databaseName;
            }
        }

        public static string GetUserId(this DbConnectionStringBuilder connectionStringBuilder)
        {
            return GetValue(connectionStringBuilder, UserIdAliases);
        }

        public static void SetUserId(this DbConnectionStringBuilder connectionStringBuilder, string userId)
        {
            if (!ChangeValue(connectionStringBuilder, UserIdAliases, userId))
            {
                connectionStringBuilder["user id"] = userId;
            }
        }

        public static string GetPassword(this DbConnectionStringBuilder connectionStringBuilder)
        {
            return GetValue(connectionStringBuilder, PasswordAliases);
        }

        public static void SetPassword(this DbConnectionStringBuilder connectionStringBuilder, string password)
        {
            if (!ChangeValue(connectionStringBuilder, PasswordAliases, password))
            {
                connectionStringBuilder["password"] = password;
            }
        }

        private static string GetValue(DbConnectionStringBuilder connectionStringBuilder, string[] keywordAliases)
        {
            foreach (string keyword in keywordAliases)
            {
                if (connectionStringBuilder.ContainsKey(keyword))
                {
                    return connectionStringBuilder[keyword] as string;
                }
            }

            return null;
        }

        private static bool ChangeValue(DbConnectionStringBuilder connectionStringBuilder, string[] keywordAliases, string value)
        {
            foreach (string keyword in keywordAliases)
            {
                if (connectionStringBuilder.ContainsKey(keyword))
                {
                    connectionStringBuilder[keyword] = value;
                    return true;
                }
            }

            return false;
        }
    }
}