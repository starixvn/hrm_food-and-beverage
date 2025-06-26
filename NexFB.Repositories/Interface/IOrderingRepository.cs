namespace NexFB.Repositories.Interface
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using NexFB.Models.Entities;
    using NexFB.Models.Ordering;

    public interface IOrderingRepository
    {
        Menu GetMenuByUserId(ICriteria criteria);

        IList<MealOrderingModel> GetMyOrderByCurrentDay(ICriteria criteria, int userId);

        MealOrderingModel GetMemberNewOrder(ICriteria criteria);

        MealOrderingModel GetMemberSavedOrder(ICriteria criteria);

        MealOrderingModel GetOrderById(ICriteria criteria);
    }
}
