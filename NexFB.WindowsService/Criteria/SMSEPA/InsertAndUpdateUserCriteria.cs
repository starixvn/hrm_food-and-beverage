// --------------------------------------------------------------------------
// <copyright file="InsertAndUpdateUserCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2015.8.27">Created</change>
// </history>

namespace NexFB.WindowsService.Criteria
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Data.Repository;
    using NexFB.WindowsService.SynchronizeDataService;

    public class InsertAndUpdateUserCriteria : CriteriaBase
    {
        private const string _UserTpl =
            "<User EmpId='{0}' DomainUsername='{1}' Code='{2}' "
            + "NickName='{3}' FullName='{4}' TeamId='{5}' JobTitleId='{6}' Phone='{7}' "
            + " Email='{8}' CardNumber='{9}' IsEnabled='{10}' IsDeleted='{11}' Password='{12}' "
            + " JoinedDate='{13}' ModifiedDate='{14}' ></User>";

        [SpParam(Ignored = true)]
        public IList<UserModel> Users { get; set; }

        public string UserXml
        {
            get
            {
                var value = new StringBuilder("<ROOT>");

                if (Users != null)
                {
                    foreach (UserModel user in Users)
                    {
                        value.AppendFormat(
                            _UserTpl,
                            user.UserId,
                            user.DomainName,
                            user.Code,
                            user.NickName,
                            user.FullName,
                            user.TeamId,
                            user.JobTitleId,
                            user.Phone,
                            user.Email,
                            user.EmployeeCard,
                            user.IsEnabled,
                            user.IsDeleted,
                            user.Password,
                            user.JoinedDate,
                            user.ModifiedDate);
                    }
                }

                return value.Append("</ROOT>").ToString();
            }
        }

        public override string GetSettingKey()
        {
            return "Sync_Employee";
        }

        public override bool IsValid()
        {
            return Users != null && Users.Any();
        }
    }
}
