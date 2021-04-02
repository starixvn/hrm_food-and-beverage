namespace NexFB.Criteria.StaffOrdering
{
    using Fanex.Data.Repository;
    using System;
    public class GetMemberNewOrderByStaffCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "SysId")]
        public int UserId { get; set; }
        public int MealId { get; set; }
        public DateTime OnDate { get; set; }

        public override string GetSettingKey()
        {
            return "Staff_Get_MealOrder";
        }

        public override bool IsValid()
        {
            return UserId > 0 && MealId > 0;
        }
    }
}
