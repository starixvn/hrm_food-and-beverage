// --------------------------------------------------------------------------
// <copyright file="InsertAndUpdateShiftCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.10.2">Created</change>
// </history>

namespace NexFB.WindowsService.Criteria
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using Fanex.Data.Repository;
    using NexFB.WindowsService.SynchronizeDataService;

    public class InsertAndUpdateShiftCriteria : CriteriaBase
    {
        private const string _ShiftTpl =
            "<Shift ShiftId='{0}' Code='{1}' Name='{2}' "
            + "BeginCheckInTime='{3}' EndCheckInTime='{4}' BeginCheckOutTime='{5}' EndCheckOutTime='{6}' IsInAfterDay='{7}' "
            + " IsOutAfterDay='{8}' ModifiedDate='{9}' IsDisabled='{10}'></Shift>";

        [SpParam(Ignored = true)]
        public IList<ShiftModel> Shifts { get; set; }

        public string ShiftXml
        {
            get
            {
                var value = new StringBuilder("<ROOT>");

                if (Shifts != null)
                {
                    foreach (ShiftModel shift in Shifts)
                    {
                        value.AppendFormat(
                            _ShiftTpl,
                            shift.ShiftId,
                            shift.Code,
                            HttpUtility.HtmlEncode(shift.Name),
                            shift.BeginCheckInTime,
                            shift.EndCheckInTime,
                            shift.BeginCheckOutTime,
                            shift.EndCheckOutTime,
                            shift.IsInAfterDay,
                            shift.IsOutAfterDay,
                            DateTime.Now,
                            shift.IsDisabled);
                    }
                }

                return value.Append("</ROOT>").ToString();
            }
        }

        public override string GetSettingKey()
        {
            return "Sync_Shift";
        }

        public override bool IsValid()
        {
            return Shifts != null && Shifts.Any();
        }
    }
}
