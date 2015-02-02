// -----------------------------------------------------------------------
// <copyright file="SideDishSummaryReportModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Tim.Trinh" date="2015/1/30">Created</change>
// </history>

namespace NexFB.Models.Report.SideDishSummaryReport
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Side dish summary report model.
    /// </summary>
    [Serializable]
    public class SideDishSummaryReportModel
    {
        public SideDishSummaryReportModel()
        {
            SideDishSummarys = new List<SideDishSummaryItemModel>();
        }

        public List<SideDishSummaryItemModel> SideDishSummarys { get; set; }

        public decimal TotalQuantity { get; set; }

        public decimal TotalAmount { get; set; }

        public int TotalItem { get; set; }
    }
}