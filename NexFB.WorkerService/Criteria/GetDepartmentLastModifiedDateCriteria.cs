using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanex.Data.Repository;

namespace NexFB.WorkerService.Criteria
{
    public class GetDepartmentLastModifiedDateCriteria : CriteriaBase
    {
        public int CompanyId { get; set; }

        public override string GetSettingKey()
        {
            return "Department_GetLastModifiedDate";
        }

        public override bool IsValid()
        {
            return CompanyId > 0;
        }
    }
}
