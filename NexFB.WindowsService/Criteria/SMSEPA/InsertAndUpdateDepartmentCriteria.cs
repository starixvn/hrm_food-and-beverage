// --------------------------------------------------------------------------
// <copyright file="InsertAndUpdateDepartmentCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2015.8.27">Created</change>
// </history>

namespace NexFB.WindowsService.Criteria
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using Fanex.Data.Repository;
    using NexFB.WindowsService.SynchronizeDataService;

    public class InsertAndUpdateDepartmentCriteria : CriteriaBase
    {
        private const string _DepartmentTpl = "<Department DepartmentId='{0}' Code='{1}' "
            + "Name='{2}' ParentId='{3}' ManagerId='{4}' DepartmentLevel='{5}' IsActive='{6}' ModifiedDate='{7}'></Department>";

        [SpParam(Ignored = true)]
        public IList<DepartmentModel> Departments { get; set; }

        public string DepartmentXml
        {
            get
            {
                var value = new StringBuilder("<ROOT>");

                if (Departments != null)
                {
                    foreach (DepartmentModel department in Departments)
                    {
                        value.AppendFormat(
                            _DepartmentTpl,
                            department.DepartmentId,
                            department.Code,
                            HttpUtility.HtmlEncode(department.Name),
                            department.ParentId,
                            department.ManagerId,
                            department.DepartmentLevel,
                            department.IsActive,
                            department.ModifiedDate);
                    }
                }

                return value.Append("</ROOT>").ToString();
            }
        }

        public override string GetSettingKey()
        {
            return "Sync_Department";
        }

        public override bool IsValid()
        {
            return Departments != null && Departments.Any();
        }
    }
}
