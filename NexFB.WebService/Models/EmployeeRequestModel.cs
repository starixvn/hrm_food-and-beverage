// --------------------------------------------------------------------------
// <copyright file="EmployeeRequestModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2015.8.27">Created</change>
// </history>

namespace NexFB.WebService.Models
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class EmployeeRequestModel
    {
        [DataMember]
        public long RequestId { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public int RequestType { get; set; }

        [DataMember]
        public string Remark { get; set; }

        [DataMember]
        public byte? CurrentStatus { get; set; }

        [DataMember]
        public int? VerifierId { get; set; }

        [DataMember]
        public int? ApproverId { get; set; }

        [DataMember]
        public int? HRId { get; set; }

        [DataMember]
        public DateTime? VerifiedDate { get; set; }

        [DataMember]
        public DateTime? ApprovedDate { get; set; }

        [DataMember]
        public DateTime? HRDate { get; set; }

        [DataMember]
        public byte? VerifiedStatus { get; set; }

        [DataMember]
        public byte? ApprovedStatus { get; set; }

        [DataMember]
        public byte? HRStatus { get; set; }

        [DataMember]
        public int? CreatedBy { get; set; }

        [DataMember]
        public DateTime? CreatedDate { get; set; }

        [DataMember]
        public int? ModifiedBy { get; set; }

        [DataMember]
        public DateTime? ModifiedDate { get; set; }

        [DataMember]
        public int? ReworkCount { get; set; }
    }
}
