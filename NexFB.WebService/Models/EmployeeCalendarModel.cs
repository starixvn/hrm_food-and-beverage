// --------------------------------------------------------------------------
// <copyright file="EmployeeCalendarModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.10.2">Created</change>
// </history>

namespace NexFB.WebService.Models
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// The Team Model.
    /// </summary>
    [DataContract]
    public class EmployeeCalendarModel
    {
        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public DateTime WorkDate { get; set; }

        [DataMember]
        public int ShiftId { get; set; }

        [DataMember]
        public bool IsHoliday { get; set; }

        [DataMember]
        public bool IsOffDay { get; set; }

        [DataMember]
        public bool IsLeaveDay { get; set; }

        [DataMember]
        public int LeaveTypeId { get; set; }

        [DataMember]
        public decimal LeaveDay { get; set; }

        [DataMember]
        public DateTime ModifiedDate { get; set; }
    }
}
