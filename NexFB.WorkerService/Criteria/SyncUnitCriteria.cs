using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.Data;
using Fanex.Data.Repository;
using NexFB.WorkerService.Models;

namespace NexFB.WorkerService.Criteria;

public class SyncUnitCriteria : CriteriaBase
{
    private const string UnitTpl = "<Unit UnitId='{0}' CompanyId='{1}' Code='{2}' "
        + "Name='{3}' IsActive='{4}' ModifiedDate='{5}' IsDeleted='{6}'></Unit>";

    [SpParam(Ignored = true)]
    public int CompanyId { get; set; }

    [SpParam(Ignored = true)]
    public IList<Unit> Units { get; set; }

    public string UnitXml
    {
        get
        {
            var value = new StringBuilder("<ROOT>");

            foreach (var unit in Units)
            {
                value.AppendFormat(
                    UnitTpl,
                    unit.UnitId,
                    CompanyId,
                    unit.Code,
                    unit.Name,
                    unit.IsActive,
                    unit.ModifiedDate,
                    unit.IsDeleted);
            }

            return value.Append("</ROOT>").ToString();
        }
    }

    public override string GetSettingKey() => "API_SYN_Unit";

    public override bool IsValid() => CompanyId > 0 && Units.Any();
}