// --------------------------------------------------------------------------
// <copyright file="ResourceManagerHelper.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.11.10">Created</change>
// </history>

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
