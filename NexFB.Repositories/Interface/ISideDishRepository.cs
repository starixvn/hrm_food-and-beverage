namespace NexFB.Repositories.Interface
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using NexFB.Models.SideDishManagement;

    /// <summary>
    /// The side dish management repository.
    /// </summary>
    public interface ISideDishRepository
    {
        IEnumerable<SideDishModel> GetAllSideDish(ICriteria criteria);

        IEnumerable<SideDishModel> GetSideDishByUserId(ICriteria criteria);
    }
}
