namespace NexFB.Repositories.Interface
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using NexFB.Models.Kitchen;

    /// <summary>
    /// Kitchen Repository.
    /// </summary>
    public interface IKitchenRepository
    {
        List<Order> GetMealOrder(ICriteria criteria);

        Order GetOrderDetails(ICriteria criteria);

        IList<Order> GetOrderByQuery(ICriteria criteria);

        Order GetMealOrderById(ICriteria criteria);

        EmployeeInGroup GetEmployeeInGroup(ICriteria criteria);
    }
}
