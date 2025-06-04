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
