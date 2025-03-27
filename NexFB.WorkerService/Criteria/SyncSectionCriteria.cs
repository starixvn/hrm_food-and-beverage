using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanex.Data;
using Fanex.Data.Repository;
using NexFB.WorkerService.Models;

namespace NexFB.WorkerService.Criteria
{
    public class SyncSectionCriteria : CriteriaBase
    {
        private const string _SectionTpl = "<Section SectionId='{0}' CompanyId='{1}' Code='{2}' "
                                           + "Name='{3}' DepartmentId='{4}' IsActive='{5}' ModifiedDate='{6}' IsDeleted='{7}'></Section>";

        [SpParam(Ignored = true)]
        public int CompanyId { get; set; }

        [SpParam(Ignored = true)]
        public IList<Section> Sections { get; set; }

        public string SectionXml
        {
            get
            {
                var value = new StringBuilder("<ROOT>");

                foreach (var section in Sections)
                {
                    value.AppendFormat(
                        _SectionTpl,
                        section.SectionId,
                        CompanyId,
                        section.Code,
                        section.Name,
                        section.DepartmentId,
                        section.IsActive,
                        section.ModifiedDate,
                        section.IsDeleted);
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
            return CompanyId > 0 && Sections != null && Sections.Any();
        }
    }
}
