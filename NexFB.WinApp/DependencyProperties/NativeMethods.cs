// --------------------------------------------------------------------------
// <copyright file="NativeMethods.cs" company="Nexcel Solutions Vietnam">
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
    /// Native Methods.
    /// </summary>
    internal static class NativeMethods
    {
        [DllImport("wininet.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool InternetGetConnectedState(out int description, int reservedValue);
    }
}
