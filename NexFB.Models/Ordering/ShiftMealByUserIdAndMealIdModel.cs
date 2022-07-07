using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NexFB.Models.Ordering
{
    [Serializable]
    public class ShiftMealByUserIdAndMealIdModel
    {
        public int UserId { get; set; }
        public bool IsAllowOrderOnFirstDate { get; set; }
        public bool IsAllowOrderOnSecondDate { get; set; }
    }
}
