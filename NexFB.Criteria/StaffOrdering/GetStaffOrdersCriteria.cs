namespace NexFB.Criteria.StaffOrdering
{
    using Fanex.Data.Repository;
    using System;

    public class GetStaffOrdersCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "SysId")]
        public int UserId { get; set; }

        public DateTime OnDate { get; set; }

        public override string GetSettingKey()
        {
            return "MealOrder_Get_StaffOrderSysID";
        }

        public override bool IsValid()
        {
            return UserId > 0;
        }
    }
}
