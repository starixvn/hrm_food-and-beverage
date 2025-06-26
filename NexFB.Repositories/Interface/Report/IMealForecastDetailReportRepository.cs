namespace NexFB.Repositories.Interface.Report
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using NexFB.Models.Report.MealForecastReport;

    /// <summary>
    /// The interface of Meal forecast detail report repository.
    /// </summary>
    public interface IMealForecastDetailReportRepository
    {
        MealForecastDetailReport GetMealForecastDetailReport(ICriteria criteria);

        IList<EmployeeRegistrationModel> GetEmployeeRegistration(ICriteria criteria);
        IList<EmployeeRegistrationModel> GetEmployeeNonRegistration(ICriteria criteria);
    }
}
