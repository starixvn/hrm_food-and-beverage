namespace NexFB.Pos.Core.InternetBased
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
