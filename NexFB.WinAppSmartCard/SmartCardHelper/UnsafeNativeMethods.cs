// --------------------------------------------------------------------------
// <copyright file="UnsafeNativeMethods.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.14">Created</change>
// </history>

namespace NexFB.WinAppSmartCard.SmartCardHelper
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;
    using NexFB.WinAppSmartCard.Enums;

    /// <summary>
    /// Unsafe Native Methods.
    /// </summary>
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Reviewed. Suppression is OK here.")]
    internal sealed partial class UnsafeNativeMethods
    {
        private UnsafeNativeMethods()
        {
        }

        #region WinScard.DLL Imports

        [DllImport("WINSCARD.DLL", EntryPoint = "SCardEstablishContext", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern uint EstablishContext(
            ScopeOption scope,
            IntPtr reserved1,
            IntPtr reserved2,
            ref SmartcardContextSafeHandle context);

        [DllImport("WINSCARD.DLL", EntryPoint = "SCardReleaseContext", CharSet = CharSet.Unicode,
            SetLastError = true)]
        internal static extern uint ReleaseContext(IntPtr context);

        [DllImport("WINSCARD.DLL", EntryPoint = "SCardListReaders", CharSet = CharSet.Unicode,
            SetLastError = true)]
        internal static extern uint ListReaders(
            SmartcardContextSafeHandle context,
            string groups,
            string readers,
            ref int size);

        [DllImport("WINSCARD.DLL", EntryPoint = "SCardGetStatusChange", CharSet = CharSet.Unicode,
            SetLastError = true)]
        internal static extern uint GetStatusChange(
            [In(), Out()] SmartcardContextSafeHandle context,
            [In(), Out()] int timeout,
            [In(), Out()] ReaderState[] states,
            [In(), Out()] int count);

        #endregion
    }
}
