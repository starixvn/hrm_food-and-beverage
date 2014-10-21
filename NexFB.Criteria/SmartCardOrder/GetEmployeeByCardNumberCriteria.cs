// --------------------------------------------------------------------------
// <copyright file="GetEmployeeByCardNumberCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.20">Created</change>
// </history>

namespace NexFB.Criteria.SmartCardOrder
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get Employee By Card Number Criteria.
    /// </summary>
    public class GetEmployeeByCardNumberCriteria : CriteriaBase
    {
        public string CardNumber { get; set; }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(CardNumber);
        }

        public override string GetSettingKey()
        {
            return "Employee_GetEmployeeByCardNumber";
        }
    }
}
