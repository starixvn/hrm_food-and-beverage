// ----------------------------------------------------------------------
// <copyright file="ShiftMealMappingModel.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="10/14/2014 10:04:32 AM">Created.</change>
// </history>

namespace NexFB.Models.ShiftMealMapping
{
    using NexFB.Models.Entities;

    /// <summary>
    /// Shift meal mapping model.
    /// </summary>
    public class ShiftMealMappingModel
    {
        public ShiftMealMappingModel()
        {
            Department = new Department();
        }

        public int ShiftId { get; set; }

        public string ShiftCode { get; set; }

        public string ShiftName { get; set; }

        public string BeginCheckInTime { get; set; }

        public string EndCheckInTime { get; set; }

        public string BeginCheckOutTime { get; set; }

        public string EndCheckOutTime { get; set; }

        public bool MealBreakfast { get; set; }

        public bool MealLunch { get; set; }

        public bool MealDinner { get; set; }

        public bool MealSupper { get; set; }

        public bool MealNextBreakfast { get; set; }

        public bool MealNextLunch { get; set; }

        public Department Department { get; set; }
    }
}
