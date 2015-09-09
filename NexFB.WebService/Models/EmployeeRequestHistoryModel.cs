// --------------------------------------------------------------------------
// <copyright file="EmployeeRequestHistoryModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2015.9.9">Created</change>
// </history>

namespace NexFB.WebService.Models
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class EmployeeRequestHistoryModel
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
        public DateTime? InsertedDate { get; set; }
    }
}
