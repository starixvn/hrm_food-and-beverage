// --------------------------------------------------------------------------
// <copyright file="JobTitleModel.cs" company="Nexcel Solutions Vietnam">
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
    public class JobTitleModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int LookupId { get; set; }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Value { get; set; }
    }
}
