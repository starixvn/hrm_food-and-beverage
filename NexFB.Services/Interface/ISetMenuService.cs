namespace NexFB.Services.Interface
{
    using System.Collections.Generic;
    using NexFB.Core.Models;
    using NexFB.Criteria.SetMenuManagement;
    using NexFB.Models.SetMenuManagement;

    /// <summary>
    /// The interface of set menu service.
    /// </summary>
    public interface ISetMenuService
    {
        CommandResult InsertSetMenu(InsertSetMenuCriteria criteria);

        CommandResult UpdateSetMenu(UpdateSetMenuCriteria criteria);

        CommandResult DeleteSetMenu(DeleteSetMenuCriteria criteria);

        CommandResult UpdateStatusSetMenu(UpdateStatusSetMenuCriteria criteria);

        SetMenuModel GetOrderById(GetSetMenuByIdCriteria criteria);

        IList<SetMenuModel> GetSetMenu(GetSetMenuCriteria criteria);
    }
}
