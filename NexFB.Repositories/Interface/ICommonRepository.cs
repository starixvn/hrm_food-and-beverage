namespace NexFB.Repositories.Interface
{
    using System;
    using System.Collections.Generic;
    using NexFB.Models.Employee;
    using NexFB.Models.MealRegistration;

    /// <summary>
    /// The interface Common repository.
    /// </summary>
    public interface ICommonRepository
    {
        IList<MealModel> GetMealsOfMenu(int menuId, IList<dynamic> rawMeals, DateTime? mealDate = null);

        IList<EmployeeTypeModel> GetEmployeeTypeOfMenu(int menuId, IList<dynamic> rawEmployeeTypes, DateTime? mealDate = null);
    }
}
