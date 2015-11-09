// --------------------------------------------------------------------------
// <copyright file="GroupSupervisorModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2015.9.21">Created</change>
// </history>

namespace NexFB.WebService.Models
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class GroupSupervisorModel
    {
        [DataMember]
        public int OwnerId { get; set; }

        [DataMember]
        public int GroupId { get; set; }

        [DataMember]
        public byte ActionType { get; set; }

        [DataMember]
        public byte RequestType { get; set; }

        [DataMember]
        public DateTime? ModifiedDate { get; set; }
    }
}
