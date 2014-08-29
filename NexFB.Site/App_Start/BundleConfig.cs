// ----------------------------------------------------------------------
// <copyright file="BundleConfig.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="8/21/2014 3:28:12 PM">Created.</change>
// </history>

namespace NexFB.Site
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
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Src/Content/Common/css").Include(
                "~/Src/Content/Common/bootstrap.min.css",
                "~/Src/Content/Common/bootstrap-theme.min.css",
                "~/Src/Content/Common/toastr.css"));

            bundles.Add(new ScriptBundle("~/bundles/vendor").Include(
                "~/Src/Components/Libs/jquery-2.1.0.min.js",
                "~/Src/Components/Libs/angular.js",
                "~/Src/Components/Libs/angular-resource.js",
                "~/Src/Components/Libs/angular-route.js",
                "~/Src/Components/Libs/angular-translate.js",
                "~/Src/Components/Libs/bootstrap.min.js",
                "~/Src/Components/Libs/ui-bootstrap-tpls-0.10.0.min.js",
                "~/Src/Components/Libs/toastr.js"));

            bundles.AddAndInclude(
                "~/NexFB/Src/Less",
                "~/Src/Content/Site/layout.less",
                "~/Src/App/EmployeeRegistration/employeeRegistration.less",
                "~/Src/App/UserControl/userControl.less");

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                "~/Src/App/app.js",
                "~/Src/App/EmployeeRegistration/employeeRegistration-route.js",
                "~/Src/App/EmployeeRegistration/employeeRegistration-controller.js",
                "~/Src/App/EmployeeRegistration/employeeRegistration-factory.js",
                "~/Src/App/UserControl/userControl-route.js",
                "~/Src/App/UserControl/userControl-controller.js",
                "~/Src/App/UserControl/userControl-factory.js",
                "~/Src/App/Resources/resources-controller.js",
                "~/Src/App/Resources/resources-factory.js",
                "~/Src/App/Logout/logout-controller.js"));

            bundles.Add(new ScriptBundle("~/bundles/extensions").Include(
                "~/Src/Components/Extensions/Common/error-factory.js",
                "~/Src/Components/Extensions/Common/notification-factory.js",
                "~/Src/Components/Extensions/Directives/fb-directives.js"));
        }
    }
}