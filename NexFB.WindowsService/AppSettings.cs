namespace NexFB.WindowsService
{
    using System;
    using System.Configuration;

    public static class AppSettings
    {
        public static int TimerInterval => int.Parse(ConfigurationManager.AppSettings["TimerInterval"]);
        public static bool EnableNexcel => bool.Parse(ConfigurationManager.AppSettings["EnableNexcel"]);
        public static bool EnableHongMA => bool.Parse(ConfigurationManager.AppSettings["EnableHongMa"]);
        public static bool EnableSms => bool.Parse(ConfigurationManager.AppSettings["EnableSMS"]);
        public static bool EnableLuminet => bool.Parse(ConfigurationManager.AppSettings["EnableLuminet"]);
        public static bool IsTransferPhotoNexcel => bool.Parse(ConfigurationManager.AppSettings["IsTransferPhotoNexcel"]);
        public static bool IsTransferPhotoHongMA => bool.Parse(ConfigurationManager.AppSettings["IsTransferPhotoHongMa"]);
        public static bool IsTransferPhotoSms => bool.Parse(ConfigurationManager.AppSettings["IsTransferPhotoSMS"]);
        public static bool IsTransferPhotoLuminet => bool.Parse(ConfigurationManager.AppSettings["IsTransferPhotoLuminet"]);

        // sentry
        public static string Sentry_Category => ConfigurationManager.AppSettings["Sentry:Category"];
        public static string Sentry_Dsn => ConfigurationManager.AppSettings["Sentry:Dsn"];
        public static int Sentry_Timeout => Convert.ToInt32(ConfigurationManager.AppSettings["Sentry:Timeout"]);
    }
}