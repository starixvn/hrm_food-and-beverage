// --------------------------------------------------------------------------
// <copyright file="OrderMemberController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.9.18">Created</change>
// </history>

namespace NexFB.Site.Controllers.API
{
    using System.Web.Http;
    using NexFB.Core.Constants;
    using NexFB.Core.Enums;
    using NexFB.Core.Models;
    using NexFB.Criteria.Ordering;
    using NexFB.Models.Ordering;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;
    using NexFB.Web.Attributes;

    /// <summary>
    /// OrderMember Controller.
    /// </summary>
    public class OrderMemberController : BaseController
    {
        private readonly IOrderingService _orderService;

        public OrderMemberController()
            : this(null)
        {
        }

        public OrderMemberController(IOrderingService orderService)
        {
            _orderService = orderService ?? new OrderingService();
        }

        [HttpPost]
        [ApiAuthorize(FunctionKey = FBFunctionKey.Order, Policy = Policy.Host)]
        public CommandResult Post(OrderMemberModel member)
        {
            var criteria = new AddMemberCriteria()
            {
                UserId = this.CurrentUser.UserId,
                MemberId = member.Member.UserId
            };

            CommandResult result = _orderService.AddMember(criteria);
            return result;
        }

        [ApiAuthorize(FunctionKey = FBFunctionKey.Order, Policy = Policy.Host)]
        public CommandResult Delete(int memberId)
        {
            var criteria = new RemoveMemberCriteria()
            {
                UserId = this.CurrentUser.UserId,
                MemberId = memberId
            };

            CommandResult result = _orderService.RemoveMember(criteria);
            return result;
        }
    }
}