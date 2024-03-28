namespace NexFB.WCFService.Models
{
    using System.Runtime.Serialization;

    [DataContract]
    public class PhotoModel
    {
        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public byte[] Photo { get; set; }
    }
}
