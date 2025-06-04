namespace NexFB.Models.Report.DailyMainDishPricePerMealReport
{
    using System;
    using NexFB.Models.Entities;

    /// <summary>
    /// The daily main dish of employee model.
    /// </summary>
    [Serializable]
    public class DailyEmployeeMainDishModel
    {
        public DateTime WorkDate { get; set; }

        public Shift Shift { get; set; }

        public Employee Employee { get; set; }

        public string SectionName { get; set; }
        public string DepartmentName { get; set; }
        public string UnitName { get; set; }
        public string TeamName { get; set; }
        public int Breakfast { get; set; }

        public int Lunch { get; set; }

        public int Dinner { get; set; }

        public int Supper { get; set; }
    }
}
