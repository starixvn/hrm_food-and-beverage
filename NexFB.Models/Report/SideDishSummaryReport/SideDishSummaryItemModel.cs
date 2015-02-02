// -----------------------------------------------------------------------
// <copyright file="SideDishSummaryItemModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Tim.Trinh" date="2015/1/30">Created</change>
// </history>

namespace NexFB.Models.Report.SideDishSummaryReport
{
    using System;

    /// <summary>
    /// Side dish summary item model.
    /// </summary>
    [Serializable]
    public class SideDishSummaryItemModel
    {
        public int SysId { get; set; }

        public string DepartmentTeamName { get; set; }

        public string FullName { get; set; }

        public string NickName { get; set; }

        public string EmpType { get; set; }

        public decimal Qty { get; set; }

        public decimal Amount { get; set; }
    }
}