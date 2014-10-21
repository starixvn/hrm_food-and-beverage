// --------------------------------------------------------------------------
// <copyright file="ISmartCardOrderService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.20">Created</change>
// </history>

namespace NexFB.Services.Interface
{
    using NexFB.Criteria.SmartCardOrder;
    using NexFB.Models.SmartCardOrder;

    /// <summary>
    /// Interface SmartCardOrderService.
    /// </summary>
    public interface ISmartCardOrderService
    {
        SCEmployeeOrder GetEmployeeByCardId(GetEmployeeByCardNumberCriteria criteria);
    }
}
