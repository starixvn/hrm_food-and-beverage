// ----------------------------------------------------------------------
// <copyright file="MealTimeManagementController.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Tim.Trinh" date="2015/2/5">Created.</change>
// </history>

namespace NexFB.Site.Controllers.API
{
    using System;
    using System.Web.Http;
    using NexFB.Core.Constants;
    using NexFB.Core.Enums;
    using NexFB.Core.Models;
    using NexFB.Criteria.MealTimeManagement;
    using NexFB.Models.MealTimeManagement;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;
    using NexFB.Web.Attributes;
    using NexFB.Web.Services.Implement;
    using NexFB.Web.Services.Interface;

    /// <summary>
    /// Meal time management controller.
    /// </summary>
    public class MealTimeManagementController : BaseController
    {
        private readonly IMealTimeService _mealTimeService;
        private readonly IWebCommonService _webCommonService;

        public MealTimeManagementController()
            : this(null, null)
        {
        }

        public MealTimeManagementController(
            IWebCommonService webCommonService,
            IMealTimeService mealTimeService)
        {
            _webCommonService = webCommonService ?? new WebCommonService();
            _mealTimeService = mealTimeService ?? new MealTimeService();
        }

        /// <summary>
        /// Update meal time.
        /// </summary>
        /// <param name="model">The meal time model.</param>
        /// <returns>The command result.</returns>
        [HttpPut]
        [ApiAuthorize(FunctionKey = FBFunctionKey.MealTimeManagement, Policy = Policy.Host)]
        public CommandResult UpdateMealTime(MealTimeModel model)
        {
            var criteria = new UpdateMealTimeCriteria
            {
                MealId = model.MealId,
                RegisterTime = model.RegisterTime,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                ModifiedBy = this.CurrentUser.UserId,
                ModifiedDate = DateTime.Now
            };

            return _mealTimeService.UpdateMealPlanning(criteria);
        }

        public int GetUserPermissionOnMealTimeManagement()
        {
            int policy = _webCommonService.GetPermission(FBFunctionKey.MealTimeManagement);
            return policy;
        }
    }
}