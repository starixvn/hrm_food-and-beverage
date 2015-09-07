// --------------------------------------------------------------------------
// <copyright file="InsertAndUpdateJobTitleCriteria.cs" company="Nexcel Solutions Vietnam">
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
    using Fanex.Data.Repository;
    using NexFB.WindowsService.SynchronizeDataService;

    public class InsertAndUpdateJobTitleCriteria : CriteriaBase
    {
        private const string _JobTitleTpl = "<JobTitle JobTitleId='{0}' LookupId='{1}' "
            + "Code='{2}' Name='{3}' Value='{4}'></JobTitle>";

        [SpParam(Ignored = true)]
        public IList<JobTitleModel> JobTitles { get; set; }

        public string JobTitleXml
        {
            get
            {
                var value = new StringBuilder("<ROOT>");

                if (JobTitles != null)
                {
                    foreach (JobTitleModel jobTitle in JobTitles)
                    {
                        value.AppendFormat(
                            _JobTitleTpl,
                            jobTitle.Id,
                            jobTitle.LookupId,
                            jobTitle.Code,
                            jobTitle.Name,
                            jobTitle.Value);
                    }
                }

                return value.Append("</ROOT>").ToString();
            }
        }

        public override string GetSettingKey()
        {
            return "Sync_JobTitle";
        }

        public override bool IsValid()
        {
            return JobTitles != null && JobTitles.Any();
        }
    }
}
