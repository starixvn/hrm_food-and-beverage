// ----------------------------------------------------------------------
// <copyright file="IMealPlanningRepository.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="10/2/2014 2:27:34 PM">Created.</change>
// </history>

namespace NexFB.Repositories.Interface
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using NexFB.Models.FoodMealPlanning;

    /// <summary>
    /// The interface of Meal Planning repository.
    /// </summary>
    public interface IMealPlanningRepository
    {
        IEnumerable<MealPlanningModel> GetMealPlanningByOnDate(ICriteria criteria);
    }
}
