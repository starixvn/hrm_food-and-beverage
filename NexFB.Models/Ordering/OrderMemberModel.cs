// --------------------------------------------------------------------------
// <copyright file="OrderMemberModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.9.1">Created</change>
// </history>

namespace NexFB.Models.Ordering
{
    using NexFB.Models.Entities;

    /// <summary>
    /// OrderMember Model.
    /// </summary>
    public class OrderMemberModel
    {
        public int UserId { get; set; }

        public string NickName { get; set; }

        public string FullName { get; set; }

        public Menu Menu { get; set; }

        public bool IsOrder { get; set; }
    }
}
