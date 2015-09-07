// --------------------------------------------------------------------------
// <copyright file="TeamModel.cs" company="Nexcel Solutions Vietnam">
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
    /// The Team Model.
    /// </summary>
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
