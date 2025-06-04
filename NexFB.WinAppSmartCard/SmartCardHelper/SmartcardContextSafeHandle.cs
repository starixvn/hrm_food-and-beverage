namespace NexFB.WinAppSmartCard.SmartCardHelper
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security;

    /// <summary>
    /// Smart card Context Safe Handle.
    /// </summary>
    internal sealed class SmartcardContextSafeHandle : SafeHandle
    {
        public SmartcardContextSafeHandle()
            : base(IntPtr.Zero, true)
        {
        }

        public override bool IsInvalid
        {
            ////[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
            [SecurityCritical]
            get { return this.handle == IntPtr.Zero; }
        }

        protected override bool ReleaseHandle()
        {
            SmartcardErrorCode result = (SmartcardErrorCode)UnsafeNativeMethods.ReleaseContext(handle);
            return result == SmartcardErrorCode.None;
        }
    }
}
