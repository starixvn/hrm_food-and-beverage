// --------------------------------------------------------------------------
// <copyright file="PhotoModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.10.6">Created</change>
// </history>

namespace NexFB.WebService.Models
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Photo Model.
    /// </summary>
    [DataContract]
    public class PhotoModel
    {
        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public byte[] Photo { get; set; }
    }
}
