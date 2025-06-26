namespace NexFB.Pos.Core.Helpers
{
    using System.IO;

    /// <summary>
    /// Resource Manager Helper.
    /// </summary>
    public static class ResourceManagerHelper
    {
        /// <summary>
        /// Method get resource path exists in an assembly.
        /// </summary>
        /// <param name="imageSource">Resource path.</param>
        /// <returns>Check path exists or not.</returns>
        public static bool GetResourceNames(string imageSource)
        {
            return File.Exists(imageSource);
        }
    }
}
