namespace NexFB.Repositories.Interface
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using NexFB.Models.SetMenuManagement;

    public interface ISetMenuRepository
    {
        IList<SetMenuModel> GetSetMenu(ICriteria criteria);
    }
}
