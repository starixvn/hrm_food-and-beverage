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
    }
}
