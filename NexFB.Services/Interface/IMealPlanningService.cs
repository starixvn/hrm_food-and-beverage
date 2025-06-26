namespace NexFB.Services.Interface
{
    using System.Collections.Generic;
    using NexFB.Core.Models;
    using NexFB.Criteria.FoodMealPlanning;
    using NexFB.Models.FoodItemManagement;
    using NexFB.Models.FoodMealPlanning;

    /// <summary>
    /// The interface of Meal Planning service.
    /// </summary>
    public interface IMealPlanningService
    {
        IEnumerable<MealPlanningModel> GetMealPlanningByOnDate(GetAllMealPlanningByOnDateCriteria criteria);

        CommandResult InsertMealPlanning(InsertMealPlanningCriteria criteria);

        CommandResult UpdateMealPlanning(UpdateMealPlanningCriteria criteria);

        CommandResult DeleteMealPlanning(DeleteMealPlanningCriteria criteria);

        CommandResult ActivateMealPlanning(ActivateMealPlanningCriteria criteria);

        IEnumerable<FoodItemModel> GetFoodByOnDate(GetFoodMealPlanningByOnDateCriteria criteria);
    }
}
