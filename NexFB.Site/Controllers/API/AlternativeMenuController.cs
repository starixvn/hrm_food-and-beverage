namespace NexFB.Site.Controllers.API
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;
    using NexFB.Core.Constants;
    using NexFB.Core.Enums;
    using NexFB.Core.Models;
    using NexFB.Criteria.AlternativeMenuManagement;
    using NexFB.Models.AlternativeMenuManagement;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;
    using NexFB.Web.Attributes;
    using NexFB.Web.Services.Implement;
    using NexFB.Web.Services.Interface;

    /// <summary>
    /// The alternative menu controller.
    /// </summary>
    public class AlternativeMenuController : BaseController
    {
        private readonly IAlternativeMenuService _alternativeMenuService;
        private readonly IWebCommonService _webCommonService;

        public AlternativeMenuController()
            : this(null, null)
        {
        }

        public AlternativeMenuController(
            IAlternativeMenuService alternativeMenuService,
            IWebCommonService webCommonService)
        {
            _alternativeMenuService = alternativeMenuService ?? new AlternativeMenuService();
            _webCommonService = webCommonService ?? new WebCommonService();
        }

        /// <summary>
        /// Get all alternative menus.
        /// </summary>
        /// <param name="alternativeMenuId">The criteria.</param>
        /// <param name="isActive">Is active or not.</param>
        /// <returns>The enumerator Alternative Menu model.</returns>
        [ApiAuthorize(FunctionKey = FBFunctionKey.AlternativeMenuManagement, Policy = Policy.Read)]
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

        /// <summary>
        /// Insert alternative menu.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        [HttpPost]
        [ApiAuthorize(FunctionKey = FBFunctionKey.AlternativeMenuManagement, Policy = Policy.Host)]
        public CommandResult InsertAlternativeMenu(InsertAlternativeMenuCriteria criteria)
        {
            if (criteria != null)
            {
                criteria.CreatedBy = this.CurrentUser.UserId;
                criteria.CreatedDate = DateTime.Now;
            }

            return _alternativeMenuService.InsertAlternativeMenu(criteria);
        }

        /// <summary>
        /// Update alternative menu.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        [HttpPut]
        [ApiAuthorize(FunctionKey = FBFunctionKey.AlternativeMenuManagement, Policy = Policy.Host)]
        public CommandResult UpdateAlternativeMenu(UpdateAlternativeMenuCriteria criteria)
        {
            if (criteria != null)
            {
                criteria.ModifiedBy = this.CurrentUser.UserId;
                criteria.ModifiedDate = DateTime.Now;
            }

            return _alternativeMenuService.UpdateAlternativeMenu(criteria);
        }

        /// <summary>
        /// Activate or deactivate alternative menu.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        [HttpPut]
        [ApiAuthorize(FunctionKey = FBFunctionKey.AlternativeMenuManagement, Policy = Policy.Host)]
        public CommandResult ActivateAlternativeMenu(ActivateAlternativeMenuCriteria criteria)
        {
            if (criteria != null)
            {
                criteria.ModifiedBy = this.CurrentUser.UserId;
                criteria.ModifiedDate = DateTime.Now;
            }

            return _alternativeMenuService.ActivateAlternativeMenu(criteria);
        }

        /// <summary>
        /// Delete alternative menu.
        /// </summary>
        /// <param name="alternativeMenuId">The alternative menu Id.</param>
        /// <returns>The command result.</returns>
        [HttpDelete]
        [ApiAuthorize(FunctionKey = FBFunctionKey.AlternativeMenuManagement, Policy = Policy.Host)]
        public CommandResult DeleteAlternativeMenu(int alternativeMenuId) 
        {
            var criteria = new DeleteAlternativeMenuCriteria
            {
                AlternativeMenuId = alternativeMenuId,
                ModifiedBy = this.CurrentUser.UserId,
                ModifiedDate = DateTime.Now
            };

            return _alternativeMenuService.DeleteAlternativeMenu(criteria);
        }

        public int GetUserPermissionOnAlternativeMenu()
        {
            int policy = _webCommonService.GetPermission(FBFunctionKey.AlternativeMenuManagement);
            return policy;
        }
    }
}
