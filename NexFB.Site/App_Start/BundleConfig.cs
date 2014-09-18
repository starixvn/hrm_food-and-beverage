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
    using System.Diagnostics.CodeAnalysis;
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
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.AddAndInclude(
                "~/Src/Content/login/css",
                "~/Src/Content/Less/bootstrap.less",
                "~/Src/Content/site/auth/style.less");

            bundles.AddAndInclude(
                "~/bundles/less",
                "~/Src/Content/Less/bootstrap.less");

            bundles.Add(new StyleBundle("~/Src/Content/Common/css").Include(
                "~/Src/Content/Common/bootstrap-theme.min.css",
                "~/Src/Content/Common/toastr.css",
                "~/Src/Content/Common/angular-multi-select.css",
                "~/Src/Content/Common/jquery-ui.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/vendor").Include(
                "~/Src/Components/Libs/jquery-2.1.0.min.js",
                "~/Src/Components/Libs/angular.js",
                "~/Src/Components/Libs/angular-resource.js",
                "~/Src/Components/Libs/angular-route.js",
                "~/Src/Components/Libs/angular-translate.js",
                "~/Src/Components/Libs/bootstrap.min.js",
                "~/Src/Components/Libs/ui-bootstrap-tpls-0.11.0.min.js",
                "~/Src/Components/Libs/toastr.js",
                "~/Src/Components/Libs/angular-multi-select.js",
                "~/Src/Components/Libs/jquery-ui.min.js",
                "~/Src/Components/Libs/datepicker.js",
                "~/Src/Components/Libs/date.js"));

            bundles.Add(new ScriptBundle("~/bundles/signalr").Include(
                "~/Src/Components/Libs/jquery.signalR-1.1.4.min.js"));

            bundles.AddAndInclude(
                "~/NexFB/Src/Less",
                "~/Src/Content/Site/variables.less",
                "~/Src/Content/Site/layout.less",
                "~/Src/App/EmployeeRegistration/employeeRegistration.less",
                "~/Src/App/UserControl/userControl.less",
                "~/Src/App/Ordering/ordering.less",
                "~/Src/App/FoodItemManagement/foodItemManagement.less",
                "~/Src/App/FoodMenuManagement/foodMenuManagement.less");

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                "~/Src/App/app.js",
                "~/Src/App/EmployeeRegistration/employeeRegistration-route.js",
                "~/Src/App/EmployeeRegistration/employeeRegistration-controller.js",
                "~/Src/App/EmployeeRegistration/employeeRegistration-factory.js",
                "~/Src/App/EmployeeRegistration/employeeRegistration-directive.js",
                "~/Src/App/UserControl/userControl-route.js",
                "~/Src/App/UserControl/userControl-controller.js",
                "~/Src/App/UserControl/userControl-factory.js",
                "~/Src/App/UserControl/userControl-directive.js",
                "~/Src/App/Resources/resources-controller.js",
                "~/Src/App/Resources/resources-factory.js",
                "~/Src/App/Logout/logout-controller.js",
                "~/Src/App/Ordering/ordering-route.js",
                "~/Src/App/Ordering/ordering-controller.js",
                "~/Src/App/Ordering/ordering-factory.js",
                "~/Src/App/FoodItemManagement/foodItemManagement-route.js",
                "~/Src/App/FoodItemManagement/foodItemManagement-controller.js",
                "~/Src/App/FoodItemManagement/foodItemManagement-factory.js",
                "~/Src/App/FoodMenuManagement/foodMenuManagement-route.js",
                "~/Src/App/FoodMenuManagement/foodMenuManagement-controller.js",
                "~/Src/App/FoodMenuManagement/foodMenuManagement-factory.js"));

            bundles.Add(new ScriptBundle("~/bundles/extensions").Include(
                "~/Src/Components/Extensions/Common/error-factory.js",
                "~/Src/Components/Extensions/Common/notification-factory.js",
                "~/Src/Components/Extensions/Directives/fb-directives.js",
                "~/Src/Components/Extensions/Services/fb-service.js",
                "~/Src/Components/Extensions/Filters/fb-filter.js",
                "~/Src/Components/Extensions/Common/modalconfirm-controller.js",
                "~/Src/Components/Extensions/Common/common-factory.js"));
        }
    }
}