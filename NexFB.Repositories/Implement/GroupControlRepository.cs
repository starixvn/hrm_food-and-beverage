// --------------------------------------------------------------------------
// <copyright file="GroupControlRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.10.31">Created</change>
// </history>

namespace NexFB.Repositories.Implement
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using NexFB.Models.GroupControl;
    using NexFB.Repositories.Interface;

    /// <summary>
    /// The Group Control repository.
    /// </summary>
    public class GroupControlRepository : IGroupControlRepository
    {
        /// <summary>
        /// Get groups.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The group list.</returns>
        public IList<GroupModel> GetGroup(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var multi = db.QueryMultiple(criteria.GetSpParams()))
                {
                    var groups = multi.Read<GroupModel>().ToList();
                    
                    return groups;
                }
            }
        }

        /// <summary>
        /// Get groups.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The group list.</returns>
        public IEnumerable<MemberModel> GetMember(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var multi = db.QueryMultiple(criteria.GetSpParams()))
                {
                    var rawMembers = multi.Read<dynamic>().ToList();
                    var rawGroups = multi.Read<dynamic>().ToList();

                    return GetMemberList(rawMembers, rawGroups);
                }
            }
        }

        /// <summary>
        /// Get member list.
        /// </summary>
        /// <param name="rawMembers">The raw members list.</param>
        /// <param name="rawGroups">The raw groups list.</param>
        /// <returns>The member list.</returns>
        private IEnumerable<MemberModel> GetMemberList(IEnumerable<dynamic> rawMembers, IEnumerable<dynamic> rawGroups)
        {
            var members = new List<MemberModel>();

            foreach (var member in rawMembers)
            {
                var assignedGroups = GetAssignedGroups(rawGroups, member);

                members.Add(new MemberModel
                    {
                        SysId = member.SysID,
                        EmpId = member.EmpId,
                        DepartmentId = member.DepartmentID,
                        DepartmentName = member.DepartmentName,
                        Fullname = member.FullName,
                        Nickname = member.NickName,
                        EmployeeTypeName = member.CategoryName,
                        AssignedGroups = assignedGroups,
                        JoinedDate = member.JoinedDate
                    });
            }

            return members;
        }

        private IEnumerable<dynamic> GetAssignedGroups(IEnumerable<dynamic> rawGroups, dynamic member)
        {
            var groups = rawGroups.Where(t => t.SysID == member.SysID).ToList();
            var assignedGroups = groups.Select(t => new
                {
                    GroupId = t.GroupID == null ? null : int.Parse(t.GroupID.ToString()),
                    GroupName = t.GroupName
                });

            return assignedGroups;
        }
    }
}
