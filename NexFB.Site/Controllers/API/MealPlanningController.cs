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
    using NexFB.Core.Models;
    using NexFB.Criteria.FoodMealPlanning;
    using NexFB.Models.FoodMealPlanning;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;

    /// <summary>
    /// Meal planning controller.
    /// </summary>
    public class MealPlanningController : BaseController
    {
        private readonly IMealPlanningService _mealPlanningService;

        public MealPlanningController()
            : this(null)
        {
        }

        public MealPlanningController(IMealPlanningService mealPlanningService)
        {
            _mealPlanningService = mealPlanningService ?? new MealPlanningService();
        }

        /// <summary>
        /// Get all meal planning.
        /// </summary>
        /// <param name="fromDate">The from date.</param>
        /// <param name="toDate">The to date.</param>
        /// <param name="mealId">The meal Id.</param>
        /// <param name="employeeTypeId">The employee type Id.</param>
        /// <returns>The enumerator Meal Planning model.</returns>
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
        public CommandResult InsertMealPlanning(InsertMealPlanningCriteria criteria)
        {
            if (criteria != null)
            {
                criteria.CreatedBy = this.CurrentUser.UserId;
                criteria.CreatedDate = DateTime.Now;
            }

            return _mealPlanningService.InsertMealPlanning(criteria);
        }
    }
}