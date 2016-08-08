namespace NexFB.WebService.Models
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class ShiftModel
    {
        [DataMember]
        public int ShiftId { get; set; }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string BeginCheckInTime { get; set; }

        [DataMember]
        public string EndCheckInTime { get; set; }

        [DataMember]
        public string BeginCheckOutTime { get; set; }

        [DataMember]
        public string EndCheckOutTime { get; set; }

        [DataMember]
        public bool IsInAfterDay { get; set; }

        [DataMember]
        public bool IsOutAfterDay { get; set; }

        [DataMember]
        public DateTime ModifiedDate { get; set; }

        [DataMember]
        public bool IsDisabled { get; set; }
    }
}
