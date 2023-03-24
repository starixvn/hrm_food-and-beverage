using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace NexFB.Site
{
    public static class Cryptography
    {
        private static readonly DESCryptoServiceProvider CryptoServiceProvider = new DESCryptoServiceProvider();

        private static readonly byte[] RgbKey = new byte[8] { 33, 99, 84, 90, 145, 172, 110, 204 };

        private static readonly byte[] RgbIv = Encoding.UTF8.GetBytes("&％#@?,:*!~^.");

        public static string EncryptPassword(string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            using (var encryptor = CryptoServiceProvider.CreateEncryptor(RgbKey, RgbIv))
            {
                var plainPasswordInBytes = Encoding.UTF8.GetBytes(password);

                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plainPasswordInBytes, 0, plainPasswordInBytes.Length);
                        cryptoStream.FlushFinalBlock();

                        return Convert.ToBase64String(memoryStream.ToArray());
                    }
                }
            }
        }

        public static string DecryptPassword(string encryptedPassword)
        {
            if (encryptedPassword == null)
            {
                throw new ArgumentNullException(nameof(encryptedPassword));
            }

            using (var decryptor = CryptoServiceProvider.CreateDecryptor(RgbKey, RgbIv))
            {
                var standardizedEncryptedPassword = StandardizeEncryptedPasswordLength(encryptedPassword);
                var encryptedPasswordInBytes = Convert.FromBase64String(standardizedEncryptedPassword);

                using (var memoryStream = new MemoryStream(encryptedPasswordInBytes))
                {
                    var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

                    using (var streamReader = new StreamReader(cryptoStream))
                    {
                        return streamReader.ReadToEnd().Trim();
                    }
                }
            }
        }

        public static string DecryptConnectionString(string connectionString)
        {
            if (connectionString == null)
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            var connectionStringBuilder = new DbConnectionStringBuilder
            {
                ConnectionString = connectionString
            };
            var encryptedPassword = connectionStringBuilder.GetPassword();

            if (!string.IsNullOrEmpty(encryptedPassword))
            {
                var decryptedPassword = DecryptPassword(encryptedPassword);

                connectionStringBuilder.SetPassword(decryptedPassword);

                return connectionStringBuilder.ToString();
            }

            return connectionString;
        }

        private static string StandardizeEncryptedPasswordLength(string encryptedPassword)
        {
            var result = string.Empty;

            if (!string.IsNullOrWhiteSpace(encryptedPassword))
            {
                int allowedLength = encryptedPassword.Length >> 2 << 2;

                result = encryptedPassword.Substring(0, allowedLength);
            }

            return result;
        }
    }
}