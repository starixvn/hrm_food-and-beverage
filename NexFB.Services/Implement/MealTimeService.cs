// -----------------------------------------------------------------------
// <copyright file="MealTimeService.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Tim.Trinh" date="2015/2/5">Created.</change>
// </history>

namespace NexFB.Services.Implement
{
    using Fanex.Common.Helper;
    using NexFB.Core.Models;
    using NexFB.Core.Services;
    using NexFB.Criteria.MealTimeManagement;
    using NexFB.Services.Interface;

    /// <summary>
    /// The meal time service.
    /// </summary>
    public class MealTimeService : BaseService, IMealTimeService
    {
        public MealTimeService()
        {
        }

        /// <summary>
        /// Update meal time.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        public CommandResult UpdateMealPlanning(UpdateMealTimeCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            CommandResult commandResult = new CommandResult();

            if (criteria.IsValid())
            {
                commandResult = Repository.Update(criteria);
            }

            return commandResult;
        }
    }
}