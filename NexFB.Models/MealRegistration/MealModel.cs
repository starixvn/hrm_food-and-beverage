// ----------------------------------------------------------------------
// <copyright file="MealModel.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/4/2014 2:13:27 PM">Created.</change>
// </history>

namespace NexFB.Models.MealRegistration
{
    using System;

    /// <summary>
    /// The meal model.
    /// </summary>
    [Serializable]
    public class MealModel
    {
        public int MealId { get; set; }

        public string MealEngName { get; set; }

        public string ShortName { get; set; }

        public string MealVnmName { get; set; }

        public int? IsVegetarian { get; set; }

        public bool IsShiftChecked { get; set; }

        public bool IsDisableVegetarian { get; set; }

        public bool IsDisableNonVegetarian { get; set; }

        public bool IsDisableNonRegistration { get; set; }
    }
}
