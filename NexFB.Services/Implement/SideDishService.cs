namespace NexFB.Services.Implement
{
    using System.Collections.Generic;
    using Fanex.Common.Helper;
    using NexFB.Core.Models;
    using NexFB.Core.Services;
    using NexFB.Criteria.SideDishManagement;
    using NexFB.Models.SideDishManagement;
    using NexFB.Repositories.Implement;
    using NexFB.Repositories.Interface;
    using NexFB.Services.Interface;

    /// <summary>
    /// The side dish service.
    /// </summary>
    public class SideDishService : BaseService, ISideDishService
    {
        private readonly ISideDishRepository _sideDishRepository;

        public SideDishService()
            : this(null)
        {
        }

        public SideDishService(ISideDishRepository sideDishRepository)
        {
            _sideDishRepository = sideDishRepository ?? new SideDishRepository();
        }

        /// <summary>
        /// Get all side dished.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The enumerator side dish model.</returns>
        public IEnumerable<SideDishModel> GetAllSideDish(GetAllSideDishCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            IEnumerable<SideDishModel> sideDishes = null;

            if (criteria.IsValid())
            {
                sideDishes = _sideDishRepository.GetAllSideDish(criteria);
            }

            return sideDishes;
        }

        /// <summary>
        /// Get all food unit.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The enumerator Unit model.</returns>
        public IEnumerable<UnitModel> GetAllFoodUnit(GetAllFoodUnitCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            IEnumerable<UnitModel> foodUnits = null;

            if (criteria.IsValid())
            {
                foodUnits = GetData<UnitModel>(criteria);
            }

            return foodUnits;
        }

        /// <summary>
        /// Insert side dish.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        public CommandResult InsertSideDish(InsertSideDishCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            CommandResult commandResult = new CommandResult();

            if (criteria.IsValid())
            {
                commandResult = Repository.Insert(criteria);
            }

            return commandResult;
        }

        /// <summary>
        /// Update side dish.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        public CommandResult UpdateSideDish(UpdateSideDishCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            CommandResult commandResult = new CommandResult();

            if (criteria.IsValid())
            {
                commandResult = Repository.Update(criteria);
            }

            return commandResult;
        }

        /// <summary>
        /// Activate or deactivate side dish.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        public CommandResult ActivateSideDish(ActivateSideDishCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            CommandResult commandResult = new CommandResult();

            if (criteria.IsValid())
            {
                commandResult = Repository.Update(criteria);
            }

            return commandResult;
        }

        /// <summary>
        /// Delete side dish.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        public CommandResult DeleteSideDish(DeleteSideDishCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            CommandResult commandResult = new CommandResult();

            if (criteria.IsValid())
            {
                commandResult = Repository.Delete(criteria);
            }

            return commandResult;
        }

        /// <summary>
        /// Get side dish by user Id.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The enumerator Side Dish model.</returns>
        public IEnumerable<SideDishModel> GetSideDishByUserId(GetSideDishByUserIdCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            IEnumerable<SideDishModel> sideDishes = null;

            if (criteria.IsValid())
            {
                sideDishes = _sideDishRepository.GetSideDishByUserId(criteria);
            }

            return sideDishes;
        }

        /// <summary>
        /// Edit kitchen only by food Id.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        public CommandResult UpdateKitchenOnlyByFoodId(UpdateKitchenOnlyByFoodIdCriteria criteria)
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
