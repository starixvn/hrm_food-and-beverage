// ----------------------------------------------------------------------
// <copyright file="BundleConfig.cs" company="Nexcel Solutions Viet Nam">
// Copyright statement. All right reserved
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="8/21/2014 3:28:12 PM">Created.</change>
// </history>

namespace NexFB.Site.App_Start
{
    using System.Web.Optimization;

    /// <summary>
    /// Bundle Config.
    /// </summary>
    public static class BundleConfig
    {
        /// <summary>
        /// Register Bundles.
        /// </summary>
        /// <param name="bundles">The bundle collection.</param>
        public static void ResgisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Src/Content/Common/css").Include(
                "~/Src/Content/Common/bootstrap.min.css",
                "~/Src/Content/Common/bootstrap-theme.min.css"));

            bundles.Add(new StyleBundle("~/Src/Components/Libs/vendor").Include(
                "~/Src/Components/Libs/jquery-2.1.0.min.js",
                "~/Src/Components/Libs/angular.min.js",
                "~/Src/Components/Libs/bootstrap.min.js",
                "~/Src/Components/Libs/ui-bootstrap-tpls-0.10.0.min.js",
                "~/Src/Components/Libs/angular-resource.js",
                "~/Src/Components/Libs/angular-route.js",
                "~/Src/Components/Libs/angular-translate.js"));
        }
    }
}