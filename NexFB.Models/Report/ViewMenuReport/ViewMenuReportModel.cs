// --------------------------------------------------------------------------
// <copyright file="ViewMenuReportModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Larry.Tran" date="2015.1.19">Created</change>
// </history>

namespace NexFB.Models.Report.ViewMenuReport
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// ViewMenuReportModel class.
    /// </summary>
    public class ViewMenuReportModel
    {
        public IEnumerable<DateTime> Dates { get; set; }

        public List<ViewMenuItemModel> ViewMenuItems { get; set; }

        public List<MenuTypeModel> Menus { get; set; }
    }
}