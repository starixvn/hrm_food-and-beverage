// --------------------------------------------------------------------------
// <copyright file="InternetAvailability.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.9.5">Created</change>
// </history>

namespace NexFB.WinApp.DependencyProperties
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// Internet Connection Availability.
    /// </summary>
    public static class InternetAvailability
    {
        public static bool IsInternetAvailable()
        {
            int description;
            return NativeMethods.InternetGetConnectedState(out description, 0);
        }
    }
}
