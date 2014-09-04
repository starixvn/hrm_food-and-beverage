// ----------------------------------------------------------------------
// <copyright file="IMealRegistrationRepository.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/4/2014 2:57:14 PM">Created.</change>
// </history>

namespace NexFB.Repositories.Interface
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using NexFB.Models.MealRegistration;

    /// <summary>
    /// The interface of Meal Registration repository.
    /// </summary>
    public interface IMealRegistrationRepository
    {
        IList<MealDateModel> GetRegisterMealByRangeDate(ICriteria criteria);
    }
}
