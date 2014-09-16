// --------------------------------------------------------------------------
// <copyright file="OrderHub.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.9.15">Created</change>
// </history>

namespace NexFB.Hub
{
    using Microsoft.AspNet.SignalR;
    using Microsoft.AspNet.SignalR.Hubs;
    using NexFB.Criteria.Ordering;
    using NexFB.Models.Ordering;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;

    /// <summary>
    /// The Order hub.
    /// </summary>
    [HubName("order")]
    public class OrderHub : Hub
    {
        private readonly IOrderingService _orderService;

        public OrderHub()
            : this(null)
        {
        }

        public OrderHub(IOrderingService orderService)
        {
            _orderService = orderService ?? new OrderingService();
        }

        public void SendMessage(string message)
        {
            Clients.All.newMessage(message);
        }

        public void SendNewOrder(int orderId)
        {
            SendNewOrderForWeb(orderId);
            SendNewOrderForApp(orderId);
        }

        private void SendNewOrderForWeb(int orderId)
        {
            var criteria = new GetMyOrderByIdCriteria()
            {
                OrderId = orderId
            };

            MealOrderingModel order = _orderService.GetMyOrderById(criteria);
            Clients.All.newOrderForWeb(order);
        }

        private void SendNewOrderForApp(int orderId)
        {
            var criteria = new GetMyOrderByIdCriteria()
            {
                OrderId = orderId
            };

            MealOrderingModel order = _orderService.GetMyOrderById(criteria);
            Clients.All.newOrderForApp(order);
        }
    }
}
