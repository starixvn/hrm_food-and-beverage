namespace NexFB.Services.Interface
{
    using NexFB.Core.Models;
    using NexFB.Criteria.MealTimeManagement;

    /// <summary>
    /// The interface of meal time service.
    /// </summary>
    public interface IMealTimeService
    {
        CommandResult UpdateMealPlanning(UpdateMealTimeCriteria criteria);
    }
}
