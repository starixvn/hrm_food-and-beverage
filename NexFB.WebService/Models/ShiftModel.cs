// --------------------------------------------------------------------------
// <copyright file="ShiftModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.10.2">Created</change>
// </history>

namespace NexFB.WebService.Models
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// The Team Model.
    /// </summary>
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
    }
}
