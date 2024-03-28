namespace NexFB.WCFService.Models
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class UnitModel
    {
        [DataMember]
        public int UnitId { get; set; }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public bool IsDeleted { get; set; }

        [DataMember]
        public DateTime ModifiedDate { get; set; }
    }
}
