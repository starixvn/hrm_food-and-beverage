// ----------------------------------------------------------------------
// <copyright file="MealRegistrationService.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/4/2014 3:29:02 PM">Created.</change>
// </history>

namespace NexFB.Services.Implement
{
    using System.Collections.Generic;
    using Fanex.Common.Helper;
    using NexFB.Core.Models;
    using NexFB.Core.Services;
    using NexFB.Criteria.MealRegistration;
    using NexFB.Models.MealRegistration;
    using NexFB.Repositories.Implement;
    using NexFB.Repositories.Interface;
    using NexFB.Services.Interface;

    /// <summary>
    /// The meal registration service.
    /// </summary>
    public class MealRegistrationService : BaseService, IMealRegistrationService
    {
        private readonly IMealRegistrationRepository _mealRegistrationRepository;

        public MealRegistrationService()
            : this(null)
        {
        }

        public MealRegistrationService(IMealRegistrationRepository mealRegistrationRepository)
        {
            _mealRegistrationRepository = mealRegistrationRepository ?? new MealRegistrationRepository();
        }

        /// <summary>
        /// Get register meal by range date.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The list meal in week model.</returns>
        public IList<MealRegisterModel> GetRegisterMealByRangeDate(GetRegisterMealByRangeDateCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            IList<MealRegisterModel> mealDates = new List<MealRegisterModel>();

            if (criteria.IsValid())
            {
                mealDates = _mealRegistrationRepository.GetRegisterMealByRangeDate(criteria);
            }

            return mealDates;
        }

        /// <summary>
        /// Register meal by range date.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        public CommandResult RegisterMealByRangeDate(RegisterMealByRangeDateCriteria criteria)
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
