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
