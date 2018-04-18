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
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.AddAndInclude(
                "~/Src/Content/login/css",
                "~/Src/Content/Less/bootstrap.less",
                "~/Src/Content/site/auth/style.less");

            bundles.AddAndInclude(
                "~/bundles/less",
                "~/Src/Content/Less/bootstrap.less",
                "~/Src/Content/Less/Fanex.Grid.less");

            bundles.Add(new StyleBundle("~/Src/Content/Common/css").Include(
                "~/Src/Content/Common/bootstrap-theme.min.css",
                "~/Src/Content/Common/toastr.css",
                "~/Src/Content/Common/angular-multi-select.css",
                "~/Src/Content/Common/jquery-ui.min.css",
                "~/Src/Content/Common/chosen.min.css",
                "~/Src/Content/Common/table.css",
                "~/Src/Content/Common/ng-cells-0.3.3.css",
                "~/Src/Content/Common/Fanex.Grid.css",
                "~/Src/Content/Common/Fanex.Pagination.css"));

            bundles.Add(new ScriptBundle("~/bundles/vendor").Include(
                "~/Src/Components/Libs/jquery-2.1.0.min.js",
                "~/Src/Components/Libs/angular.js",
                "~/Src/Components/Libs/angular-resource.js",
                "~/Src/Components/Libs/angular-route.js",
                "~/Src/Components/Libs/angular-translate.js",
                "~/Src/Components/Libs/ui-bootstrap-tpls-0.11.0.min.js",
                "~/Src/Components/Libs/toastr.js",
                "~/Src/Components/Libs/angular-multi-select.js",
                "~/Src/Components/Libs/jquery-ui.min.js",
                "~/Src/Components/Libs/datepicker.js",
                "~/Src/Components/Libs/date.js",
                "~/Src/Components/Libs/chosen.jquery.min.js",
                "~/Src/Components/Libs/tai-placeholder.js",
                "~/Src/Components/Libs/Fanex.Grid.js",
                "~/Src/Components/Libs/moment.min.js",
                "~/Src/Components/Libs/numeral.min.js",
                "~/Src/Components/Libs/head.js",
                "~/Src/Components/Libs/Fanex.Pagination.js"));

            bundles.Add(new ScriptBundle("~/bundles/signalr").Include(
                "~/Src/Components/Libs/jquery.signalR-1.1.4.min.js"));

            bundles.AddAndInclude(
                "~/NexFB/Src/Less",
                "~/Src/Content/Site/variables.less",
                "~/Src/Content/Site/layout.less",
                "~/Src/App/EmployeeRegistration/employeeRegistration.less",
                "~/Src/App/UserControl/userControl.less",
                "~/Src/App/GroupControl/groupControl.less",
                "~/Src/App/Ordering/ordering.less",
                "~/Src/App/FoodItemManagement/foodItemManagement.less",
                "~/Src/App/FoodMenuManagement/foodMenuManagement.less",
                "~/Src/App/SetMenuManagement/setMenuManagement.less",
                "~/Src/App/MealPlanningManagement/mealPlanningManagement.less",
                "~/Src/App/SideDishManagement/sideDishManagement.less",
                "~/Src/App/AlternativeMenuManagement/alternativeMenuManagement.less",
                "~/Src/App/ShiftMealMapping/shiftMealMapping.less",
                "~/Src/App/GroupControl/groupControl.less",
                "~/Src/App/ManageUserInGroup/manageUserInGroup.less",
                "~/Src/App/ChangePassword/changePassword.less",
                "~/Src/App/Report/report.less",
                "~/Src/App/DeliveryArea/deliveryArea.less",
                "~/Src/App/Employee/employee.less",
                "~/Src/App/MealTimeManagement/mealTimeManagement.less");

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
                "~/Src/App/Authentication/authentication-controller.js",
                "~/Src/App/Ordering/ordering-route.js",
                "~/Src/App/Ordering/ordering-controller.js",
                "~/Src/App/Ordering/editOrder-controller.js",
                "~/Src/App/Ordering/ordering-factory.js",
                "~/Src/App/Ordering/ordering-directive.js",
                "~/Src/App/FoodItemManagement/foodItemManagement-route.js",
                "~/Src/App/FoodItemManagement/foodItemManagement-controller.js",
                "~/Src/App/FoodItemManagement/createFoodItem-controller.js",
                "~/Src/App/FoodItemManagement/editFoodItem-controller.js",
                "~/Src/App/FoodItemManagement/foodItemManagement-factory.js",
                "~/Src/App/FoodItemManagement/foodItemManagement-directive.js",
                "~/Src/App/FoodMenuManagement/foodMenuManagement-route.js",
                "~/Src/App/FoodMenuManagement/foodMenuManagement-controller.js",
                "~/Src/App/FoodMenuManagement/createFoodMenu-controller.js",
                "~/Src/App/FoodMenuManagement/editFoodMenu-controller.js",
                "~/Src/App/FoodMenuManagement/foodMenuManagement-factory.js",
                "~/Src/App/FoodMenuManagement/foodMenuManagement-directive.js",
                "~/Src/App/SetMenuManagement/setMenuManagement-route.js",
                "~/Src/App/SetMenuManagement/setMenuManagement-controller.js",
                "~/Src/App/SetMenuManagement/createSetMenu-controller.js",
                "~/Src/App/SetMenuManagement/editSetMenu-controller.js",
                "~/Src/App/SetMenuManagement/setMenuManagement-factory.js",
                "~/Src/App/SetMenuManagement/setMenuManagement-directive.js",
                "~/Src/App/MealPlanningManagement/mealPlanningManagement-route.js",
                "~/Src/App/MealPlanningManagement/mealPlanningManagement-controller.js",
                "~/Src/App/MealPlanningManagement/createMealPlanning-controller.js",
                "~/Src/App/MealPlanningManagement/editMealPlanning-controller.js",
                "~/Src/App/MealPlanningManagement/mealPlanningManagement-factory.js",
                "~/Src/App/MealPlanningManagement/mealPlanningManagement-directive.js",
                "~/Src/App/SideDishManagement/sideDishManagement-route.js",
                "~/Src/App/SideDishManagement/sideDishManagement-controller.js",
                "~/Src/App/SideDishManagement/sideDishManagement-factory.js",
                "~/Src/App/AlternativeMenuManagement/alternativeMenuManagement-route.js",
                "~/Src/App/AlternativeMenuManagement/alternativeMenuManagement-controller.js",
                "~/Src/App/AlternativeMenuManagement/alternativeMenuManagement-factory.js",
                "~/Src/App/ShiftMealMapping/shiftMealMapping-route.js",
                "~/Src/App/ShiftMealMapping/shiftMealMapping-controller.js",
                "~/Src/App/ShiftMealMapping/shiftMealMapping-factory.js",
                "~/Src/App/Report/report-route.js",
                "~/Src/App/Report/monthlyReportLocal-controller.js",
                "~/Src/App/Report/dailyMainDishReport-controller.js",
                "~/Src/App/Report/employeeOrderHistory-controller.js",
                "~/Src/App/Report/adminOrderHistoryReport-controller.js",
                 "~/Src/App/Report/sideDishReport-controller.js",
                "~/Src/App/Report/monthlyReportPerDay-controller.js",
                "~/Src/App/Report/mealForecastReport-controller.js",
                "~/Src/App/Report/sideDishSummaryReport-controller.js",
                "~/Src/App/Report/viewMenuReport-controller.js",
                "~/Src/App/Report/mealForecastDetailReport-controller.js",
                "~/Src/App/Report/employeeRegistrationDetailReport-controller.js",
                "~/Src/App/Report/monthlyLunchAndDinnerAmountReport-controller.js",
                "~/Src/App/Report/monthlyBreakfastAndSupperAmountReport-controller.js",
                "~/Src/App/Report/report-factory.js",
                "~/Src/App/Report/monthlyReportLocal-directive.js",
                "~/Src/App/Report/dailyMainDishReport-directive.js",
                "~/Src/App/Report/employeeOrderHistory-directive.js",
                "~/Src/App/Report/adminOrderHistoryReport-directive.js",
                "~/Src/App/Report/sideDishReport-directive.js",
                "~/Src/App/Report/monthlyReportPerDay-directive.js",
                "~/Src/App/Report/mealForecastReport-directive.js",
                "~/Src/App/Report/sideDishSummaryReport-directive.js",
                "~/Src/App/Report/viewMenuReport-directive.js",
                "~/Src/App/Report/mealForecastDetailReport-directive.js",
                "~/Src/App/Report/employeeRegistrationDetailReport-directive.js",
                "~/Src/App/GroupControl/groupControl-route.js",
                "~/Src/App/GroupControl/groupControl-controller.js",
                "~/Src/App/GroupControl/createGroup-controller.js",
                "~/Src/App/GroupControl/editGroup-controller.js",
                "~/Src/App/GroupControl/groupControl-factory.js",
                "~/Src/App/GroupControl/groupControl-directive.js",
                "~/Src/App/ManageUserInGroup/mangeUserInGroup-route.js",
                "~/Src/App/ManageUserInGroup/manageUserInGroup-controller.js",
                "~/Src/App/ManageUserInGroup/manageUserInGroup-factory.js",
                "~/Src/App/ManageUserInGroup/manageUserInGroup-directive.js",
                "~/Src/App/Employee/employee-route.js",
                "~/Src/App/Employee/employee-controller.js",
                "~/Src/App/Employee/createSpecialEmployee-controller.js",
                "~/Src/App/Employee/editSpecialEmployee-controller.js",
                "~/Src/App/Employee/employee-factory.js",
                "~/Src/App/Employee/employee-directive.js",
                "~/Src/App/ChangePassword/changePassword-controller.js",
                "~/Src/App/ChangePassword/changePassword-factory.js",
                "~/Src/App/DeliveryArea/deliveryArea-route.js",
                "~/Src/App/DeliveryArea/deliveryArea-controller.js",
                "~/Src/App/DeliveryArea/deliveryArea-factory.js",
                "~/Src/App/MealTimeManagement/mealTimeManagement-route.js",
                "~/Src/App/MealTimeManagement/mealTimeManagement-controller.js",
                "~/Src/App/MealTimeManagement/mealTimeManagement-factory.js"));

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