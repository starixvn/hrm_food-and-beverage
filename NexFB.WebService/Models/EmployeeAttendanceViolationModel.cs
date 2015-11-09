// --------------------------------------------------------------------------
// <copyright file="EmployeeAttendanceViolationModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2015.9.14">Created</change>
// </history>

namespace NexFB.WebService.Models
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class EmployeeAttendanceViolationModel
    {
        [DataMember]
        public DateTime WorkDate { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public int MissingCheckInOut { get; set; }

        [DataMember]
        public int LateIn { get; set; }

        [DataMember]
        public int EarlyOut { get; set; }

        [DataMember]
        public int ChangeShift { get; set; }

        [DataMember]
        public int WorkingBelowTime { get; set; }

        [DataMember]
        public int Absent { get; set; }

        [DataMember]
        public int UnpaidLeaveAbsent { get; set; }

        [DataMember]
        public double UnpaidLeave { get; set; }

        [DataMember]
        public DateTime? ModifiedDate { get; set; }

        [DataMember]
        public string ShiftName { get; set; }

        [DataMember]
        public DateTime? InFixed { get; set; }

        [DataMember]
        public DateTime? OutFixed { get; set; }

        [DataMember]
        public int? LateInValue { get; set; }

        [DataMember]
        public int? EarlyOutValue { get; set; }

        [DataMember]
        public decimal? TotalWorkHours { get; set; }

        [DataMember]
        public string LateInApproverComment { get; set; }

        [DataMember]
        public string LateInEmployeeComment { get; set; }

        [DataMember]
        public string EarlyOutApproverComment { get; set; }

        [DataMember]
        public string EarlyOutEmployeeComment { get; set; }

        [DataMember]
        public string ChangeShiftApproverComment { get; set; }

        [DataMember]
        public string ChangeShiftEmployeeComment { get; set; }

        [DataMember]
        public string LeaveApproverComment { get; set; }

        [DataMember]
        public string LeaveEmployeeComment { get; set; }
    }
}
