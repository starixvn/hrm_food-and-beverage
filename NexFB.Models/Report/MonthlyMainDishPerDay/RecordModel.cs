// ----------------------------------------------------------------------
// <copyright file="RecordModel.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="12/30/2014 12:58:14 PM">Created.</change>
// </history>

namespace NexFB.Models.Report.MonthlyMainDishPerDay
{
    using System.Collections.Generic;

    /// <summary>
    /// The record model.
    /// </summary>
    public class RecordModel
    {
        public IEnumerable<object> Records { get; set; }

        public int RowIndex { get; set; }

        public List<string> DaysOfRedOrder { get; set; }
        public List<string> DaysOfGreenOrder { get; set; }
    }
}
