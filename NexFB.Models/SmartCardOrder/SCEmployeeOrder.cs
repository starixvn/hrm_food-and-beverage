// --------------------------------------------------------------------------
// <copyright file="SCEmployeeOrder.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.20">Created</change>
// </history>

namespace NexFB.Models.SmartCardOrder
{
    using System;

    /// <summary>
    /// SC Employee Order.
    /// </summary>
    public class SCEmployeeOrder
    {
        public int OrderId { get; set; }

        public int QuantityOrder { get; set; }

        public int OrderNo { get; set; }

        public string MealEngName { get; set; }

        public string MealVnmName { get; set; }

        public bool IsMainMenu { get; set; }

        public bool IsSideDish { get; set; }
    }
}
