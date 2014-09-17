// --------------------------------------------------------------------------
// <copyright file="HubData.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.9.17">Created</change>
// </history>

namespace NexFB.Models.Kitchen
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Class HubData.
    /// </summary>
    public class HubData
    {
        public int CurrentOrderId { get; set; }

        public ObservableCollection<Order> CurrentOrderList { get; set; }

        public int CurrentScreen { get; set; }
    }
}
