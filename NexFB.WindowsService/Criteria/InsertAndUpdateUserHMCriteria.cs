namespace NexFB.WindowsService.Criteria
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Data.Repository;

    public class InsertAndUpdateUserHMCriteria : CriteriaBase
    {
        private const string _UserTpl =
            "<User EmpId='{0}' Password='{1}' Code='{2}' "
            + "NickName='{3}' FullName='{4}' DomainName='{5}' CompanyId='{6}' IsEnabled='{7}' "
            + " IsDeleted='{8}' JoinedDate='{9}' ModifiedDate='{10}' TeamId='{11}' IsLocal='{12}' "
             + " CardNumber='{13}' CostCenter='{14}'></User>";

        [SpParam(Ignored = true)]
        public int CompanyId { get; set; }

        [SpParam(Ignored = true)]
        public IList<HongMaService.UserModel> Users { get; set; }

        public string UserXml
        {
            get
            {
                var value = new StringBuilder("<ROOT>");

                if (Users != null)
                {
                    foreach (HongMaService.UserModel user in Users)
                    {
                        value.AppendFormat(
                            _UserTpl,
                            user.UserId,
                            user.Password,
                            user.Code,
                            user.NickName,
                            user.FullName,
                            user.DomainName,
                            CompanyId,
                            user.IsEnabled,
                            user.IsDeleted,
                            user.JoinedDate,
                            user.ModifiedDate,
                            user.TeamId,
                            true,
                            user.EmployeeCard,
                            user.CostCenter);
                    }
                }

                return value.Append("</ROOT>").ToString();
            }
        }

        public override string GetSettingKey()
        {
            return "API_SYN_Employee";
        }

        public override bool IsValid()
        {
            return CompanyId > 0 && Users != null && Users.Any();
        }
    }
}
