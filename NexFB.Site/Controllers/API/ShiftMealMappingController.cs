// ----------------------------------------------------------------------
// <copyright file="ShiftMealMappingController.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="10/14/2014 1:19:41 PM">Created.</change>
// </history>

using NexFB.Models.Ordering;

namespace NexFB.Site.Controllers.API
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;
    using NexFB.Core.Constants;
    using NexFB.Core.Enums;
    using NexFB.Core.Models;
    using NexFB.Criteria.ShiftMealMapping;
    using NexFB.Models.ShiftMealMapping;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;
    using NexFB.Web.Attributes;
    using NexFB.Web.Services.Implement;
    using NexFB.Web.Services.Interface;

    /// <summary>
    /// Shift-Meal mapping controller.
    /// </summary>
    public class ShiftMealMappingController : BaseController
    {
        private readonly IShiftMealMappingService _shiftMealMappingService;
        private readonly IWebCommonService _webCommonService;

        public ShiftMealMappingController()
            : this(null, null)
        {
        }

        public ShiftMealMappingController(
            IShiftMealMappingService shiftMealMappingService,
            IWebCommonService webCommonService)
        {
            _shiftMealMappingService = shiftMealMappingService ?? new ShiftMealMappingService();
            _webCommonService = webCommonService ?? new WebCommonService();
        }

        [ApiAuthorize(FunctionKey = FBFunctionKey.MealRegistrationByShift, Policy = Policy.Read)]
        public IEnumerable<ShiftMealMappingModel> GetAllShiftMeal(string departmentId = null, string teamCode = null, string shiftCode = null)
        {
            var criteria = new GetAllShiftMealMappingCriteria
            {
                DepartmentId = departmentId,
                TeamCode = teamCode,
                ShiftCode = shiftCode,
            };

            return _shiftMealMappingService.GetAllShiftMeal(criteria);
        }

        [ApiAuthorize(FunctionKey = FBFunctionKey.MealRegistrationByShift, Policy = Policy.Read)]
        public IEnumerable<ShiftMealByUserIdAndMealIdModel> GetAllShiftMealByUserIdAndMealId(int mealId = 0, string userId = null)
        {
            var today = DateTime.Now;
            var fd = new DateTime();
            var sd = new DateTime();


            if (mealId == 4)
            {
                fd = new DateTime(today.Year, today.Month, today.Day);
                sd = fd.AddDays(1);
            }
            else
            {
                sd = new DateTime(today.Year, today.Month, today.Day);
                fd = sd.AddDays(-1);
            }

            var criteria = new GetShiftMealByUserIdAndMealIdCriteria
            {
                MealId = mealId,
                FirstDate = fd,
                SecondDate = sd,
                UserId = userId
            };

            return _shiftMealMappingService.GetAllShiftMealByUserIdAndMealId(criteria);
        }

        [ApiAuthorize(FunctionKey = FBFunctionKey.MealRegistrationByShift, Policy = Policy.Host)]
        [HttpPost]
        public CommandResult UpdateShiftMeal(UpdateShiftMealByShiftIdCriteria criteria)
        {
            if (criteria != null)
            {
                criteria.ModifiedBy = this.CurrentUser.UserId;
                criteria.ModifiedDate = DateTime.Now;
            }

            return _shiftMealMappingService.UpdateShiftMeal(criteria);
        }

        public int GetUserPermissionOnShiftMealMapping()
        {
            int policy = _webCommonService.GetPermission(FBFunctionKey.MealRegistrationByShift);
            return policy;
        }
    }
}