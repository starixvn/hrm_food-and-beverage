namespace NexFB.Services.Interface
{
    using System.Collections.Generic;
    using NexFB.Core.Models;
    using NexFB.Criteria.AlternativeMenuManagement;
    using NexFB.Models.AlternativeMenuManagement;

    /// <summary>
    /// The interface of alternative service.
    /// </summary>
    public interface IAlternativeMenuService
    {
        IEnumerable<AlternativeMenuModel> GetAllAlternativeMenu(GetAllAlternativeMenuCriteria criteria);

        CommandResult InsertAlternativeMenu(InsertAlternativeMenuCriteria criteria);

        CommandResult UpdateAlternativeMenu(UpdateAlternativeMenuCriteria criteria);

        CommandResult ActivateAlternativeMenu(ActivateAlternativeMenuCriteria criteria);

        CommandResult DeleteAlternativeMenu(DeleteAlternativeMenuCriteria criteria);
    }
}
