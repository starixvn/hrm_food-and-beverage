using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexFB.Models.Report.MonthlyMainDishPerDay
{

    [Serializable]
    public class BreakRuleOrder
    {
        public DateTime OnDate { get; set; }
        public int MealID { get; set; }
        public int SysId { get; set; }
        public bool IsBreakRule { get; set; }
    }
}
