// --------------------------------------------------------------------------
// <copyright file="DataPrint.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.8.29">Created</change>
// </history>

namespace NexFB.WinApp.Models.ManageOrder
{
    using System;

    /// <summary>
    /// Data Print.
    /// </summary>
    public class DataPrint
    {
        public string CustomerName { get; set; }

        public string Address { get; set; }

        public string PledgeNumber { get; set; }

        public DateTime Date { get; set; }
    }
}
