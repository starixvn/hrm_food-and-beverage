// ----------------------------------------------------------------------
// <copyright file="IShiftMealMappingRepository.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="10/14/2014 12:35:38 PM">Created.</change>
// </history>

namespace NexFB.Repositories.Interface
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using NexFB.Models.ShiftMealMapping;

    /// <summary>
    /// The interface of Shift-Meal Mapping repository.
    /// </summary>
    public interface IShiftMealMappingRepository
    {
        IEnumerable<ShiftMealMappingModel> GetAllShiftMeal(ICriteria criteria);
    }
}
