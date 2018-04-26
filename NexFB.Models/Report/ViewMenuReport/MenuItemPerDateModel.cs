namespace NexFB.Models.Report.ViewMenuReport
{
    using System;
    using System.Collections.Generic;

    public class MenuItemPerDateModel
    {
        public DateTime OnDate { get; set; }

        public IList<ViewMenuItemModel> Menus { get; set; }
    }
}