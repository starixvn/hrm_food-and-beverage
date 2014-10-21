// --------------------------------------------------------------------------
// <copyright file="UserModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.9.4">Created</change>
// </history>

namespace NexFB.WebService.Models
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// User Model.
    /// </summary>
    [DataContract]
    public class UserModel
    {
        [DataMember]
        public int UserId { get; set; }

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
        public bool IsEnabled { get; set; }

        [DataMember]
        public bool IsDeleted { get; set; }

        [DataMember]
        public DateTime JoinedDate { get; set; }

        [DataMember]
        public DateTime ModifiedDate { get; set; }

        [DataMember]
        public int TeamId { get; set; }
    }
}
