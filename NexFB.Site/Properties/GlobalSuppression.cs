// ----------------------------------------------------------------------
// <copyright file="GlobalSuppression.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/3/2014 1:03:44 PM">Created.</change>
// </history>

using System.Diagnostics.CodeAnalysis;

[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member",
    Target = "NexFB.Site.Controllers.API.CommonController.#GetAllDepartment()", Justification = "Reviewed. Suppression is OK here.")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member",
    Target = "NexFB.Site.Controllers.API.UserControlController.#GetAllSystemRole()", Justification = "Reviewed. Suppression is OK here.")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member",
    Target = "NexFB.Site.Controllers.API.UserControlController.#GetManageRolePermission()", Justification = "Reviewed. Suppression is OK here.")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member",
    Target = "NexFB.Site.Controllers.API.OrderingController.#GetAddNew()", Justification = "Reviewed. Suppression is OK here.")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member",
    Target = "NexFB.Site.Controllers.API.OrderingController.#GetMyOrder()", Justification = "Reviewed. Suppression is OK here.")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member",
    Target = "NexFB.Site.Controllers.API.OrderingController.#GetMyMenu()", Justification = "Reviewed. Suppression is OK here.")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member",
    Target = "NexFB.Site.Controllers.API.OrderingController.#GetSavedOrder()", Justification = "Reviewed. Suppression is OK here.")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member",
    Target = "NexFB.Site.Controllers.API.OrderingController.#GetCurrentUser()", Justification = "Reviewed. Suppression is OK here.")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member",
    Target = "NexFB.Site.Controllers.API.UserControlController.#GetManageUserPermission()", Justification = "Reviewed. Suppression is OK here.")]
[module: SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", Scope = "namespace",
    Target = "NexFB.Web.Services.Interface", MessageId = "Interface", Justification = "Reviewed. Suppression is OK here.")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member",
    Target = "NexFB.Site.Controllers.API.CommonController.#GetAllMealTimesheet()", Justification = "Reviewed. Suppression is OK here.")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member",
    Target = "NexFB.Site.Controllers.API.MealRegistrationController.#GetDefaultRegisterMeal()", Justification = "Reviewed. Suppression is OK here.")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member",
    Target = "NexFB.Site.Controllers.API.MealRegistrationController.#GetUserPermissionOnRegistration()", Justification = "Reviewed. Suppression is OK here.")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member",
    Target = "NexFB.Site.Controllers.API.FoodMenuManagementController.#GetAllFoodMenuToFilter()", Justification = "Reviewed. Suppression is OK here.")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member",
    Target = "NexFB.Site.Controllers.API.FoodItemManagementController.#GetUserPermissionOnFoodItem()", Justification = "Reviewed. Suppression is OK here.")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member",
    Target = "NexFB.Site.Controllers.API.SetMenuManagementController.#GetUserPermissionOnSetMenu()", Justification = "Reviewed. Suppression is OK here.")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member",
    Target = "NexFB.Site.Controllers.API.CommonController.#GetAllEmployeeType()", Justification = "Reviewed. Suppression is OK here.")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member",
    Target = "NexFB.Site.Controllers.API.FoodMenuManagementController.#GetUserPermissionOnFoodMenu()", Justification = "Reviewed. Suppression is OK here.")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member",
    Target = "NexFB.Site.Controllers.API.MealPlanningController.#GetUserPermissionOnMealPlanning()", Justification = "Reviewed. Suppression is OK here.")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member",
    Target = "NexFB.Site.Controllers.API.SideDishManagementController.#GetUserPermissionOnSideDish()", Justification = "Reviewed. Suppression is OK here.")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member",
    Target = "NexFB.Site.Controllers.API.AlternativeMenuController.#GetUserPermissionOnAlternativeMenu()", Justification = "Reviewed. Suppression is OK here.")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member",
    Target = "NexFB.Site.Controllers.API.ShiftMealMappingController.#GetUserPermissionOnShiftMealMapping()", Justification = "Reviewed. Suppression is OK here.")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member",
    Target = "NexFB.Site.Controllers.API.MealRegistrationController.#GetRegisterMealSetting()", Justification = "Reviewed. Suppression is OK here.")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member",
    Target = "NexFB.Site.Controllers.API.EmployeeController.#GetSpecialUsers()", Justification = "Reviewed. Suppression is OK here.")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member",
    Target = "NexFB.Site.Controllers.API.Report.ReportController.#GetOrderedByEnum()", Justification = "Reviewed. Suppression is OK here.")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member",
    Target = "NexFB.Site.Controllers.API.Report.ReportController.#GetOrderPlatformEnum()", Justification = "Reviewed. Suppression is OK here.")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member",
    Target = "NexFB.Site.Controllers.API.Report.ReportController.#GetOrderStatusEnum()", Justification = "Reviewed. Suppression is OK here.")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member",
    Target = "NexFB.Site.Controllers.API.Report.ReportController.#GetMenuTypeEnum()", Justification = "Reviewed. Suppression is OK here.")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member",
    Target = "NexFB.Site.Controllers.API.Report.ReportController.#GetDepartmentByUserRole()", Justification = "Reviewed. Suppression is OK here.")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member",
    Target = "NexFB.Site.Controllers.API.DeliveryAreaController.#GetUserPermissionOnDeliveryArea()", Justification = "Reviewed. Suppression is OK here.")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member",
    Target = "NexFB.Site.Controllers.API.Report.ReportController.#GetDayType()", Justification = "Reviewed. Suppression is OK here.")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member",
    Target = "NexFB.Site.Controllers.API.EmployeeController.#GetUserPermissionOnSpecialEmployee()", Justification = "Reviewed. Suppression is OK here.")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member",
    Target = "NexFB.Site.Controllers.API.CommonController.#GetDepartmentOfCurrentUser()", Justification = "Reviewed. Suppression is OK here.")]