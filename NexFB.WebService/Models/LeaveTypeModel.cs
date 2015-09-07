// --------------------------------------------------------------------------
// <copyright file="LeaveTypeModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2015.8.27">Created</change>
// </history>

namespace NexFB.WebService.Models
{
    using System.Runtime.Serialization;

    [DataContract]
    public class LeaveTypeModel
    {
        [DataMember]
        public int LeaveTypeId { get; set; }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public bool IsDisabled { get; set; }
    }
}
