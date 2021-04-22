using Fanex.Common.Helper;
using NexFB.Core.Models;
using NexFB.Core.Services;
using NexFB.Criteria.StaffOrdering;
using NexFB.Models.Ordering;
using NexFB.Repositories.Implement;
using NexFB.Repositories.Interface;
using NexFB.Services.Interface;
using System.Collections.Generic;

namespace NexFB.Services.Implement
{
    public class StaffOrderingService : BaseService, IStaffOrderingService
    {

        private readonly IOrderingRepository _orderRepository;

        public StaffOrderingService()
            : this(null)
        {
        }

        public StaffOrderingService(IOrderingRepository orderRepository)
        {
            _orderRepository = orderRepository ?? new OrderingRepository();
        }

        public MealOrderingModel GetMemberNewOrderByStaff(GetMemberNewOrderByStaffCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            var order = new MealOrderingModel();
            if (criteria.IsValid())
            {
                order = _orderRepository.GetMemberNewOrder(criteria);
            }

            if (order == null)
            {
                order = new MealOrderingModel();
            }

            return order;
        }

        public IList<MealOrderingModel> GetStaffOrder(GetStaffOrdersCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            IList<MealOrderingModel> orders = null;
            if (criteria.IsValid())
            {
                orders = _orderRepository.GetMyOrderByCurrentDay(criteria, criteria.UserId);
            }

            return orders;
        }

        public CommandResult InsertOrder(InsertOrderByStaffCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            var result = new CommandResult();
            if (criteria.IsValid())
            {
                result = Repository.Insert(criteria, hasGetOutputParams: true);
            }

            return result;
        }

        public CommandResult UpdateOrderByStaff(UpdateOrderByStaffCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            var result = new CommandResult();
            if (criteria.IsValid())
            {
                result = Repository.Update(criteria);
            }

            return result;
        }

        public CommandResult DeleteOrderByStaff(DeleteOrderByStaffCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            var result = new CommandResult();
            if (criteria.IsValid())
            {
                result = Repository.Delete(criteria);
            }

            return result;
        }
    }
}
