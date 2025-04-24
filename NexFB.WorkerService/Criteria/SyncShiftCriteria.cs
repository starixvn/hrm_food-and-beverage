using System.Text;
using Fanex.Data.Repository;
using Fanex.Data;
using NexFB.WorkerService.Models;

namespace NexFB.WorkerService.Criteria;

public class SyncShiftCriteria : CriteriaBase
{
    private const string _ShiftTpl =
        "<Shift ShiftId='{0}' Code='{1}' Name='{2}' "
        + "BeginCheckInTime='{3}' EndCheckInTime='{4}' BeginCheckOutTime='{5}' EndCheckOutTime='{6}' IsInAfterDay='{7}' "
        + " IsOutAfterDay='{8}' CompanyId='{9}' ModifiedDate='{10}' IsDisabled='{11}'></Shift>";

    [SpParam(Ignored = true)]
    public int CompanyId { get; set; }

    [SpParam(Ignored = true)]
    public IList<Shift> Shifts { get; set; }

    public string ShiftXml
    {
        get
        {
            var value = new StringBuilder("<ROOT>");

            foreach (var shift in Shifts)
            {
                value.AppendFormat(
                    _ShiftTpl,
                    shift.ShiftId,
                    shift.Code,
                    shift.Name,
                    shift.BeginCheckInTime,
                    shift.EndCheckInTime,
                    shift.BeginCheckOutTime,
                    shift.EndCheckOutTime,
                    shift.IsInAfterDay,
                    shift.IsOutAfterDay,
                    CompanyId,
                    DateTime.Now,
                    shift.IsDisabled);
            }

            return value.Append("</ROOT>").ToString();
        }
    }

    public override string GetSettingKey()
    {
        return "API_SYN_Shift";
    }

    public override bool IsValid()
    {
        return CompanyId > 0 && Shifts.Any();
    }
}