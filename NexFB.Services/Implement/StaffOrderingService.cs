using Fanex.Common.Helper;
using NexFB.Core.Models;
using NexFB.Core.Services;
using NexFB.Criteria.StaffOrdering;
using NexFB.Models.Ordering;
using NexFB.Repositories.Implement;
using NexFB.Repositories.Interface;
using NexFB.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
