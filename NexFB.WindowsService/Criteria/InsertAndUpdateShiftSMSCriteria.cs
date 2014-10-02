// --------------------------------------------------------------------------
// <copyright file="InsertAndUpdateShiftSMSCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.10.2">Created</change>
// </history>

namespace NexFB.WindowsService.Criteria
{
    using System.Collections.Generic;
    using System.Text;
    using Fanex.Data.Repository;

    /// <summary>
    /// InsertAndUpdateShift Criteria.
    /// </summary>
    public class InsertAndUpdateShiftSmsCriteria : CriteriaBase
    {
        private const string _ShiftTpl =
            "<Shift ShiftId='{0}' Code='{1}' Name='{2}' "
            + "BeginCheckInTime='{3}' EndCheckInTime='{4}' BeginCheckOutTime='{5}' EndCheckOutTime='{6}' IsInAfterDay='{7}' "
            + " IsOutAfterDay='{8}' DepartmentId='{9}' ModifiedDate='{10}'></Shift>";

        [SpParam(Ignored = true)]
        public int DepartmentId { get; set; }

        [SpParam(Ignored = true)]
        public IList<SMSService.ShiftModel> Shifts { get; set; }

        public string ShiftXml
        {
            get
            {
                var value = new StringBuilder("<ROOT>");

                if (Shifts != null)
                {
                    foreach (SMSService.ShiftModel shift in Shifts)
                    {
                        value.AppendFormat(
                            _ShiftTpl,
                            shift.ShiftId,
                            shift.Code,
                            shift.Name,
                            shift.BeginCheckInTime,
                            shift.EndCheckInTime,
                            shift.BeginCheckOutTime,
                            shift.EndCheckOutTime,
                            shift.IsInAfterDay,
                            shift.IsOutAfterDay,
                            this.DepartmentId,
                            shift.ModifiedDate);
                    }
                }

                return value.Append("</ROOT>").ToString();
            }
        }

        public override string GetSettingKey()
        {
            return "API_SYN_Shift";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
