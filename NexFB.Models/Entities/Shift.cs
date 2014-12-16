// ----------------------------------------------------------------------
// <copyright file="Shift.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="12/15/2014 3:24:40 PM">Created.</change>
// </history>

namespace NexFB.Models.Entities
{
    using System;

    /// <summary>
    /// The Shift model.
    /// </summary>
    [Serializable]
    public class Shift
    {
        public int ShiftId { get; set; }

        public string ShiftCode { get; set; }

        public string ShiftName { get; set; }
    }
}
