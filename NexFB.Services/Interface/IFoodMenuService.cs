namespace NexFB.Services.Interface
{
    using System.Collections.Generic;
    using NexFB.Core.Models;
    using NexFB.Criteria.FoodMenuManagement;
    using NexFB.Models.FoodMenuManagement;

    /// <summary>
    /// The interface of food menu service.
    /// </summary>
    public interface IFoodMenuService
    {
        IList<FoodMenuModel> GetAllFoodMenu(GetAllFoodMenuCriteria criteria);

        IEnumerable<FoodMenuModel> GetAllFoodMenuToFilter(GetAllFoodMenuCriteria criteria);

        IEnumerable<FoodMenuHistoryModel> GetFoodMenuHistory(int menuId);

        CommandResult InsertFoodMenu(InsertFoodMenuCriteria criteria);

        CommandResult DeleteFoodMenu(DeleteFoodMenuByMenuIdCriteria criteria);

        CommandResult UpdateFoodMenu(UpdateFoodMenuCriteria criteria);

        CommandResult ActivateFoodMenu(ActivateFoodMenuCriteria criteria);

        CommandResult InsertFoodMenuHistory(InsertFoodMenuHistoryCriteria criteria);
    }
}
