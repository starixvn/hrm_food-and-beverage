namespace NexFB.Repositories.Interface
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using NexFB.Models.GroupControl;

    public interface IGroupControlRepository
    {
        IEnumerable<MemberModel> GetMember(ICriteria criteria);
    }
}