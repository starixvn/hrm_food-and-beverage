// --------------------------------------------------------------------------
// <copyright file="KitchenService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.9.1">Created</change>
// </history>

namespace NexFB.Services.Implement
{
    using System.Collections.Generic;
    using Fanex.Common.Helper;
    using NexFB.Criteria.Kitchen;
    using NexFB.Models.Kitchen;
    using NexFB.Repositories.Implement;
    using NexFB.Repositories.Interface;
    using NexFB.Services.Interface;

    /// <summary>
    /// Kitchen Service.
    /// </summary>
    public class KitchenService : IKitchenService
    {
        private IKitchenRepository _kitchenRepository;

        public KitchenService()
            : this(null)
        {
        }

        public KitchenService(IKitchenRepository kitchenRepository)
        {
            _kitchenRepository = kitchenRepository ?? new KitchenRepository();
        }

        public List<Order> GetWaitngMealOrder(GetWaitingMealOrderCriteria criteria)
        {
            List<Order> waitingMeals = null;
            Check.Argument.IsNotNull(criteria, "criteria");
            if (criteria.IsValid())
            {
                waitingMeals = this._kitchenRepository.GetMealOrder(criteria);
            }

            return waitingMeals;
        }

        public List<Order> GetDeliveredMealOrder(GetDeliveredMealOrderCriteria criteria)
        {
            List<Order> waitingMeals = null;
            Check.Argument.IsNotNull(criteria, "criteria");
            if (criteria.IsValid())
            {
                waitingMeals = this._kitchenRepository.GetMealOrder(criteria);
            }

            return waitingMeals;
        }
    }
}
