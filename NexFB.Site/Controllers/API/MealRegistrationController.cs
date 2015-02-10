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
    using NexFB.Models.Authority;
    using NexFB.Models.MealRegistration;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;
    using NexFB.Web.Attributes;
    using NexFB.Web.Services.Implement;
    using NexFB.Web.Services.Interface;

    /// <summary>
    /// Registration Meal controller.
    /// </summary>
    public class MealRegistrationController : BaseController
    {
        private readonly IMealRegistrationService _mealRegistrationService;
        private readonly IWebCommonService _webCommonService;

        public MealRegistrationController()
            : this(null, null)
        {
        }

        public MealRegistrationController(
            IMealRegistrationService mealRegistrationService,
            IWebCommonService webCommonService)
        {
            _mealRegistrationService = mealRegistrationService ?? new MealRegistrationService();
            _webCommonService = webCommonService ?? new WebCommonService();
        }

        /// <summary>
        /// Get register meal by range date.
        /// </summary>
        /// <param name="fromDate">The from date.</param>
        /// <param name="toDate">The to date.</param>
        /// <param name="mealId">The meal Id.</param>
        /// <returns>The list meal date model.</returns>
        [ApiAuthorize(FunctionKey = FBFunctionKey.Registration, Policy = Policy.Read)]
        public IList<MealRegisterModel> GetRegisterMealByRangeDate(
            DateTime fromDate,
            DateTime toDate,
            short? mealId = null)
        {
            var criteria = new GetRegisterMealByRangeDateCriteria
            {
                SystemId = this.CurrentUser.UserId,
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

        [ApiAuthorize(FunctionKey = FBFunctionKey.Registration, Policy = Policy.Read)]
        public IList<MealDateModel> GetDefaultRegisterMeal()
        {
            var criteria = new GetRegisterMealByDefaultCriteria
            {
                SystemId = this.CurrentUser.UserId
            };

            return _mealRegistrationService.GetDefaultRegisterMeal(criteria);
        }

        [HttpPost]
        [ApiAuthorize(FunctionKey = FBFunctionKey.Registration, Policy = Policy.Host)]
        public CommandResult RegisterMealByDefault(SetRegisterMealByDefaultCriteria criteria)
        {
            if (criteria != null)
            {
                criteria.UserId = this.CurrentUser.UserId;
                criteria.ModifiedBy = this.CurrentUser.UserId;
                criteria.ModifiedDate = DateTime.Now;
            }

            return _mealRegistrationService.RegisterMealByDefault(criteria);
        }

        [HttpPut]
        [ApiAuthorize(FunctionKey = FBFunctionKey.Registration, Policy = Policy.Host)]
        public CommandResult ApplyRegisterMealFromTemplate()
        {
            var criteria = new ApplyRegisterMealFromTemplateCriteria
            {
                UserId = this.CurrentUser.UserId,
                ModifiedBy = this.CurrentUser.UserId,
                ModifiedDate = DateTime.Now
            };

            return _mealRegistrationService.ApplyRegisterMealFromTemplate(criteria);
        }

        public int GetUserPermissionOnRegistration()
        {
            int policy = _webCommonService.GetPermission(FBFunctionKey.Registration);
            return policy;
        }

        public RegisterMealSetting GetRegisterMealSetting()
        {
            var criteria = new GetRegisterMealSettingCriteria();
            RegisterMealSetting setting = _mealRegistrationService.GetRegisterMealSetting(criteria);

            return setting;
        }

        public UserSession GetCurrentUser()
        {
            return this.CurrentUser;
        }
    }
}