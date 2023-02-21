namespace NexFB.Criteria.ShiftMealMapping
{
    using System;
    using Fanex.Data.Repository;

    public class UpdateShiftMealByShiftIdCriteria : CriteriaBase
    {
        public int ShiftId { get; set; }

        public short CompanyId { get; set; }

        [SpParam(SqlParamName = "Meal_S_0")]
        public bool MealPreviousSupper { get; set; }

        [SpParam(SqlParamName = "Meal_B")]
        public bool MealBreakfast { get; set; }

        [SpParam(SqlParamName = "Meal_L")]
        public bool MealLunch { get; set; }

        [SpParam(SqlParamName = "Meal_D")]
        public bool MealDinner { get; set; }

        [SpParam(SqlParamName = "Meal_S")]
        public bool MealSupper { get; set; }

        [SpParam(SqlParamName = "Meal_B_1")]
        public bool MealNextBreakfast { get; set; }

        [SpParam(SqlParamName = "Meal_L_1")]
        public bool MealNextLunch { get; set; }

        [SpParam(SqlParamName = "Meal_D_1")]
        public bool MealNextDinner { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey() => "Shift_Edit_ByShiftID";

        public override bool IsValid() => ShiftId > 0 && CompanyId > 0;
    }
}