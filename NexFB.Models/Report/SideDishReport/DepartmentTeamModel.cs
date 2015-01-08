// -----------------------------------------------------------------------
// <copyright file="DepartmentTeamModel.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Larry.Tran" date="1/7/2015 5:03 PM">Created.</change>
// </history>

namespace NexFB.Models.Report.SideDishReport
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// The DepartmentTeamModel class.
    /// </summary>
    public class DepartmentTeamModel
    {
        public DepartmentTeamModel()
        {
            OrderModels = new List<OrderModel>();
        }

        public string DepartmentTeamName { get; set; }

        public List<OrderModel> OrderModels { get; private set; }
    }
}
