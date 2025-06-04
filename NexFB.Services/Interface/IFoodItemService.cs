namespace NexFB.Services.Interface
{
    using System.Collections.Generic;
    using NexFB.Core.Models;
    using NexFB.Criteria.FoodItemManagement;
    using NexFB.Models.FoodItemManagement;

    /// <summary>
    /// The interface of Food Item service.
    /// </summary>
    public interface IFoodItemService
    {
        IList<FoodItemModel> GetAllFoodItem(GetAllFoodItemCriteria criteria);

        CommandResult InsertFoodItem(InsertFoodItemCriteria criteria);

        CommandResult UpdateFoodItem(UpdateFoodItemCriteria criteria);

        CommandResult DeleteFoodItem(DeleteFoodItemCriteria criteria);

        CommandResult UpdateStatusFoodItem(UpdateStatusFoodItemCriteria criteria);

        IEnumerable<FoodItemModel> GetBasicFoodItem(GetBasicFoodItemCriteria criteria);
    }
}
