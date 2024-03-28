namespace NexFB.WCFService.Models
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class UserModel
    {
        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public int EmpId { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string NickName { get; set; }

        [DataMember]
        public string FullName { get; set; }

        [DataMember]
        public string EmployeeCard { get; set; }

        [DataMember]
        public string DomainName { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Phone { get; set; }

        [DataMember]
        public bool IsEnabled { get; set; }

        [DataMember]
        public bool IsDeleted { get; set; }

        [DataMember]
        public DateTime JoinedDate { get; set; }

        [DataMember]
        public bool IsLocal { get; set; }

        [DataMember]
        public DateTime ModifiedDate { get; set; }

        [DataMember]
        public int TeamId { get; set; }

        [DataMember]
        public int JobTitleId { get; set; }

        [DataMember]
        public string CostCenter { get; set; }
    }
}
