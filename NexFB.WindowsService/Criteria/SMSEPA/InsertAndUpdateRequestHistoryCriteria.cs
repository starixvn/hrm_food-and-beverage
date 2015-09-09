// --------------------------------------------------------------------------
// <copyright file="InsertAndUpdateRequestHistoryCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2015.9.9">Created</change>
// </history>

namespace NexFB.WindowsService.Criteria
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using Fanex.Data.Repository;
    using NexFB.WindowsService.SynchronizeDataService;

    public class InsertAndUpdateRequestHistoryCriteria : CriteriaBase
    {
        private const string _RequestTpl =
            "<Request RequestId='{0}' EmployeeId='{1}' RequestType='{2}' Remark='{3}' CurrentStatus='{4}' InsertedDate='{5}'></Request>";

        [SpParam(Ignored = true)]
        public IList<EmployeeRequestHistoryModel> RequestHistories { get; set; }

        public string RequestHistoryXml
        {
            get
            {
                var value = new StringBuilder("<ROOT>");

                if (RequestHistories != null)
                {
                    foreach (EmployeeRequestHistoryModel requestHistory in RequestHistories)
                    {
                        value.AppendFormat(
                            _RequestTpl,
                            requestHistory.RequestId,
                            requestHistory.UserId,
                            requestHistory.RequestType,
                            HttpUtility.HtmlEncode(requestHistory.Remark),
                            requestHistory.CurrentStatus,
                            requestHistory.InsertedDate);
                    }
                }

                return value.Append("</ROOT>").ToString();
            }
        }

        public override string GetSettingKey()
        {
            return "Sync_RequestHistory";
        }

        public override bool IsValid()
        {
            return RequestHistories != null && RequestHistories.Any();
        }
    }
}
