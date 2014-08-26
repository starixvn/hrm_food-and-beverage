// ----------------------------------------------------------------------
// <copyright file="FunctionModel.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="8/26/2014 10:33:37 AM">Created.</change>
// </history>

namespace NexFB.Models.UserControl
{
    using System.Collections.Generic;

    /// <summary>
    /// The function model.
    /// </summary>
    public class FunctionModel
    {
        public FunctionModel()
        {
            ChildFunctions = new List<FunctionModel>();
        }

        public int FunctionId { get; set; }

        public string FunctionKey { get; set; }

        public string Name { get; set; }

        public int? ParentFunctionId { get; set; }

        public bool IsReadOnly { get; set; }

        public bool IsRead { get; set; }

        public bool IsHost { get; set; }

        public List<FunctionModel> ChildFunctions { get; set; }
    }
}
