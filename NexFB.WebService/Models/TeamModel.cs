namespace NexFB.WebService.Models
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class TeamModel
    {
        [DataMember]
        public int TeamId { get; set; }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int DepartmentId { get; set; }

        [DataMember]
        public int LeaderId { get; set; }

        [DataMember]
        public int AssistantLeaderId { get; set; }

        [DataMember]
        public int Rank { get; set; }

        [DataMember]
        public bool IsDeleted { get; set; }

        [DataMember]
        public DateTime ModifiedDate { get; set; }
    }
}
