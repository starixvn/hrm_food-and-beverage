

namespace NexFB.Criteria.UserControl
{
    using Fanex.Data.Repository;

    public class AssignFreeOrderToUserCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "SysID")]
        public int UserId { get; set; }

        public bool? IsFreeOrder { get; set; }

        public override string GetSettingKey() => "UserCtrl_SetFreeOrder";

        public override bool IsValid() => UserId > 0;
    }
}
