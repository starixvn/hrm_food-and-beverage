namespace NexFB.Models.Report.ViewMenuReport
{
    using System;

    /// <summary>
    /// ViewMenuItemModel class.
    /// </summary>
    public class ViewMenuItemModel
    {
        public DateTime OnDate { get; set; }

        public bool IsVegetarian { get; set; }

        public string MealId { get; set; }

        public string MenuName { get; set; }

        public string FoodVNMName { get; set; }

        public string FoodEngName { get; set; }

        public int OrderNo { get; set; }
    }
}
