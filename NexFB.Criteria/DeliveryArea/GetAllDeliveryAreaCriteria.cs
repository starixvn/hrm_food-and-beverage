// ----------------------------------------------------------------------
// <copyright file="GetAllDeliveryAreaCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="12/17/2014 8:09:48 AM">Created.</change>
// </history>

namespace NexFB.Criteria.DeliveryArea
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get all delivery area criteria.
    /// </summary>
    public class GetAllDeliveryAreaCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "LocationID")]
        public int? DeliveryAreaId { get; set; }

        public bool? IsActive { get; set; }

        public string Query { get; set; }

        public override string GetSettingKey()
        {
            return "Location_GetAll";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
