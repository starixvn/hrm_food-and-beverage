// --------------------------------------------------------------------------
// <copyright file="InsertAndUpdateUserNexcelCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.9.4">Created</change>
// </history>

namespace NexFB.WindowsService.Criteria
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Data.Repository;

    /// <summary>
    /// InsertAndUpdateUser Criteria.
    /// </summary>
    public class InsertAndUpdateUserNexcelCriteria : CriteriaBase
    {
        private const string _UserTpl =
            "<User EmpId='{0}' Password='{1}' Code='{2}' "
            + "NickName='{3}' FullName='{4}' DomainName='{5}' DepartmentId='{6}' IsEnabled='{7}' "
            + " IsDeleted='{8}' JoinedDate='{9}' ModifiedDate='{10}' TeamId='{11}' IsLocal='{12}' "
            + " CardNumber='{13}'></User>";

        [SpParam(Ignored = true)]
        public int DepartmentId { get; set; }

        [SpParam(Ignored = true)]
        public IList<NexcelService.UserModel> Users { get; set; }

        public string UserXml
        {
            get
            {
                var value = new StringBuilder("<ROOT>");

                if (Users != null)
                {
                    foreach (NexcelService.UserModel user in Users)
                    {
                        value.AppendFormat(
                            _UserTpl,
                            user.UserId,
                            user.Password,
                            user.Code,
                            user.NickName,
                            user.FullName,
                            user.DomainName,
                            DepartmentId,
                            user.IsEnabled,
                            user.IsDeleted,
                            user.JoinedDate,
                            user.ModifiedDate,
                            user.TeamId,
                            true,
                            user.EmployeeCard);
                    }
                }

                return value.Append("</ROOT>").ToString();
            }
        }

        public override string GetSettingKey()
        {
            return "API_SYN_Employee";
        }

        public override bool IsValid()
        {
            return DepartmentId > 0 && Users != null && Users.Any();
        }
    }
}
