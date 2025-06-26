namespace NexFB.Repositories.Interface
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using NexFB.Models.FoodItemManagement;

    /// <summary>
    /// The food item management repository.
    /// </summary>
    public interface IFoodItemRepository
    {
        IList<FoodItemModel> GetAllFoodItem(ICriteria criteria);
    }
}
