using System.Text;
using Fanex.Data;
using Fanex.Data.Repository;
using NexFB.WorkerService.Models;

namespace NexFB.WorkerService.Criteria;

public class SyncTeamCriteria : CriteriaBase
{
    private const string _TeamTpl = "<Section SectionId='{0}' CompanyId='{1}' Code='{2}' "
                                       + "Name='{3}' DepartmentId='{4}' IsActive='{5}' ModifiedDate='{6}' IsDeleted='{7}'></Section>";

    [SpParam(Ignored = true)]
    public int CompanyId { get; set; }

    [SpParam(Ignored = true)]
    public IList<Team> Teams { get; set; }

    public string SectionXML
    {
        get
        {
            var value = new StringBuilder("<ROOT>");

            foreach (var team in Teams)
            {
                value.AppendFormat(
                    _TeamTpl,
                    team.TeamId,
                    CompanyId,
                    team.Code,
                    team.Name,
                    team.DepartmentId,
                    true,
                    team.ModifiedDate,
                    team.IsDeleted);
            }

            return value.Append("</ROOT>").ToString();
        }
    }

    public override string GetSettingKey()
    {
        return "API_SYN_Section";
    }

    public override bool IsValid()
    {
        return CompanyId > 0 && Teams.Any();
    }
}