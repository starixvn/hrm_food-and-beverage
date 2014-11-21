// ----------------------------------------------------------------------
// <copyright file="CommandResult.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="8/25/2014 9:34:34 AM">Created.</change>
// </history>

namespace NexFB.Core.Models
{
    using NexFB.Resources;

    /// <summary>
    /// The model contain error code and error message.
    /// </summary>
    public class CommandResult
    {
        public CommandResult()
        {
            Code = 1;
            Message = Lang.InvalidInputData;
        }

        public int Code { get; set; }

        public string Message { get; set; }

        public bool IsSuccessful
        {
            get
            {
                return Code == 0;
            }
        }
    }
}
