// ----------------------------------------------------------------------
// <copyright file="GetEmployeeOrderHistoryCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="12/22/2014 1:03:33 PM">Created.</change>
// </history>

namespace NexFB.Criteria.Report
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Get employee order history criteria.
    /// </summary>
    public class GetEmployeeOrderHistoryCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "SysID")]
        public int UserId { get; set; }

        public int OrderBy { get; set; }

        public int MyMenu { get; set; }

        public int OrderPlatform { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string MealId { get; set; }

        public string StatusId { get; set; }

        public override string GetSettingKey()
        {
            return "Rpt_Get_MyOrderHistory";
        }

        public override bool IsValid()
        {
            return UserId > 0 && FromDate >= ToDate;
        }
    }
}
