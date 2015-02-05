// -----------------------------------------------------------------------
// <copyright file="IMealTimeService.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Tim.Trinh" date="2015/2/5">Created.</change>
// </history>

namespace NexFB.Services.Interface
{
    using NexFB.Core.Models;
    using NexFB.Criteria.MealTimeManagement;

    /// <summary>
    /// The interface of meal time service.
    /// </summary>
    public interface IMealTimeService
    {
        CommandResult UpdateMealPlanning(UpdateMealTimeCriteria criteria);
    }
}