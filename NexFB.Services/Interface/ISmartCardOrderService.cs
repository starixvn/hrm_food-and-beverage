namespace NexFB.Services.Interface
{
    using System.Collections.Generic;
    using NexFB.Criteria.SmartCardOrder;
    using NexFB.Models.Kitchen;
    using NexFB.Models.SmartCardOrder;

    /// <summary>
    /// Interface SmartCardOrderService.
    /// </summary>
    public interface ISmartCardOrderService
    {
        EmployeeInfo GetEmployeeByCardId(GetEmployeeByCardNumberCriteria criteria);

        List<FoodOrder> GetAlternativeFoodBySysId(GetAlternativeFoodBySysIdCriteria criteria);

        OrderResult InsertOrderFood(InsertOrderMealByUserCriteria criteria);

        OrderResult InsertOrderSideFood(InsertOrderSideDishesByUserCriteria criteria);

        List<FoodOrder> GetSideFoodBySysId(GetSideFoodBySysIdCriteria criteria);
        List<FoodMenu> GetMenuShowOnScreen(GetMenuShowOnScreenCriteria criteria);

        List<SCEmployeeOrder> GetMyOrderBySysId(GetMyOrderBySysIdCriteria criteria);
        List<SCMainDishEmployeeOrder> GetInfoMainDishOrder(GetInfoMainDishOrderCriteria criteria);
    }
}
