// ----------------------------------------------------------------------
// <copyright file="MealPlanningController.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/18/2014 3:45:52 PM">Created.</change>
// </history>

namespace NexFB.Site.Controllers.API
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;
    using NexFB.Core.Constants;
    using NexFB.Core.Enums;
    using NexFB.Core.Models;
    using NexFB.Criteria.FoodMealPlanning;
    using NexFB.Models.FoodMealPlanning;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;
    using NexFB.Web.Attributes;
    using NexFB.Web.Services.Implement;
    using NexFB.Web.Services.Interface;

    /// <summary>
    /// Meal planning controller.
    /// </summary>
    public class MealPlanningController : BaseController
    {
        private readonly IMealPlanningService _mealPlanningService;
        private readonly IWebCommonService _webCommonService;

        public MealPlanningController()
            : this(null, null)
        {
        }

        public MealPlanningController(
            IMealPlanningService mealPlanningService,
            IWebCommonService webCommonService)
        {
            _mealPlanningService = mealPlanningService ?? new MealPlanningService();
            _webCommonService = webCommonService ?? new WebCommonService();
        }

        /// <summary>
        /// Get all meal planning.
        /// </summary>
        /// <param name="fromDate">The from date.</param>
        /// <param name="toDate">The to date.</param>
        /// <param name="mealId">The meal Id.</param>
        /// <param name="employeeTypeId">The employee type Id.</param>
        /// <returns>The enumerator Meal Planning model.</returns>
        [ApiAuthorize(FunctionKey = FBFunctionKey.MealPlanningManagement, Policy = Policy.Read)]
        public IEnumerable<MealPlanningModel> GetAllMealPlanning(
            DateTime fromDate,
            DateTime toDate,
            string mealId = "",
            string employeeTypeId = "")
        {
            var criteria = new GetAllMealPlanningByOnDateCriteria
            {
                FromDate = fromDate,
                ToDate = toDate,
                MealId = mealId,
                EmployeeTypeId = employeeTypeId
            };

            return _mealPlanningService.GetMealPlanningByOnDate(criteria);
        }

        /// <summary>
        /// Insert meal planning.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        [HttpPost]
        [ApiAuthorize(FunctionKey = FBFunctionKey.MealPlanningManagement, Policy = Policy.Host)]
        public CommandResult InsertMealPlanning(InsertMealPlanningCriteria criteria)
        {
            if (criteria != null)
            {
                criteria.CreatedBy = this.CurrentUser.UserId;
                criteria.CreatedDate = DateTime.Now;
            }

            return _mealPlanningService.InsertMealPlanning(criteria);
        }

        /// <summary>
        /// Update meal planning.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        [HttpPut]
        [ApiAuthorize(FunctionKey = FBFunctionKey.MealPlanningManagement, Policy = Policy.Host)]
        public CommandResult UpdateMealPlanning(UpdateMealPlanningCriteria criteria)
        {
            if (criteria != null)
            {
                criteria.ModifiedBy = this.CurrentUser.UserId;
                criteria.ModifiedDate = DateTime.Now;
            }

            return _mealPlanningService.UpdateMealPlanning(criteria);
        }

        /// <summary>
        /// Delete meal planning.
        /// </summary>
        /// <param name="menuId">The menu Id.</param>
        /// <param name="date">The date has meal planning.</param>
        /// <returns>The command result.</returns>
        [ApiAuthorize(FunctionKey = FBFunctionKey.MealPlanningManagement, Policy = Policy.Host)]
        public CommandResult DeleteMealPlanning(int menuId, DateTime date)
        {
            var criteria = new DeleteMealPlanningCriteria
            {
                MenuId = menuId,
                OnDate = date,
                ModifiedBy = this.CurrentUser.UserId,
                ModifiedDate = DateTime.Now
            };

            return _mealPlanningService.DeleteMealPlanning(criteria);
        }

        /// <summary>
        /// Activate or deactivate meal planning.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        [HttpPut]
        [ApiAuthorize(FunctionKey = FBFunctionKey.MealPlanningManagement, Policy = Policy.Host)]
        public CommandResult ActivateMealPlanning(ActivateMealPlanningCriteria criteria)
        {
            if (criteria != null)
            {
                criteria.ModifiedBy = this.CurrentUser.UserId;
                criteria.ModifiedDate = DateTime.Now;
            }

            return _mealPlanningService.ActivateMealPlanning(criteria);
        }

        public int GetUserPermissionOnMealPlanning()
        {
            int policy = _webCommonService.GetPermission(FBFunctionKey.MealPlanningManagement);
            return policy;
        }
    }
}