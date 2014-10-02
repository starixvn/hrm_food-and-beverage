// ----------------------------------------------------------------------
// <copyright file="ICommonRepository.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="10/2/2014 3:20:48 PM">Created.</change>
// </history>

namespace NexFB.Repositories.Interface
{
    using System.Collections.Generic;
    using NexFB.Models.Employee;
    using NexFB.Models.MealRegistration;

    /// <summary>
    /// The interface Common repository.
    /// </summary>
    public interface ICommonRepository
    {
        IList<MealModel> GetMealsOfMenu(int menuId, IList<dynamic> rawMeals);

        IList<EmployeeTypeModel> GetEmployeeTypeOfMenu(int menuId, IList<dynamic> rawEmployeeTypes);
    }
}
