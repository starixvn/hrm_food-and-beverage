// ----------------------------------------------------------------------
// <copyright file="DeliveryAreaService.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="12/17/2014 8:39:16 AM">Created.</change>
// </history>

namespace NexFB.Services.Implement
{
    using System.Collections.Generic;
    using Fanex.Common.Helper;
    using NexFB.Core.Models;
    using NexFB.Core.Services;
    using NexFB.Criteria.DeliveryArea;
    using NexFB.Models.Entities;
    using NexFB.Services.Interface;

    /// <summary>
    /// The implement of Delivery Area service.
    /// </summary>
    public class DeliveryAreaService : BaseService, IDeliveryAreaService
    {
        /// <summary>
        /// Get all delivery area.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The enumerator Delivery Area model.</returns>
        public IEnumerable<Location> GetAllDeliveryArea(GetAllDeliveryAreaCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            IEnumerable<Location> deliveryAreas = null;

            if (criteria.IsValid())
            {
                deliveryAreas = GetData<Location>(criteria);
            }

            return deliveryAreas;
        }

        /// <summary>
        /// Insert delivery area.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        public CommandResult InsertDeliveryArea(InsertDeliveryAreaCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            CommandResult commandResult = new CommandResult();

            if (criteria.IsValid())
            {
                commandResult = Repository.Insert(criteria);
            }

            return commandResult;
        }

        /// <summary>
        /// Update delivery area.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        public CommandResult UpdateDeliveryArea(UpdateDeliveryAreaCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            CommandResult commandResult = new CommandResult();

            if (criteria.IsValid())
            {
                commandResult = Repository.Update(criteria);
            }

            return commandResult;
        }

        /// <summary>
        /// Activate or deactivate delivery area.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        public CommandResult ActivateDeliveryArea(ActivateDeliveryAreaCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            CommandResult commandResult = new CommandResult();

            if (criteria.IsValid())
            {
                commandResult = Repository.Update(criteria);
            }

            return commandResult;
        }

        /// <summary>
        /// Delete delivery area.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        public CommandResult DeleteDeliveryArea(DeleteDeliveryAreaCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            CommandResult commandResult = new CommandResult();

            if (criteria.IsValid())
            {
                commandResult = Repository.Delete(criteria);
            }

            return commandResult;
        }
    }
}
