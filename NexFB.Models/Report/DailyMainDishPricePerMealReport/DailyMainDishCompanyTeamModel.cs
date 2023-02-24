
namespace NexFB.Models.Report.DailyMainDishPricePerMealReport
{
    using System;
    using System.Collections.Generic;
    using NexFB.Models.Entities;

    [Serializable]
    public class DailyMainDishCompanyTeamModel
    {
        public DailyMainDishCompanyTeamModel()
        {
            EmployeeDishes = new List<DailyEmployeeMainDishModel>();
        }

        public Company Company { get; set; }

        public Team Team { get; set; }

        public List<DailyEmployeeMainDishModel> EmployeeDishes { get; set; }
    }
}
