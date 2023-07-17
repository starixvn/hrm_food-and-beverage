namespace NexFB.Models.Report.MealForecastReport
{
    using System;
    using NexFB.Models.Entities;

    [Serializable]
    public class EmployeeRegistrationModel
    {
        public DateTime WorkDate { get; set; }

        public bool? IsVegetarian { get; set; }

        public int ActualOrder { get; set; }

        public string CompanyName { get; set; }

        public string ShiftName { get; set; }

        public MealModel Meal { get; set; }

        public Employee Employee { get; set; }
    }
}
