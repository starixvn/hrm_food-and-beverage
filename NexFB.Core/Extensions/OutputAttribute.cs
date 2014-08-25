// ----------------------------------------------------------------------
// <copyright file="OutputAttribute.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="8/25/2014 9:41:21 AM">Created.</change>
// </history>

namespace NexFB.Core.Extensions
{
    using System;

    /// <summary>
    /// The output attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public sealed class OutputAttribute : Attribute
    {
        public OutputAttribute()
        {
        }
    }
}
