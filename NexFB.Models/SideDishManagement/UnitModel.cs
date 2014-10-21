// ----------------------------------------------------------------------
// <copyright file="UnitModel.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="10/9/2014 9:56:46 AM">Created.</change>
// </history>

namespace NexFB.Models.SideDishManagement
{
    /// <summary>
    /// The unit model.
    /// </summary>
    public class UnitModel
    {
        public int UnitId { get; set; }

        public string UnitEngName { get; set; }

        public string UnitVnmName { get; set; }

        public string UnitName
        {
            get
            {
                return UnitVnmName;
            }
        }
    }
}
