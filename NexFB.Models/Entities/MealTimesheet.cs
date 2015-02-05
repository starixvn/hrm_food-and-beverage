// ----------------------------------------------------------------------
// <copyright file="MealTimesheet.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/11/2014 10:02:36 AM">Created.</change>
// </history>

namespace NexFB.Models.Entities
{
    using System;

    /// <summary>
    /// The meal timesheet.
    /// </summary>
    public class MealTimesheet
    {
        public int MealId { get; set; }

        public string MealVnmName { get; set; }

        public string MealEngName { get; set; }

        public DateTime StartTime { get; set; }

        public string HHStartTime
        {
            get
            {
                string hour = StartTime.Hour.ToString();
                if (hour.Length.Equals(1))
                {
                    hour = hour.Insert(0, "0");
                }

                return hour;
            }
        }

        public string MMStartTime
        {
            get
            {
                string minute = StartTime.Minute.ToString();
                if (minute.Length.Equals(1))
                {
                    minute = minute.Insert(0, "0");
                }

                return minute;
            }
        }

        public DateTime EndTime { get; set; }

        public string HHEndTime
        {
            get
            {
                string hour = EndTime.Hour.ToString();
                if (hour.Length.Equals(1))
                {
                    hour = hour.Insert(0, "0");
                }

                return hour;
            }
        }

        public string MMEndTime
        {
            get
            {
                string minute = EndTime.Minute.ToString();
                if (minute.Length.Equals(1))
                {
                    minute = minute.Insert(0, "0");
                }

                return minute;
            }
        }

        public DateTime RegisterTime { get; set; }

        public string HHRegisterTime
        {
            get
            {
                string hour = RegisterTime.Hour.ToString();
                if (hour.Length.Equals(1))
                {
                    hour = hour.Insert(0, "0");
                }

                return hour;
            }
        }

        public string MMRegisterTime
        {
            get
            {
                string minute = RegisterTime.Minute.ToString();
                if (minute.Length.Equals(1))
                {
                    minute = minute.Insert(0, "0");
                }

                return minute;
            }
        }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
