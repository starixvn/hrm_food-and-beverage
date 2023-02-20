namespace NexFB.Repositories.Implement
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using NexFB.Models.GroupControl;
    using NexFB.Repositories.Interface;

    public class GroupControlRepository : IGroupControlRepository
    {
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
                    CompanyId = member.CompanyId,
                    CompanyName = member.CompanyName,
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