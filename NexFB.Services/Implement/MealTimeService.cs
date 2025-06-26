namespace NexFB.Services.Implement
{
    using Fanex.Common.Helper;
    using NexFB.Core.Models;
    using NexFB.Core.Services;
    using NexFB.Criteria.MealTimeManagement;
    using NexFB.Services.Interface;

    /// <summary>
    /// The meal time service.
    /// </summary>
    public class MealTimeService : BaseService, IMealTimeService
    {
        public MealTimeService()
        {
        }

        /// <summary>
        /// Update meal time.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        public CommandResult UpdateMealPlanning(UpdateMealTimeCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            CommandResult commandResult = new CommandResult();

            if (criteria.IsValid())
            {
                commandResult = Repository.Update(criteria);
            }

            return commandResult;
        }
    }
}
