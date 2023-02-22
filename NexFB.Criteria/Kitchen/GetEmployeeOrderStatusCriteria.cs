namespace NexFB.Criteria.Kitchen
{
    using Fanex.Data.Repository;

    public class GetEmployeeOrderStatusCriteria : CriteriaBase
    {
        public int SysId { get; set; }

        public override bool IsValid()
        {
            return SysId > 0;
        }

        public override string GetSettingKey()
        {
            return "POS_Get_MyStatusBySysID";
        }
    }
}