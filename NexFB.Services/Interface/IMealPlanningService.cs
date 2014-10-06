// ----------------------------------------------------------------------
// <copyright file="IMealPlanningService.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="10/2/2014 3:59:45 PM">Created.</change>
// </history>

namespace NexFB.Services.Interface
{
    using System.Collections.Generic;
    using NexFB.Core.Models;
    using NexFB.Criteria.FoodMealPlanning;
    using NexFB.Models.FoodMealPlanning;

    /// <summary>
    /// The interface of Meal Planning service.
    /// </summary>
    public interface IMealPlanningService
    {
        IEnumerable<MealPlanningModel> GetMealPlanningByOnDate(GetAllMealPlanningByOnDateCriteria criteria);

        CommandResult InsertMealPlanning(InsertMealPlanningCriteria criteria);
    }
}
