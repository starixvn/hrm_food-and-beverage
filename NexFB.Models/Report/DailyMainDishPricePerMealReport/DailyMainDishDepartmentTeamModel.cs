
namespace NexFB.Models.Report.DailyMainDishPricePerMealReport
{
    using System;
    using System.Collections.Generic;
    using NexFB.Models.Entities;

    [Serializable]
    public class DailyMainDishDepartmentTeamModel
    {
        public DailyMainDishDepartmentTeamModel()
        {
            EmployeeDishes = new List<DailyEmployeeMainDishModel>();
        }

        public Company Company { get; set; }

        public Team Team { get; set; }

        public List<DailyEmployeeMainDishModel> EmployeeDishes { get; set; }
    }
}
