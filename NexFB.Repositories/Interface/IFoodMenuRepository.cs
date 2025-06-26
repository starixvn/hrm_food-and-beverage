namespace NexFB.Repositories.Interface
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using NexFB.Models.FoodMenuManagement;

    /// <summary>
    /// The interface of food menu repository.
    /// </summary>
    public interface IFoodMenuRepository
    {
        IList<FoodMenuModel> GetAllFoodMenu(ICriteria criteria);

        IEnumerable<FoodMenuModel> GetAllFoodMenuToFilter(ICriteria criteria);

        IEnumerable<FoodMenuHistoryModel> GetFoodMenuHistory(ICriteria criteria);
    }
}
