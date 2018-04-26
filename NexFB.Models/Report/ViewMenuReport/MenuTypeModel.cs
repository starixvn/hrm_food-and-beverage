namespace NexFB.Models.Report.ViewMenuReport
{
    using System.Collections.Generic;

    public class MenuTypeModel
    {
        public string MenuName { get; set; }

        public int MaxMenuItemPerMeal { get; set; }

        public IList<MenuItemPerDateModel> MenuForEachDay { get; set; }
    }
}