// ----------------------------------------------------------------------
// <copyright file="ShiftMealMappingService.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="10/14/2014 1:08:37 PM">Created.</change>
// </history>

namespace NexFB.Services.Implement
{
    using System.Collections.Generic;
    using Fanex.Common.Helper;
    using NexFB.Core.Models;
    using NexFB.Core.Services;
    using NexFB.Criteria.ShiftMealMapping;
    using NexFB.Models.ShiftMealMapping;
    using NexFB.Repositories.Implement;
    using NexFB.Repositories.Interface;
    using NexFB.Services.Interface;

    /// <summary>
    /// The Shift-Meal mapping service.
    /// </summary>
    public class ShiftMealMappingService : BaseService, IShiftMealMappingService
    {
        private readonly IShiftMealMappingRepository _shiftMealMappingRepository;

        public ShiftMealMappingService()
            : this(null)
        {
        }

        public ShiftMealMappingService(IShiftMealMappingRepository shiftMealMappingRepository)
        {
            _shiftMealMappingRepository = shiftMealMappingRepository ?? new ShiftMealMappingRepository();
        }

        /// <summary>
        /// Get all shift meal to mapping.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The enumerator Shift Meal Mapping model.</returns>
        public IEnumerable<ShiftMealMappingModel> GetAllShiftMeal(GetAllShiftMealMappingCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            IEnumerable<ShiftMealMappingModel> shiftMeals = null;

            if (criteria.IsValid())
            {
                shiftMeals = _shiftMealMappingRepository.GetAllShiftMeal(criteria);
            }

            return shiftMeals;
        }

        /// <summary>
        /// Update shift meal.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        public CommandResult UpdateShiftMeal(UpdateShiftMealByShiftIdCriteria criteria)
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
