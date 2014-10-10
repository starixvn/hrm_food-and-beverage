// ----------------------------------------------------------------------
// <copyright file="AlternativeMenuController.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="10/10/2014 2:39:40 PM">Created.</change>
// </history>

namespace NexFB.Site.Controllers.API
{
    using System.Collections.Generic;
    using NexFB.Criteria.AlternativeMenuManagement;
    using NexFB.Models.AlternativeMenuManagement;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;

    /// <summary>
    /// The alternative menu controller.
    /// </summary>
    public class AlternativeMenuController : BaseController
    {
        private readonly IAlternativeMenuService _alternativeMenuService;

        public AlternativeMenuController()
            : this(null)
        {
        }

        public AlternativeMenuController(IAlternativeMenuService alternativeMenuService)
        {
            _alternativeMenuService = alternativeMenuService ?? new AlternativeMenuService();
        }

        /// <summary>
        /// Get all alternative menus.
        /// </summary>
        /// <param name="alternativeMenuId">The criteria.</param>
        /// <param name="isActive">Is active or not.</param>
        /// <returns>The enumerator Alternative Menu model.</returns>
        public IEnumerable<AlternativeMenuModel> GetAllAlternativeMenu(
            int? alternativeMenuId = null,
            bool? isActive = null)
        {
            var criteria = new GetAllAlternativeMenuCriteria
            {
                AlternativeMenuId = alternativeMenuId,
                IsActive = isActive
            };

            return _alternativeMenuService.GetAllAlternativeMenu(criteria);
        }
    }
}