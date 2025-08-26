using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.Data.Repository;
using Fanex.Data;
using NexFB.WorkerService.Models;

namespace NexFB.WorkerService.Criteria;

internal class SyncEmployeesSecondmentCriteria : CriteriaBase
{
    private const string UserTpl =
        "<User UserId='{0}' EmpId='{1}' Password='{2}' Code='{3}' "
        + "NickName='{4}' FullName='{5}' DomainName='{6}' CompanyId='{7}' IsEnabled='{8}' "
        + " IsDeleted='{9}' JoinedDate='{10}' ModifiedDate='{11}' TeamId='{12}' IsLocal='{13}' "
        + " CardNumber='{14}' CostCenter='{15}' IsSecondment='{16}'></User>";

    [SpParam(Ignored = true)]
    public int CompanyId { get; set; }

    [SpParam(Ignored = true)]
    public List<Employee> Users { get; set; }

    public string UserXml
    {
        get
        {
            var value = new StringBuilder("<ROOT>");

            foreach (var user in Users)
            {
                value.AppendFormat(
                    UserTpl,
                    user.UserId,
                    user.EmpId,
                    user.Password,
                    user.Code,
                    user.NickName,
                    user.FullName,
                    user.DomainName,
                    CompanyId,
                    user.Enabled,
                    user.IsDeleted,
                    user.JoinedDate,
                    user.ModifiedDate,
                    user.TeamId,
                    user.IsLocal,
                    user.EmployeeCard,
                    user.CostCenter,
                    1);
            }

            return value.Append("</ROOT>").ToString();
        }
    }

    public override string GetSettingKey() => "API_SYN_Employee_Secondment";

    public override bool IsValid() => CompanyId > 0 && Users.Any();
}