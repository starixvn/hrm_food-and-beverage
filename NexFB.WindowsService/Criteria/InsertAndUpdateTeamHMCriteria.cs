namespace NexFB.WindowsService.Criteria
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Data.Repository;

    public class InsertAndUpdateTeamHMCriteria : CriteriaBase
    {
        private const string _TeamTpl = "<Team TeamId='{0}' CompanyId='{1}' Code='{2}' "
            + "Name='{3}' IsDeleted='{4}' ModifiedDate='{5}' SectionId='{6}'></Team>";

        [SpParam(Ignored = true)]
        public int CompanyId { get; set; }

        [SpParam(Ignored = true)]
        public IList<HongMaService.TeamModel> Teams { get; set; }

        public string TeamXml
        {
            get
            {
                var value = new StringBuilder("<ROOT>");

                if (Teams != null)
                {
                    foreach (HongMaService.TeamModel team in Teams)
                    {
                        value.AppendFormat(
                            _TeamTpl,
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

        public override string GetSettingKey()
        {
            return "API_SYN_Team";
        }

        public override bool IsValid()
        {
            return CompanyId > 0 && Teams != null && Teams.Any();
        }
    }
}
