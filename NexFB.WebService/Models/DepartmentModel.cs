// --------------------------------------------------------------------------
// <copyright file="DepartmentModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2015.8.28">Created</change>
// </history>

namespace NexFB.WebService.Models
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class DepartmentModel
    {
        [DataMember]
        public int DepartmentId { get; set; }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int? ParentId { get; set; }

        [DataMember]
        public int? ManagerId { get; set; }

        [DataMember]
        public int? DepartmentLevel { get; set; }

        [DataMember]
        public bool? IsActive { get; set; }

        [DataMember]
        public DateTime? ModifiedDate { get; set; }
    }
}
