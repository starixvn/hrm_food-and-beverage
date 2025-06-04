namespace NexFB.Services.Interface
{
    using System.Collections.Generic;
    using NexFB.Core.Models;
    using NexFB.Criteria.SideDishManagement;
    using NexFB.Models.SideDishManagement;

    /// <summary>
    /// The interface of side dish service.
    /// </summary>
    public interface ISideDishService
    {
        IEnumerable<SideDishModel> GetAllSideDish(GetAllSideDishCriteria criteria);

        IEnumerable<UnitModel> GetAllFoodUnit(GetAllFoodUnitCriteria criteria);

        CommandResult InsertSideDish(InsertSideDishCriteria criteria);

        CommandResult UpdateSideDish(UpdateSideDishCriteria criteria);

        CommandResult ActivateSideDish(ActivateSideDishCriteria criteria);

        CommandResult DeleteSideDish(DeleteSideDishCriteria criteria);

        IEnumerable<SideDishModel> GetSideDishByUserId(GetSideDishByUserIdCriteria criteria);

        CommandResult UpdateKitchenOnlyByFoodId(UpdateKitchenOnlyByFoodIdCriteria criteria);
    }
}
