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
    public class EmployeeAttendanceModel
    {
        [DataMember]
        public DateTime WorkDate { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public int? ShiftId { get; set; }

        [DataMember]
        public DateTime? InFixed { get; set; }

        [DataMember]
        public DateTime? OutFixed { get; set; }

        [DataMember]
        public int? LateIn { get; set; }

        [DataMember]
        public int? EarlyOut { get; set; }

        [DataMember]
        public int? LeaveTypeId { get; set; }

        [DataMember]
        public int? LeaveTypeId2 { get; set; }

        [DataMember]
        public bool? Absence { get; set; }

        [DataMember]
        public decimal? TotalWorkHours { get; set; }

        [DataMember]
        public decimal? LeaveDay { get; set; }

        [DataMember]
        public DateTime? ModifiedDate { get; set; }
    }
}
