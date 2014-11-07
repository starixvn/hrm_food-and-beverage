// --------------------------------------------------------------------------
// <copyright file="InternetAvailability.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.27">Created</change>
// </history>

namespace NexFB.Pos.Core.InternetBased
{
    /// <summary>
    /// Internet Availability.
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
