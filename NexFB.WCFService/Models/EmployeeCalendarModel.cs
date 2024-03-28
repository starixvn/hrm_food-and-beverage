namespace NexFB.WCFService.Models
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class EmployeeCalendarModel
    {
        [DataMember]
        public DateTime WorkDate { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public int? ShiftId { get; set; }

        [DataMember]
        public bool? IsHoliday { get; set; }

        [DataMember]
        public bool? IsOffDay { get; set; }

        [DataMember]
        public bool? IsLeaveDay { get; set; }

        [DataMember]
        public bool? IsKeepInView { get; set; }

        [DataMember]
        public int? LeaveTypeId { get; set; }

        [DataMember]
        public int? LeaveTypeId2 { get; set; }

        [DataMember]
        public decimal? LeaveDay { get; set; }

        [DataMember]
        public int? LateIn { get; set; }

        [DataMember]
        public int? EarlyOut { get; set; }

        [DataMember]
        public DateTime? RequestedCheckinTime { get; set; }

        [DataMember]
        public DateTime? RequestedCheckoutTime { get; set; }

        [DataMember]
        public long? ChangeShiftId { get; set; }

        [DataMember]
        public DateTime? ModifiedDate { get; set; }

        [DataMember]
        public bool? HasCheckIn { get; set; }
    }
}
