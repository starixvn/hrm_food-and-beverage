namespace NexFB.Models.Report.MealForecastReport
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class EmployeeRegistrationReportModel
    {
        public EmployeeRegistrationReportModel()
        {
            EmployeeRegistrations = new List<EmployeeRegistrationModel>();
            EmployeeNonRegistrations = new List<EmployeeRegistrationModel>();
            TotalMeal = new TotalMealForecastModel();
        }

        public IList<EmployeeRegistrationModel> EmployeeRegistrations { get; set; }
        public IList<EmployeeRegistrationModel> EmployeeNonRegistrations { get; set; }

        public TotalMealForecastModel TotalMeal { get; set; }

        public int TotalItem { get; set; }
    }
}