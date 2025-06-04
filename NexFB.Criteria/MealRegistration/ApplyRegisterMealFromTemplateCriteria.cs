namespace NexFB.Criteria.MealRegistration
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Apply register meal from template criteria.
    /// </summary>
    public class ApplyRegisterMealFromTemplateCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "SysId")]
        public int UserId { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "RegisterMeal_ApplyFromTemplate";
        }

        public override bool IsValid()
        {
            return UserId > 0;
        }
    }
}
