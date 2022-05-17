// ----------------------------------------------------------------------
// <copyright file="IMealRegistrationService.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/4/2014 3:26:29 PM">Created.</change>
// </history>

namespace NexFB.Services.Interface
{
    using System.Collections.Generic;
    using NexFB.Core.Models;
    using NexFB.Criteria.MealRegistration;
    using NexFB.Models.MealRegistration;

    /// <summary>
    /// The interface of meal registration service.
    /// </summary>
    public interface IMealRegistrationService
    {
        IList<MealRegisterModel> GetRegisterMealByRangeDate(GetRegisterMealByRangeDateCriteria criteria);

        CommandResult RegisterMealByRangeDate(RegisterMealByRangeDateCriteria criteria);

        IList<MealDateModel> GetDefaultRegisterMeal(GetRegisterMealByDefaultCriteria criteria);
        EmployeeRegisterModel GetEmployeeRegisterByWorkDateAndMealId(GetEmployeeRegisterByWorkDateAndMealIdCriteria criteria);

        CommandResult RegisterMealByDefault(SetRegisterMealByDefaultCriteria criteria);

        CommandResult ApplyRegisterMealFromTemplate(ApplyRegisterMealFromTemplateCriteria criteria);

        RegisterMealSetting GetRegisterMealSetting(GetRegisterMealSettingCriteria criteria);
    }
}
