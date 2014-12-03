// --------------------------------------------------------------------------
// <copyright file="MenuCount.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.12.2">Created</change>
// </history>

namespace NexFB.Models.Kitchen
{
    /// <summary>
    /// Menu Count.
    /// </summary>
    public class MenuCount
    {
        public int MenuId { get; set; }

        public int Quantity { get; set; }

        public string MenuVnmName { get; set; }

        public string MenuEngName { get; set; }

        public int DisplayRowHeight
        {
            get
            {
                //// Grater than 25 and less than 45 letter: set Height = 40, 
                //// Less than 25 letter Height = 28
                //// Grater than 45 letter Height = 52
                var textLength = MenuVnmName.Length;
                if (textLength <= 45 && textLength > 25)
                {
                    return 40;
                }
                else
                {
                    return textLength > 45 ? 52 : 28;
                }
            }
        }
    }
}
