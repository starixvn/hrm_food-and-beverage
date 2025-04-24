using System.Text;
using Fanex.Data;
using Fanex.Data.Repository;
using NexFB.WorkerService.Models;

namespace NexFB.WorkerService.Criteria;

public class SyncDepartmentCriteria : CriteriaBase
{
    private const string _DepartmentTpl = "<Department DepartmentId='{0}' CompanyId='{1}' Code='{2}' "
                                          + "Name='{3}' UnitId='{4}' IsActive='{5}' ModifiedDate='{6}' IsDeleted='{7}'></Department>";

    [SpParam(Ignored = true)]
    public int CompanyId { get; set; }

    [SpParam(Ignored = true)]
    public IList<Department> Departments { get; set; }

    public string DepartmentXml
    {
        get
        {
            var value = new StringBuilder("<ROOT>");

            foreach (var department in Departments)
            {
                value.AppendFormat(
                    _DepartmentTpl,
                    department.DepartmentId,
                    CompanyId,
                    department.Code,
                    department.Name,
                    department.UnitId,
                    department.IsActive,
                    department.ModifiedDate,
                    department.IsDeleted);
            }

            return value.Append("</ROOT>").ToString();
        }
    }

    public override string GetSettingKey()
    {
        return "API_SYN_Department";
    }

    public override bool IsValid()
    {
        return CompanyId > 0 && Departments != null && Departments.Any();
    }
}