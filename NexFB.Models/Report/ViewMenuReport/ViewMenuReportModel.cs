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
