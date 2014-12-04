// --------------------------------------------------------------------------
// <copyright file="NotificationSound.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.12.4">Created</change>
// </history>

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
