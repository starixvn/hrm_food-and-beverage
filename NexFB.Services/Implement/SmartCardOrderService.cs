// --------------------------------------------------------------------------
// <copyright file="SmartCardOrderService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.20">Created</change>
// </history>

namespace NexFB.Services.Implement
{
    using Fanex.Common.Helper;
    using NexFB.Core.Services;
    using NexFB.Criteria.SmartCardOrder;
    using NexFB.Models.SmartCardOrder;
    using NexFB.Services.Interface;

    /// <summary>
    /// Smart Card Order Service.
    /// </summary>
    public class SmartCardOrderService : BaseService, ISmartCardOrderService
    {
        public SCEmployeeOrder GetEmployeeByCardId(GetEmployeeByCardNumberCriteria criteria)
        {
            SCEmployeeOrder employeeInfo = null;
            Check.Argument.IsNotNull(criteria, "criteria");
            if (criteria.IsValid())
            {
                employeeInfo = this.GetDataEntity<SCEmployeeOrder>(criteria);
            }

            return employeeInfo;
        }
    }
}
