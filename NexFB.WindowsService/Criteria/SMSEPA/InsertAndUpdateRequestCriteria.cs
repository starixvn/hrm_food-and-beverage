// --------------------------------------------------------------------------
// <copyright file="InsertAndUpdateRequestCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2015.8.28">Created</change>
// </history>

namespace NexFB.WindowsService.Criteria
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using Fanex.Data.Repository;
    using NexFB.WindowsService.SynchronizeDataService;

    public class InsertAndUpdateRequestCriteria : CriteriaBase
    {
        private const string _RequestTpl =
            "<Request RequestId='{0}' EmployeeId='{1}' RequestType='{2}' Remark='{3}' CurrentStatus='{4}' VerifierId='{5}' ApproverId='{6}'"
            + " HRId='{7}' VerifiedDate='{8}' ApprovedDate='{9}' HRDate='{10}' VerifiedStatus='{11}' ApprovedStatus='{12}'"
            + " HRStatus='{13}' CreatedBy='{14}' CreatedDate='{15}' ModifiedBy='{16}' ModifiedDate='{17}' ReworkCount='{18}'></Request>";

        [SpParam(Ignored = true)]
        public IList<EmployeeRequestModel> EmployeeRequests { get; set; }

        public string RequestXml
        {
            get
            {
                var value = new StringBuilder("<ROOT>");

                if (EmployeeRequests != null)
                {
                    foreach (EmployeeRequestModel request in EmployeeRequests)
                    {
                        value.AppendFormat(
                            _RequestTpl,
                            request.RequestId,
                            request.UserId,
                            request.RequestType,
                            HttpUtility.HtmlEncode(request.Remark),
                            request.CurrentStatus,
                            request.VerifierId,
                            request.ApproverId,
                            request.HRId,
                            request.VerifiedDate,
                            request.ApprovedDate,
                            request.HRDate,
                            request.VerifiedStatus,
                            request.ApprovedStatus,
                            request.HRStatus,
                            request.CreatedBy,
                            request.CreatedDate,
                            request.ModifiedBy,
                            request.ModifiedDate,
                            request.ReworkCount);
                    }
                }

                return value.Append("</ROOT>").ToString();
            }
        }

        public override string GetSettingKey()
        {
            return "Sync_Request";
        }

        public override bool IsValid()
        {
            return EmployeeRequests != null && EmployeeRequests.Any();
        }
    }
}
