namespace NexFB.Services.Implement
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common.Helper;
    using NexFB.Core.Models;
    using NexFB.Core.Services;
    using NexFB.Criteria.SmartCardOrder;
    using NexFB.Models.Kitchen;
    using NexFB.Models.SmartCardOrder;
    using NexFB.Services.Interface;

    public class SmartCardOrderService : BaseService, ISmartCardOrderService
    {
        public EmployeeInfo GetEmployeeByCardId(GetEmployeeByCardNumberCriteria criteria)
        {
            EmployeeInfo employeeInfo = null;
            if (criteria.IsValid())
            {
                employeeInfo = this.GetDataEntity<EmployeeInfo>(criteria);
            }

            return employeeInfo;
        }

        public List<FoodOrder> GetAlternativeFoodBySysId(GetAlternativeFoodBySysIdCriteria criteria)
        {
            List<FoodOrder> alternativeFoods = null;
            if (criteria.IsValid())
            {
                alternativeFoods = this.GetData<FoodOrder>(criteria).ToList();
            }

            return alternativeFoods;
        }

        public List<FoodOrder> GetSideFoodBySysId(GetSideFoodBySysIdCriteria criteria)
        {
            List<FoodOrder> sideFoods = null;
            if (criteria.IsValid())
            {
                sideFoods = this.GetData<FoodOrder>(criteria).ToList();
            }

            return sideFoods;
        }

        public OrderResult InsertOrderFood(InsertOrderMealByUserCriteria criteria)
        {
            var result = new OrderResult
            {
                CommandResult = new CommandResult()
            };
            if (criteria.IsValid())
            {
                result.CommandResult = this.Repository.Insert(criteria, hasGetOutputParams: true);
            }

            if (result.CommandResult.Code == 0)
            {
                result.OrderId = criteria.OrderId;
                result.OrderNo = criteria.OrderNo;
            }

            return result;
        }

        public OrderResult InsertOrderSideFood(InsertOrderSideDishesByUserCriteria criteria)
        {
            var result = new OrderResult
            {
                CommandResult = new CommandResult()
            };
            if (criteria.IsValid())
            {
                result.CommandResult = this.Repository.Insert(criteria, hasGetOutputParams: true);
            }

            if (result.CommandResult.Code == 0)
            {
                result.OrderId = criteria.OrderId;
                result.OrderNo = criteria.OrderNo;
            }

            return result;
        }

        public List<SCEmployeeOrder> GetMyOrderBySysId(GetMyOrderBySysIdCriteria criteria)
        {
            List<SCEmployeeOrder> employeeOrders = null;
            if (criteria.IsValid())
            {
                employeeOrders = this.GetData<SCEmployeeOrder>(criteria).ToList();
            }

            return employeeOrders;
        }

        public List<SCMainDishEmployeeOrder> GetInfoMainDishOrder(GetInfoMainDishOrderCriteria criteria)
        {
            List<SCMainDishEmployeeOrder> result = null;
            if (criteria.IsValid())
            {
                result = this.GetData<SCMainDishEmployeeOrder>(criteria).ToList();
            }

            return result;
        }
    }
}
