// --------------------------------------------------------------------------
// <copyright file="MemberModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.11.6">Created</change>
// </history>

namespace NexFB.Models.GroupControl
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Member model.
    /// </summary>
    public class MemberModel
    {
        public int SysId { get; set; }

        public int EmpId { get; set; }

        public string Fullname { get; set; }

        public string Nickname { get; set; }

        public short? DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public IEnumerable<int?> GroupIds { get; set; }

        public IEnumerable<string> GroupNames { get; set; }

        public DateTime? JoinedDate { get; set; }
    }
}