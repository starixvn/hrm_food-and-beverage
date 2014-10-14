// ----------------------------------------------------------------------
// <copyright file="IShiftMealMappingService.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="10/14/2014 1:06:14 PM">Created.</change>
// </history>

namespace NexFB.Services.Interface
{
    using System.Collections.Generic;
    using NexFB.Core.Models;
    using NexFB.Criteria.ShiftMealMapping;
    using NexFB.Models.ShiftMealMapping;

    /// <summary>
    /// The interface of Shift-Meal mapping service.
    /// </summary>
    public interface IShiftMealMappingService
    {
        IEnumerable<ShiftMealMappingModel> GetAllShiftMeal(GetAllShiftMealMappingCriteria criteria);

        CommandResult UpdateShiftMeal(UpdateShiftMealByShiftIdCriteria criteria);
    }
}
