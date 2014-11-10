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
    using System.Collections;
    using System.Linq;
    using System.Reflection;
    using NexFB.Pos.Core.Constants;

    /// <summary>
    /// Resource Manager Helper.
    /// </summary>
    public static class ResourceManagerHelper
    {
        /// <summary>
        /// Method get resource path exists in an assembly.
        /// </summary>
        /// <param name="resourcePath">Resource path.</param>
        /// <returns>Check path exists or not.</returns>
        public static bool GetResourceNames(string resourcePath)
        {
            string[] resourceNames;
            string assemblyName = Constant.PosKitchenAssembly;
            var assembly = Assembly.Load(assemblyName);
            string resName = assemblyName + ".g.resources";
            using (var stream = assembly.GetManifestResourceStream(resName))
            {
                using (var reader = new System.Resources.ResourceReader(stream))
                {
                    resourceNames = reader.Cast<DictionaryEntry>().Select(entry => (string)entry.Key).ToArray();
                }
            }

            return resourceNames.Contains(resourcePath);
        }
    }
}
