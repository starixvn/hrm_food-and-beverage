namespace NexFB.Models.Report.MonthlyMainDishPerDay
{
    using System;

    [Serializable]
    public class BreakRuleOrder
    {
        public DateTime OnDate { get; set; }
        public int MealID { get; set; }
        public int SysId { get; set; }
        public bool IsBreakRule { get; set; }
    }
}
