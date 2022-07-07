using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.Data.Repository;

namespace NexFB.Criteria.ShiftMealMapping
{
    public class GetShiftMealByUserIdAndMealIdCriteria : CriteriaBase
    {
        public int MealId { get; set; }
        public DateTime? FirstDate { get; set; }
        public DateTime? SecondDate { get; set; }
        public string UserId { get; set; }
        public override string GetSettingKey()
        {
            return "Shift_Meal_GetByMealID";
        }

        public override bool IsValid()
        {
            return UserId != "" && MealId > 0 && MealId < 5;
        }
    }
}
