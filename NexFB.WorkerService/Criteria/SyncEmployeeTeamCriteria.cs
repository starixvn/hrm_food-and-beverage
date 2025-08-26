using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.Data;
using Fanex.Data.Repository;
using NexFB.WorkerService.Models;

namespace NexFB.WorkerService.Criteria;

internal class SyncEmployeeTeamCriteria : CriteriaBase
{
    private const string TeamTpl = "<Team TeamId='{0}' CompanyId='{1}' Code='{2}' "
        + "Name='{3}' IsDeleted='{4}' ModifiedDate='{5}' SectionId='{6}'></Team>";

    [SpParam(Ignored = true)]
    public int CompanyId { get; set; }

    [SpParam(Ignored = true)]
    public IList<Team> Teams { get; set; }

    public string TeamXml
    {
        get
        {
            var value = new StringBuilder("<ROOT>");

            if (Teams != null)
            {
                foreach (var team in Teams)
                {
                    value.AppendFormat(
                        TeamTpl,
                        team.TeamId,
                        CompanyId,
                        team.Code,
                        team.Name,
                        team.IsDeleted,
                        team.ModifiedDate,
                        team.SectionId);
                }
            }

            return value.Append("</ROOT>").ToString();
        }
    }

    public override string GetSettingKey() => "API_SYN_Team";

    public override bool IsValid() => CompanyId > 0 && Teams != null && Teams.Any();
}