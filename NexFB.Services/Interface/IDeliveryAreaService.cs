namespace NexFB.Services.Interface
{
    using System.Collections.Generic;
    using NexFB.Core.Models;
    using NexFB.Criteria.DeliveryArea;
    using NexFB.Models.Entities;

    /// <summary>
    /// The interface of delivery area service..
    /// </summary>
    public interface IDeliveryAreaService
    {
        IEnumerable<Location> GetAllDeliveryArea(GetAllDeliveryAreaCriteria criteria);

        CommandResult InsertDeliveryArea(InsertDeliveryAreaCriteria criteria);

        CommandResult UpdateDeliveryArea(UpdateDeliveryAreaCriteria criteria);

        CommandResult ActivateDeliveryArea(ActivateDeliveryAreaCriteria criteria);

        CommandResult DeleteDeliveryArea(DeleteDeliveryAreaCriteria criteria);

        bool CheckDeliveryAreaExistOrderMeal(CheckDeliveryAreaExistOrderMealCriteria criteria);
    }
}
