namespace NexFB.Repositories.Interface
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using NexFB.Models.GroupControl;

    /// <summary>
    /// The interface of User Control repository.
    /// </summary>
    public interface IGroupControlRepository
    {
        IList<GroupModel> GetGroup(ICriteria criteria);

        IEnumerable<MemberModel> GetMember(ICriteria criteria);
    }
}
