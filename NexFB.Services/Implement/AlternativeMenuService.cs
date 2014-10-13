// ----------------------------------------------------------------------
// <copyright file="AlternativeMenuService.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="10/10/2014 2:37:10 PM">Created.</change>
// </history>

namespace NexFB.Services.Implement
{
    using System.Collections.Generic;
    using Fanex.Common.Helper;
    using NexFB.Core.Models;
    using NexFB.Core.Services;
    using NexFB.Criteria.AlternativeMenuManagement;
    using NexFB.Models.AlternativeMenuManagement;
    using NexFB.Repositories.Implement;
    using NexFB.Repositories.Interface;
    using NexFB.Services.Interface;

    /// <summary>
    /// The alternative service.
    /// </summary>
    public class AlternativeMenuService : BaseService, IAlternativeMenuService
    {
        private readonly IAlternativeMenuRepository _alternativeMenuRepository;

        public AlternativeMenuService()
            : this(null)
        {
        }

        public AlternativeMenuService(IAlternativeMenuRepository alternativeMenuRepository)
        {
            _alternativeMenuRepository = alternativeMenuRepository ?? new AlternativeMenuRepository();
        }

        /// <summary>
        /// Get all alternative menus.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The enumerator alternative menu model.</returns>
        public IEnumerable<AlternativeMenuModel> GetAllAlternativeMenu(GetAllAlternativeMenuCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            IEnumerable<AlternativeMenuModel> alternativeMenus = null;

            if (criteria.IsValid())
            {
                alternativeMenus = _alternativeMenuRepository.GetAllAlternativeMenu(criteria);
            }

            return alternativeMenus;
        }

        /// <summary>
        /// Insert alternative menu.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        public CommandResult InsertAlternativeMenu(InsertAlternativeMenuCriteria criteria)
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
        /// Update alternative menu.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        public CommandResult UpdateAlternativeMenu(UpdateAlternativeMenuCriteria criteria)
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
        /// Activate or deactivate alternative menu.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        public CommandResult ActivateAlternativeMenu(ActivateAlternativeMenuCriteria criteria)
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
        /// Delete alternative menu.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        public CommandResult DeleteAlternativeMenu(DeleteAlternativeMenuCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            CommandResult commandResult = new CommandResult();

            if (criteria.IsValid())
            {
                commandResult = Repository.Delete(criteria);
            }

            return commandResult;
        }
    }
}
