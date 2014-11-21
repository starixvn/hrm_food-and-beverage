// --------------------------------------------------------------------------
// <copyright file="RoleFunctionModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.11.19">Created</change>
// </history>

namespace NexFB.Models.UserControl
{
    /// <summary>
    /// Role Function Model.
    /// </summary>
    public class RoleFunctionModel
    {
        public int RoleId { get; set; }

        public int FunctionId { get; set; }

        public string FunctionKey { get; set; }

        public bool IsRead { get; set; }

        public bool IsHost { get; set; }
    }
}
