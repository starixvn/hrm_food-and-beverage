// ----------------------------------------------------------------------
// <copyright file="MealRegistrationController.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="8/22/2014 2:42:38 PM">Created.</change>
// </history>

namespace NexFB.Site.Controllers.API
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;
    using NexFB.Core.Constants;
    using NexFB.Core.Enums;
    using NexFB.Core.Models;
    using NexFB.Criteria.MealRegistration;
    using NexFB.Models.MealRegistration;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;
    using NexFB.Web.Attributes;

    /// <summary>
    /// Registration Meal controller.
    /// </summary>
    public class MealRegistrationController : BaseController
    {
        private readonly IMealRegistrationService _mealRegistrationService;

        public MealRegistrationController()
            : this(null)
        {
        }

        public MealRegistrationController(IMealRegistrationService mealRegistrationService)
        {
            _mealRegistrationService = mealRegistrationService ?? new MealRegistrationService();
        }

        /// <summary>
        /// Get register meal by range date.
        /// </summary>
        /// <param name="systemId">The system Id.</param>
        /// <param name="fromDate">The from date.</param>
        /// <param name="toDate">The to date.</param>
        /// <param name="mealId">The meal Id.</param>
        /// <returns>The list meal date model.</returns>
        public IList<MealRegisterModel> GetRegisterMealByRangeDate(
            int systemId,
            DateTime fromDate,
            DateTime toDate,
            short? mealId = null)
        {
            var criteria = new GetRegisterMealByRangeDateCriteria
            {
                SystemId = systemId,
                MealId = mealId,
                FromDate = fromDate,
                ToDate = toDate
            };

            return _mealRegistrationService.GetRegisterMealByRangeDate(criteria);
        }

        /// <summary>
        /// Register meal by range date.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        [HttpPost]
        [ApiAuthorize(FunctionKey = FBFunctionKey.Registration, Policy = Policy.Host)]
        public CommandResult RegisterMealByRangeDate(RegisterMealByRangeDateCriteria criteria)
        {
            if (criteria != null)
            {
                criteria.SystemId = this.CurrentUser.UserId;
                criteria.ModifiedBy = this.CurrentUser.UserId;
                criteria.ModifiedDate = DateTime.Now;
            }

            return _mealRegistrationService.RegisterMealByRangeDate(criteria);
        }
    }
}
