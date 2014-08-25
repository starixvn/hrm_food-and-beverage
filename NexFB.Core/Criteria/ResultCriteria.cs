// ----------------------------------------------------------------------
// <copyright file="ResultCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="8/25/2014 9:43:20 AM">Created.</change>
// </history>

namespace NexFB.Core.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// The result criteria contain error code and error message.
    /// </summary>
    public class ResultCriteria : CriteriaBase
    {
        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }
    }
}
