namespace NexFB.Pos.Core.Helpers
{
    using System;
    using System.IO;
    using System.Media;

    /// <summary>
    /// Notification Sound.
    /// </summary>
    public static class NotificationSound
    {
        public static void PlayingNotificationSound()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string defaultImageSource = Path.Combine(baseDirectory, "Sounds\\Message.wav");
            if (ResourceManagerHelper.GetResourceNames(defaultImageSource))
            {
                SoundPlayer sp = new SoundPlayer(defaultImageSource);
                sp.Play();
            }
        }
    }
}
