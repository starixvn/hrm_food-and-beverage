// --------------------------------------------------------------------------
// <copyright file="SetMenuService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.10.22">Created</change>
// </history>

namespace NexFB.Services.Implement
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common.Helper;
    using NexFB.Core.Models;
    using NexFB.Core.Services;
    using NexFB.Criteria.SetMenuManagement;
    using NexFB.Models.SetMenuManagement;
    using NexFB.Repositories.Implement;
    using NexFB.Repositories.Interface;
    using NexFB.Resources;
    using NexFB.Services.Interface;

    /// <summary>
    /// The set menu service.
    /// </summary>
    public class SetMenuService : BaseService, ISetMenuService
    {
        private readonly ISetMenuRepository _setMenuRepository;

        public SetMenuService()
            : this(null)
        {
        }

        public SetMenuService(ISetMenuRepository setMenuRepository)
        {
            _setMenuRepository = setMenuRepository ?? new SetMenuRepository();
        }

        /// <summary>
        /// Insert set menu.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The Command Result.</returns>
        public CommandResult InsertSetMenu(InsertSetMenuCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            CommandResult result = null;

            if (criteria.IsValid())
            {
                result = Repository.Insert(criteria);
            }
            else
            {
                result = new CommandResult()
                {
                    Code = 1,
                    Message = Lang.InvalidInputData
                };
            }

            return result;
        }

        /// <summary>
        /// Update set menu.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        public CommandResult UpdateSetMenu(UpdateSetMenuCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            CommandResult result = null;

            if (criteria.IsValid())
            {
                result = Repository.Update(criteria);
            }
            else
            {
                result = new CommandResult()
                {
                    Code = 1,
                    Message = Lang.InvalidInputData
                };
            }

            return result;
        }

        /// <summary>
        /// Delete set menu.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        public CommandResult DeleteSetMenu(DeleteSetMenuCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            CommandResult result = null;

            if (criteria.IsValid())
            {
                result = Repository.Delete(criteria);
            }
            else
            {
                result = new CommandResult()
                {
                    Code = 1,
                    Message = Lang.InvalidInputData
                };
            }

            return result;
        }

        /// <summary>
        /// Update status of set menu.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        public CommandResult UpdateStatusSetMenu(UpdateStatusSetMenuCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            CommandResult result = null;

            if (criteria.IsValid())
            {
                result = Repository.Update(criteria);
            }
            else
            {
                result = new CommandResult()
                {
                    Code = 1,
                    Message = Lang.InvalidInputData
                };
            }

            return result;
        }

        /// <summary>
        /// Function to get Set Menu by id.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>SetMenu Model.</returns>
        public SetMenuModel GetOrderById(GetSetMenuByIdCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            SetMenuModel setMenu = null;
            if (criteria.IsValid())
            {
                IList<SetMenuModel> setMenus = _setMenuRepository.GetSetMenu(criteria);
                if (setMenus != null && setMenus.Any())
                {
                    setMenu = setMenus.SingleOrDefault();
                    setMenu.RawFoodIds = setMenu.FoodItems.Select(t => t.FoodId).ToList();
                    setMenu.RawMenuIds = setMenu.Menus.Select(t => t.MenuId).ToList();
                }
            }

            return setMenu;
        }

        /// <summary>
        /// Function to get set menus.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The list Set Menu model.</returns>
        public IList<SetMenuModel> GetSetMenu(GetSetMenuCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            IList<SetMenuModel> setMenus = null;

            if (criteria.IsValid())
            {
                setMenus = _setMenuRepository.GetSetMenu(criteria);
            }

            return setMenus;
        }
    }
}
