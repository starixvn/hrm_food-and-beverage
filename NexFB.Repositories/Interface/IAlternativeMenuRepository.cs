namespace NexFB.Repositories.Interface
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using NexFB.Models.AlternativeMenuManagement;

    /// <summary>
    /// The interface of alternative menu repository.
    /// </summary>
    public interface IAlternativeMenuRepository
    {
        IEnumerable<AlternativeMenuModel> GetAllAlternativeMenu(ICriteria criteria);
    }
}
